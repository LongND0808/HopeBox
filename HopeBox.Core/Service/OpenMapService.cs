using HopeBox.Core.IService;
using HopeBox.Domain.ResponseDto;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace HopeBox.Infrastructure.Service
{
    public class OpenMapService : IOpenMapService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly ILogger<OpenMapService> _logger;
        private const string BaseUrl = "https://mapapis.openmap.vn/v1";

        public OpenMapService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<OpenMapService> logger)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenMap:ApiKey"] ?? throw new ArgumentNullException("OpenMap:ApiKey not configured");
            _logger = logger;
        }

        public async Task<List<OpenMapPlaceSuggestionDto>> AutocompleteAsync(string keyword, string? sessionToken = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    _logger.LogWarning("Keyword is null or empty for autocomplete");
                    return new List<OpenMapPlaceSuggestionDto>();
                }

                var url = $"{BaseUrl}/autocomplete?text={Uri.EscapeDataString(keyword)}&format=google";
                if (!string.IsNullOrEmpty(sessionToken))
                    url += $"&sessiontoken={sessionToken}";

                _httpClient.DefaultRequestHeaders.Remove("x-api-key");
                _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("OpenMap autocomplete API failed with status: {StatusCode}", response.StatusCode);
                    return new List<OpenMapPlaceSuggestionDto>();
                }

                var json = await response.Content.ReadAsStringAsync();
                var document = JsonDocument.Parse(json);

                var suggestions = new List<OpenMapPlaceSuggestionDto>();

                if (document.RootElement.TryGetProperty("features", out var features))
                {
                    foreach (var feature in features.EnumerateArray())
                    {
                        if (feature.TryGetProperty("properties", out var props))
                        {
                            suggestions.Add(new OpenMapPlaceSuggestionDto
                            {
                                Id = props.TryGetProperty("id", out var id) ? id.GetString() ?? string.Empty : string.Empty,
                                Name = props.TryGetProperty("name", out var name) ? name.GetString() ?? string.Empty : string.Empty,
                                Label = props.TryGetProperty("label", out var label) ? label.GetString() ?? string.Empty : string.Empty,
                                ShortAddress = props.TryGetProperty("short_address", out var sa) ? sa.GetString() : null,
                                Country = props.TryGetProperty("country", out var country) ? country.GetString() : null,
                                Region = props.TryGetProperty("region", out var region) ? region.GetString() : null,
                                Locality = props.TryGetProperty("locality", out var locality) ? locality.GetString() : null
                            });
                        }
                    }
                }

                _logger.LogInformation("Successfully retrieved {Count} suggestions for keyword: {Keyword}", suggestions.Count, keyword);
                return suggestions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting autocomplete suggestions for keyword: {Keyword}", keyword);
                return new List<OpenMapPlaceSuggestionDto>();
            }
        }

        public async Task<OpenMapPlaceDetailDto?> GetPlaceDetailAsync(string placeId, string? sessionToken = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(placeId))
                {
                    _logger.LogWarning("PlaceId is null or empty");
                    return null;
                }

                var url = $"{BaseUrl}/place?ids={placeId}&format=google";
                if (!string.IsNullOrEmpty(sessionToken))
                    url += $"&sessiontoken={sessionToken}";

                _httpClient.DefaultRequestHeaders.Remove("x-api-key");
                _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("OpenMap place detail API failed with status: {StatusCode}", response.StatusCode);
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                var document = JsonDocument.Parse(json);

                if (!document.RootElement.TryGetProperty("features", out var features) || features.GetArrayLength() == 0)
                {
                    _logger.LogWarning("No place details found for placeId: {PlaceId}", placeId);
                    return null;
                }

                var feature = features[0];
                var props = feature.GetProperty("properties");
                var geometry = feature.GetProperty("geometry");
                var coordinates = geometry.GetProperty("coordinates");

                var placeDetail = new OpenMapPlaceDetailDto
                {
                    Id = props.TryGetProperty("id", out var id) ? id.GetString() ?? string.Empty : string.Empty,
                    Name = props.TryGetProperty("name", out var name) ? name.GetString() ?? string.Empty : string.Empty,
                    Label = props.TryGetProperty("label", out var label) ? label.GetString() ?? string.Empty : string.Empty,
                    Longitude = coordinates[0].GetDouble(),
                    Latitude = coordinates[1].GetDouble(),
                    Country = props.TryGetProperty("country", out var country) ? country.GetString() : null,
                    Region = props.TryGetProperty("region", out var region) ? region.GetString() : null,
                    Locality = props.TryGetProperty("locality", out var locality) ? locality.GetString() : null,
                    ShortAddress = props.TryGetProperty("short_address", out var sa) ? sa.GetString() : null
                };

                _logger.LogInformation("Successfully retrieved place detail for placeId: {PlaceId}", placeId);
                return placeDetail;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting place detail for placeId: {PlaceId}", placeId);
                return null;
            }
        }

        public async Task<(double? latitude, double? longitude, string? formattedAddress)> GetCoordinatesFromAddressAsync(string address)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(address))
                {
                    _logger.LogWarning("Address is null or empty for geocoding");
                    return (null, null, null);
                }

                // Sử dụng autocomplete để tìm địa chỉ
                var suggestions = await AutocompleteAsync(address);
                if (!suggestions.Any())
                {
                    _logger.LogWarning("No suggestions found for address: {Address}", address);
                    return (null, null, null);
                }

                // Lấy chi tiết của suggestion đầu tiên
                var firstSuggestion = suggestions.First();
                var placeDetail = await GetPlaceDetailAsync(firstSuggestion.Id);

                if (placeDetail != null)
                {
                    return (placeDetail.Latitude, placeDetail.Longitude, placeDetail.Label);
                }

                return (null, null, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while geocoding address: {Address}", address);
                return (null, null, null);
            }
        }

        public async Task<string?> GetAddressFromCoordinatesAsync(double latitude, double longitude)
        {
            try
            {
                var url = $"{BaseUrl}/reverse?point.lat={latitude}&point.lon={longitude}&format=google";

                _httpClient.DefaultRequestHeaders.Remove("x-api-key");
                _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("OpenMap reverse geocoding failed with status: {StatusCode}", response.StatusCode);
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                var document = JsonDocument.Parse(json);

                if (document.RootElement.TryGetProperty("features", out var features) && features.GetArrayLength() > 0)
                {
                    var feature = features[0];
                    if (feature.TryGetProperty("properties", out var props) &&
                        props.TryGetProperty("label", out var label))
                    {
                        return label.GetString();
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while reverse geocoding coordinates: {Lat}, {Lng}", latitude, longitude);
                return null;
            }
        }

        public async Task<bool> ValidateCoordinatesAsync(double latitude, double longitude)
        {
            if (latitude < -90 || latitude > 90 || longitude < -180 || longitude > 180)
            {
                return false;
            }

            try
            {
                var address = await GetAddressFromCoordinatesAsync(latitude, longitude);
                return !string.IsNullOrEmpty(address);
            }
            catch
            {
                return false;
            }
        }
    }
}
