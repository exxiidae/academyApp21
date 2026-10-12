using Microsoft.AspNetCore.Authentication.Negotiate;
using DBConnect.Data;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddScoped<StudentRepository>();
builder.Services.AddScoped<GroupRepository>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
   .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy.
    options.FallbackPolicy = options.DefaultPolicy;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapGet("/start", () =>
{
    List<String> forecast = new List<String> { "All students" };
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();
static List<String> StartUp()
{
    List<String> list = new List<string>() { "All Student" };
    return list;
}
