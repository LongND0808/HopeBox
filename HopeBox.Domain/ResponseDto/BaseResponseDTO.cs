namespace HopeBox.Domain.ResponseDto
{
    public class BaseResponseDTO<T>
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
