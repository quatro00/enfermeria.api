namespace enfermeria.api.Models.Stripe
{
    public class PaymentIntentRequest
    {
        public long Amount { get; set; } // Monto en centavos
        public string Currency { get; set; }
        public string Description { get; set; }
    }
}
