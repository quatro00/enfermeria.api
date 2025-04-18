using enfermeria.api.Models.DTO.Mail;
using enfermeria.api.Repositories.Interface;
using System.Net.Mail;
using System.Net;

namespace enfermeria.api.Repositories.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        string token = "abcdef123456"; // Este es tu token único generado para esa cotización
        string cotizacionId = ""; // ID de la cotización, si lo necesitas como parte de validación

        string aceptarUrl = $"https://tudominio.com/api/cotizaciones/responder?token=eeee&respuesta=aceptado";
        string rechazarUrl = $"https://tudominio.com/api/cotizaciones/responder?token=errr&respuesta=rechazado";

        string htmlBody = $@"
    <html>
    <body style='font-family: Arial, sans-serif;'>
        <h2>Revisión de Cotización</h2>
        <p>Estimado cliente,</p>
        <p>Gracias por solicitar una cotización con nuestra agencia. Por favor, revisa la propuesta y confirma tu decisión.</p>
        
        <div style='margin-top: 20px; margin-bottom: 30px;'>
            <a href='#' 
               style='padding: 10px 20px; background-color: #28a745; color: white; text-decoration: none; border-radius: 5px; margin-right: 15px;'>
               ✅ Aceptar Cotización
            </a>
            <a href='#' 
               style='padding: 10px 20px; background-color: #dc3545; color: white; text-decoration: none; border-radius: 5px;'>
               ❌ Rechazar Cotización
            </a>
        </div>

        <p>Si tienes dudas o deseas más información, no dudes en contactarnos.</p>
        <p>Atentamente,<br><strong>Agencia de Enfermería</strong></p>
    </body>
    </html>";

        public EmailService(EmailSettings settings)
        {
            _settings = settings;
        }

        public async Task SendEmailAsync(EmailRequest request)
        {
            using var message = new MailMessage
            {
                From = new MailAddress(_settings.Usuario),
                Subject = request.Subject,
                Body = htmlBody,
                IsBodyHtml = true
            };

            foreach (var toAddress in request.ToMultiple)
            {
                message.To.Add(toAddress);
            }

            foreach (var attachment in request.Attachments)
            {
                message.Attachments.Add(attachment);
            }

            using var smtp = new SmtpClient(_settings.Dominio)
            {
                Port = _settings.Puerto,
                Credentials = new NetworkCredential(_settings.Usuario, _settings.Contrasena),
                EnableSsl = _settings.EnableSsl
            };

            await smtp.SendMailAsync(message);
        }
    }
}
