using FluentAssertions;
using Gateway.API.Controllers;
using Gateway.API.Models;
using Gateway.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Gateway.API.Tests
{
    public class PaymentsControllerTests
    {
        private readonly Guid _merchantId;
        private readonly Mock<IPaymentService> _paymentService;
        private readonly PaymentsController _paymentsController;

        public PaymentsControllerTests()
        {
            _merchantId = Guid.NewGuid();
            _paymentService = new Mock<IPaymentService>();
            _paymentsController = new PaymentsController(Mock.Of<ILogger<PaymentsController>>(), _paymentService.Object);
            _paymentsController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
                {
                    Items = new Dictionary<object, object>
                     {
                         { "MerchantId", _merchantId }
                     }
                }
            };
        }

        [Fact]
        public async Task ProcessPassesMerchantIdToServiceAsync()
        {
            await _paymentsController.ProcessPayment(new());

            _paymentService.Verify(x => x.InitiatePaymentFlow(It.IsAny<PaymentRequest>(), It.Is<Guid>(m => m == _merchantId)));
        }

        [Fact]
        public async Task ProcessReturnsCreatedResultAsync()
        {
            var paymentId = Guid.NewGuid();

            _paymentService.Setup(x => x.InitiatePaymentFlow(It.IsAny<PaymentRequest>(), It.IsAny<Guid>()))
                .ReturnsAsync(paymentId);

            var result = await _paymentsController.ProcessPayment(new());

            var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.Value.Should().BeEquivalentTo(new PaymentResponse
            {
                Id = paymentId,
                Status = "Processing"
            });
        }

        [Fact]
        public async Task ProcessReturnsBadRequestWhenNoMerchantIdAsync()
        {
            _paymentsController.ControllerContext.HttpContext.Items.Clear();

            var result = await _paymentsController.ProcessPayment(new());

            var createdResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            createdResult.Value.Should().BeEquivalentTo(new
            {
                Message = "No merchant ID could be found"
            });
        }

        [Fact]
        public async Task RetrieveReturnsOkResultAsync()
        {
            _paymentService.Setup(x => x.RetrievePaymentDetails(It.IsAny<Guid>()))
                .ReturnsAsync(new PaymentDetails());

            var result = await _paymentsController.RetrievePaymentDetails(Guid.NewGuid());

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeAssignableTo<PaymentDetails>();
        }

        [Fact]
        public async Task RetrieveReturnsNotFoundWhenNoPaymentExists()
        {
            _paymentService.Setup(x => x.RetrievePaymentDetails(It.IsAny<Guid>()))
                .ReturnsAsync((PaymentDetails)null);

            var result = await _paymentsController.RetrievePaymentDetails(Guid.NewGuid());

            var okResult = result.Should().BeOfType<NotFoundResult>();
        }
    }
}