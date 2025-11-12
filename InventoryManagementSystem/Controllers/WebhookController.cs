using InventoryManagementSystem.DataAccess.Repository.IRepository;
using InventoryManagementSystem.Services.PaymentService;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace InventoryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhookController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;
        private readonly ILogger<WebhookController> _logger;

        public WebhookController(
            IUnitOfWork unitOfWork,
            IPaymentService paymentService,
            ILogger<WebhookController> logger)
        {
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpPost("instapay")]
        public async Task<IActionResult> HandleInstapayWebhook()
        {
            try
            {
                // في الإنتاج هتستخدم الـ webhook secret للتحقق
                var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                _logger.LogInformation("Instapay webhook received: {Json}", json);

                var webhookEvent = JsonSerializer.Deserialize<InstapayWebhookEvent>(json);

                if (webhookEvent?.Type == "payment_intent.succeeded")
                {
                    var paymentIntentId = webhookEvent.Data?.Object?.Id;

                    if (!string.IsNullOrEmpty(paymentIntentId))
                    {
                        // البحث عن الـ Order بالـ Payment Intent ID
                        var orders = _unitOfWork.OrderRepository.GetAllOrders();
                        var order = orders.FirstOrDefault(o => o.PaymentIntentId == paymentIntentId);

                        if (order != null)
                        {
                            order.PaymentStatus = "Paid";
                            order.OrderStatus = "Confirmed";
                            _unitOfWork.OrderRepository.UpdateOrder(order);
                            _unitOfWork.Save();

                            _logger.LogInformation("Order {OrderId} payment confirmed via webhook", order.OrderId);
                        }
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Instapay webhook");
                return StatusCode(500);
            }
        }
    }

    public class InstapayWebhookEvent
    {
        public string Type { get; set; } = string.Empty;
        public WebhookData Data { get; set; } = new WebhookData();
    }

    public class WebhookData
    {
        public PaymentIntentObject Object { get; set; } = new PaymentIntentObject();
    }

    public class PaymentIntentObject
    {
        public string Id { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}