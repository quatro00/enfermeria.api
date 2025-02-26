namespace enfermeria.api.Models.DTO.Auth
{
    public class ChangePasswordRequestDto
    {
        public required string username { get; set; }
        public required string currentPassword { get; set; }
        public required string newPassword { get; set; }
    }
}
