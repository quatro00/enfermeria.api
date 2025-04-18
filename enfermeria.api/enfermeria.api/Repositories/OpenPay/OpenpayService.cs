namespace enfermeria.api.Repositories.OpenPay
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class OpenpayService
    {
        private readonly string _merchantId = "mmywxjjvpl3ifpcg7x3y";  // Reemplaza con tu Merchant ID
        private readonly string _privateKey = "sk_cae894e0d7074711a595410f7c10c504";  // Reemplaza con tu Private Key
        private readonly string _apiUrl = "https://sandbox-api.openpay.mx/v1";  // URL del entorno sandbox de Openpay

        private HttpClient _httpClient;

        public OpenpayService()
        {
            _httpClient = new HttpClient();
        }


        // Método para crear un cargo (payment)
        public async Task<string> CreateChargeAsync(decimal amount, string description)
        {
            var url = $"{_apiUrl}/{_merchantId}/checkout/orders";

            var payload = new
            {
                amount = amount,
                currency = "MXN",
                description = description,
                order_id = Guid.NewGuid().ToString(),
                customer = new
                {
                    name = "Cliente genérico", // Puedes pedir estos datos
                    email = "cliente@correo.com"
                },
                redirect_url = "https://tu-dominio.com/pago-exitoso" // opcional, para redirigir al finalizar el pago
            };

            var jsonPayload = JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var byteArray = Encoding.ASCII.GetBytes($"{_privateKey}:");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                dynamic json = JsonConvert.DeserializeObject(responseData);
                return json.payment_url; // ESTE es el link de pago
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al generar link de pago: {error}");
            }

        }
    }

}
