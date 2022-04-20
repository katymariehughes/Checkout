using Gateway.API.Infrastructure.Swagger;
using Gateway.API.Models;
using Gateway.API.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace Gateway.API.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class PaymentsController : ControllerBase
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly IPaymentService _paymentsService;

        public PaymentsController(ILogger<PaymentsController> logger, IPaymentService paymentsService)
        {
            _logger = logger;
            _paymentsService = paymentsService;
        }

        [HttpPost("payments")]
        [SwaggerRequestExample(typeof(PaymentRequest), typeof(PaymentRequestExample))]
        [ProducesResponseType(typeof(PaymentResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ProcessPayment(PaymentRequest request)
        {
            if (Request.HttpContext.Items?.TryGetValue("MerchantId", out var merchantIdValue) != true)
                return BadRequest(new { Message = "No merchant ID could be found" });

            var paymentId = await _paymentsService.InitiatePaymentFlow(request, Guid.Parse(merchantIdValue.ToString()));

            return CreatedAtAction(nameof(RetrievePaymentDetails), new { id = paymentId }, new PaymentResponse
            {
                Id = paymentId,
                Status = "Processing"
            });
        }

        [HttpGet("payments/{id}")]
        [ProducesResponseType(typeof(PaymentDetails), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RetrievePaymentDetails(Guid id)
        {
            var paymentDetails = await _paymentsService.RetrievePaymentDetails(id);

            if (paymentDetails is null)
                return NotFound();

            return Ok(paymentDetails);
        }
    }
}