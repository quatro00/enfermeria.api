using enfermeria.api.Models.Domain;
using enfermeria.api.Models.DTO.Mail;

namespace enfermeria.api.Repositories.Interface
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequest request, string paymentUrl);
        Task SendEmailAsync_Cuenta(EmailRequest request, string cuenta);

        Task SendEmailAsync_NuevoMensaje(Mensaje mensaje);

        Task SendEmailAsync_RecuperarPassword(string linkRecuperacion, string correoElectronico);
    }
}
