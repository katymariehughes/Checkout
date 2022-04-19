
using Microsoft.AspNetCore.Mvc;
using System.Net;

//[ApiController]
[Route("api/v1")]
public class ProcessController : ControllerBase
{
    [HttpPost("process")]
    //[SwaggerRequestExample(typeof(PaymentRequest), typeof(PaymentRequestExample))]
    [ProducesResponseType(typeof(ProcessPaymentResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    public IActionResult ProcessPayment([FromBody]ProcessPaymentRequest request)
    {
        return Ok(ProcessPaymentResponse.Generate(request));
    }
}