using InventoryManagementSystem.Models.Entities;
using InventoryManagementSystem.Models.ViewModels;
using System.Text;
using System.Text.Json;

namespace InventoryManagementSystem.Services.PaymentService
{
    public class InstapayPaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly InstapayConfig _config;
        private readonly ILogger<InstapayPaymentService> _logger;

        public InstapayPaymentService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<InstapayPaymentService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            // Load configuration
            _config = new InstapayConfig
            {
                ApiKey = configuration["Instapay:ApiKey"] ?? "test_sk_yourapikey",
                BaseUrl = configuration["Instapay:BaseUrl"] ?? "https://api.instapay.com/v1/",
                WebhookSecret = configuration["Instapay:WebhookSecret"] ?? "whsec_yoursecret",
                SuccessUrl = configuration["Instapay:SuccessUrl"] ?? "https://localhost:7001/Checkout/PaymentSuccess",
                CancelUrl = configuration["Instapay:CancelUrl"] ?? "https://localhost:7001/Checkout/PaymentFailed"
            };

            // Setup HttpClient
            _httpClient.BaseAddress = new Uri(_config.BaseUrl);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_config.ApiKey}");
        }

        public async Task<PaymentResult> ProcessInstapayPaymentAsync(Order order)
        {
            try
            {
                // Mock payment for development - في الإنتاج هتتعامل مع الـ API الحقيقي
                if (_config.ApiKey.StartsWith("test_"))
                {
                    return await ProcessMockInstapayPaymentAsync(order);
                }

                // الـ API الحقيقي هيبقى كده:
                var paymentRequest = new
                {
                    amount = (int)(order.OrderTotal * 100), // Convert to cents
                    currency = "EGP",
                    order_id = order.OrderId.ToString(),
                    customer = new
                    {
                        email = order.ShippingEmail,
                        name = order.ShippingFullName,
                        phone = order.ShippingPhone
                    },
                    success_url = $"{_config.SuccessUrl}?orderId={order.OrderId}",
                    cancel_url = $"{_config.CancelUrl}?orderId={order.OrderId}",
                    metadata = new
                    {
                        order_id = order.OrderId,
                        customer_id = order.ApplicationUserId
                    }
                };

                var json = JsonSerializer.Serialize(paymentRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("payment-intents", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var paymentResponse = JsonSerializer.Deserialize<InstapayPaymentResponse>(responseContent);

                    return new PaymentResult
                    {
                        Success = true,
                        PaymentUrl = paymentResponse?.CheckoutUrl ?? "",
                        PaymentIntentId = paymentResponse?.Id ?? ""
                    };
                }
                else
                {
                    _logger.LogError("Instapay API error: {StatusCode} - {Content}",
                        response.StatusCode, await response.Content.ReadAsStringAsync());

                    return new PaymentResult
                    {
                        Success = false,
                        ErrorMessage = "Payment processing failed. Please try again."
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Instapay payment for order {OrderId}", order.OrderId);
                return new PaymentResult
                {
                    Success = false,
                    ErrorMessage = "An error occurred while processing payment."
                };
            }
        }

        // Mock payment for development
        private async Task<PaymentResult> ProcessMockInstapayPaymentAsync(Order order)
        {
            await Task.Delay(1000); // Simulate API call

            var mockPaymentIntentId = $"pi_mock_{order.OrderId}_{DateTime.Now.Ticks}";
            var mockPaymentUrl = $"/Checkout/MockPayment?orderId={order.OrderId}&paymentIntentId={mockPaymentIntentId}";

            return new PaymentResult
            {
                Success = true,
                PaymentUrl = mockPaymentUrl,
                PaymentIntentId = mockPaymentIntentId
            };
        }

        public async Task<bool> VerifyInstapayPaymentAsync(string paymentIntentId)
        {
            try
            {
                // Mock verification for development
                if (_config.ApiKey.StartsWith("test_"))
                {
                    await Task.Delay(500);
                    return true; // Always return success in mock mode
                }

                // الـ API الحقيقي
                var response = await _httpClient.GetAsync($"payment-intents/{paymentIntentId}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var paymentIntent = JsonSerializer.Deserialize<InstapayPaymentResponse>(responseContent);

                    return paymentIntent?.Status == "succeeded";
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying Instapay payment {PaymentIntentId}", paymentIntentId);
                return false;
            }
        }

        public async Task<PaymentResult> ProcessWalletPaymentAsync(Order order)
        {
            // Mock wallet payment - هنعمله في الجزء الرابع
            await Task.Delay(1000);

            return new PaymentResult
            {
                Success = true,
                PaymentUrl = $"/Checkout/MockWalletPayment?orderId={order.OrderId}",
                PaymentIntentId = $"wallet_mock_{order.OrderId}"
            };
        }
    }

    public class InstapayPaymentResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string CheckoutUrl { get; set; } = string.Empty;
        public long Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
    }
}