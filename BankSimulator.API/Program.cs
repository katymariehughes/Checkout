using Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.AddLogging("Seq");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BankSimulator.API v1"));

app.UseHttpsRedirection();

//app.MapPost("/process", (ProcessPaymentRequest request) =>
//{
//    return ProcessPaymentResponse.Generate(request);
//})
//.WithName("ProcessPayment");

app.MapControllers();
app.Run();