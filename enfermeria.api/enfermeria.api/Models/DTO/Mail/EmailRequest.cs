using System.Net.Mail;

namespace enfermeria.api.Models.DTO.Mail
{
    public class EmailRequest
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public List<string> ToMultiple { get; set; } = new List<string>();
        public List<Attachment> Attachments { get; set; } = new();
    }
}
