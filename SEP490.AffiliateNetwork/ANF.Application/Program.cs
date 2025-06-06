using ANF.Application.Extensions;
using ANF.Service.Hubs;
using ANF.Application.Middlewares;
using Microsoft.AspNetCore.HttpOverrides;

// The CreateBuilder method already sets up configuration with environment support, 
// but you can customize it if needed
var builder = WebApplication.CreateBuilder(args);

// Explicitly configure the configuration
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add services to the container.
builder.Services.Register(builder.Configuration);

builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();
builder.Services.AddScoped<UserClaimsMiddleware>();

var app = builder.Build();

// Add this line to verify the environment
Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SEP490.AffiliateNetwork V1");
});

app.UseForwardedHeaders();

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("ANF");
app.UseAuthentication();

app.UseAuthorization();

app.UseWebSockets();

app.UseMiddleware<UserClaimsMiddleware>();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();


app.MapControllers();

app.MapHub<NotificationHub>("/notiHub");

app.Run();
