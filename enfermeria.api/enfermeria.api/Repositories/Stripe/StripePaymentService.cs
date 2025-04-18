using Stripe.Checkout;
using Stripe;

namespace enfermeria.api.Repositories.Stripe
{
    public class StripePaymentService
    {
        public StripePaymentService(string apiKey)
        {
            StripeConfiguration.ApiKey = apiKey; // Tu clave secreta de Stripe
        }

        public async Task<string> CreateCheckoutSessionAsync(decimal amount, string description, string successUrl, string cancelUrl, string referencia)
        {
            try
            {
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string>
            {
                "card",     // tarjeta
                "oxxo"      // para permitir pagos en OXXO
            },
                    LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(amount * 100), // en centavos
                        Currency = "mxn",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = description
                        },
                    },
                    Quantity = 1,
                },
            },
                    Mode = "payment",
                    SuccessUrl = successUrl + "?session_id={CHECKOUT_SESSION_ID}",
                    CancelUrl = cancelUrl,
                    ClientReferenceId = referencia
                };

                var service = new SessionService();
                Session session = await service.CreateAsync(options);

                return session.Url;
            }
            catch (Exception ex) {
                throw ex;
            }

           
        }
    }
}
