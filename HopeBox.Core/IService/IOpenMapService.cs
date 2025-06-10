using HopeBox.Domain.ResponseDto;

namespace HopeBox.Core.IService
{
    public interface IOpenMapService
    {
        Task<List<OpenMapPlaceSuggestionDto>> AutocompleteAsync(string keyword, string? sessionToken = null);
        Task<OpenMapPlaceDetailDto?> GetPlaceDetailAsync(string placeId, string? sessionToken = null);
        Task<(double? latitude, double? longitude, string? formattedAddress)> GetCoordinatesFromAddressAsync(string address);
        Task<string?> GetAddressFromCoordinatesAsync(double latitude, double longitude);
        Task<bool> ValidateCoordinatesAsync(double latitude, double longitude);
    }
}
