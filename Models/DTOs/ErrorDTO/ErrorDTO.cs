namespace MyShopPet.Models.DTOs.ErrorDTO
{
    public class ErrorDTO
    {
        public string Message { get; init; } = default!;

        public int StatusCode { get; init; } = default!;
    }
}
