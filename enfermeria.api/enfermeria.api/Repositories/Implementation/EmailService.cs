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

       

        public EmailService(EmailSettings settings)
        {
            _settings = settings;
        }

        public async Task SendEmailAsync(EmailRequest request, string paymentUrl)
        {
            string htmlBody = $@"
            <div style='font-family: Arial, sans-serif; background-color: #f5f5f5; padding: 30px;'>
              <div style='max-width: 600px; margin: auto; background-color: white; border-radius: 8px; padding: 30px; box-shadow: 0 0 10px rgba(0,0,0,0.05);'>

                <h2 style='color: #2c3e50;'>¡Hola!</h2>
                <p style='font-size: 16px; color: #333;'>
                  Reciba un cordial saludo de parte de <strong style='color: #2980b9;'>Enfermería AlfaCare</strong>.
                </p>

                <p style='font-size: 16px; color: #333;'>
                  En el archivo adjunto encontrará el <strong>PDF con la cotización detallada</strong> del servicio solicitado.
                </p>

                <p style='font-size: 16px; color: #333;'>
                  A continuación, puede acceder al enlace para realizar su pago de manera segura a través de nuestra plataforma:
                </p>

                <div style='text-align: center; margin: 30px 0;'>
                  <a href='{paymentUrl}' target='_blank' style='background-color: #27ae60; color: white; padding: 15px 25px; text-decoration: none; font-size: 16px; border-radius: 5px; display: inline-block;'>
                    Ir al enlace de pago
                  </a>
                </div>

                <p style='font-size: 14px; color: #999; text-align: center;'>
                  Si el botón anterior no funciona, también puede copiar y pegar el siguiente enlace en su navegador:
                </p>
                <p style='font-size: 14px; color: #2980b9; word-break: break-word; text-align: center;'>
                  {paymentUrl}
                </p>

                <hr style='margin: 30px 0; border: none; border-top: 1px solid #ddd;'>

                <p style='font-size: 14px; color: #555;'>
                  <strong>Enfermería AlfaCare</strong><br>
                  Av. Salud 123, Colonia Bienestar, Monterrey, NL, México<br>
                  Tel: <a href='tel:8180000000' style='color: #2980b9;'>(81) 8000-0000</a> | <a href='tel:8123456789' style='color: #2980b9;'>(81) 2345-6789</a><br>
                  Correo: <a href='mailto:contacto@alfacare.com' style='color: #2980b9;'>contacto@alfacare.com</a><br>
                  Sitio web: <a href='https://www.alfacare.com' style='color: #2980b9;'>www.alfacare.com</a>
                </p>

                <p style='font-size: 13px; color: #aaa; text-align: center; margin-top: 40px;'>
                  © 2025 Enfermería AlfaCare. Todos los derechos reservados.
                </p>
              </div>
            </div>";

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

        public async Task SendEmailAsync_Cuenta(EmailRequest request, string cuenta)
        {
            string htmlBody = $@"
            <div style='font-family: Arial, sans-serif; background-color: #f5f5f5; padding: 30px;'>
              <div style='max-width: 600px; margin: auto; background-color: white; border-radius: 8px; padding: 30px; box-shadow: 0 0 10px rgba(0,0,0,0.05);'>

                <h2 style='color: #2c3e50;'>¡Hola!</h2>
                <p style='font-size: 16px; color: #333;'>
                  Reciba un cordial saludo de parte de <strong style='color: #2980b9;'>Enfermería AlfaCare</strong>.
                </p>

                <p style='font-size: 16px; color: #333;'>
                  En el archivo adjunto encontrará el <strong>PDF con la cotización detallada</strong> del servicio solicitado.
                </p>

                <p style='font-size: 16px; color: #333;'>
                  Debido al monto el unico pago disponible seria transferencia bancaria a la siguiente cuenta:
                </p>

                <div style='text-align: center; margin: 30px 0;'>
                  {cuenta}
                </div>

                <hr style='margin: 30px 0; border: none; border-top: 1px solid #ddd;'>

                <p style='font-size: 14px; color: #555;'>
                  <strong>Enfermería AlfaCare</strong><br>
                  Av. Salud 123, Colonia Bienestar, Monterrey, NL, México<br>
                  Tel: <a href='tel:8180000000' style='color: #2980b9;'>(81) 8000-0000</a> | <a href='tel:8123456789' style='color: #2980b9;'>(81) 2345-6789</a><br>
                  Correo: <a href='mailto:contacto@alfacare.com' style='color: #2980b9;'>contacto@alfacare.com</a><br>
                  Sitio web: <a href='https://www.alfacare.com' style='color: #2980b9;'>www.alfacare.com</a>
                </p>

                <p style='font-size: 13px; color: #aaa; text-align: center; margin-top: 40px;'>
                  © 2025 Enfermería AlfaCare. Todos los derechos reservados.
                </p>
              </div>
            </div>";

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
