namespace enfermeria.api.Models.DTO.Auth
{
    public class RestorePasswordRequestDto
    {
        public required string newPassword { get; set; }
        public required string token { get; set; }
    }
}
