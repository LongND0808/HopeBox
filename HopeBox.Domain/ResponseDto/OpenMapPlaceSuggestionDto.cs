namespace HopeBox.Domain.ResponseDto
{
    public class OpenMapPlaceSuggestionDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string? ShortAddress { get; set; }
        public string? Country { get; set; }
        public string? Region { get; set; }
        public string? Locality { get; set; }
    }
}
