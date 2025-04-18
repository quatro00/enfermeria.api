namespace enfermeria.api.Models.DTO.Mail
{
    public class EmailSettings
    {
        public string Dominio { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public int Puerto { get; set; }
        public bool EnableSsl { get; set; }
    }
}
