var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BankSimulator.API v1"));

app.UseHttpsRedirection();

app.MapPost("/process", (ProcessPaymentRequest request) =>
{
    return ProcessPaymentResponse.Generate(request);
})
.WithName("ProcessPayment");

app.Run();