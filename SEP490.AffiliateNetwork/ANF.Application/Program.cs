using ANF.Application.Extensions;
using ANF.Application.Middlewares;
using ANF.Core.Commons;
using Microsoft.Extensions.Options;
using R2.NET;
using R2.NET.Configuration;
using R2.NET.Factories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Register(builder.Configuration);
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

// Configure Cloudflare R2
//builder.Services.Configure<CloudflareR2Constants>(builder.Configuration.GetSection(CloudflareR2Options.SettingsName));
//builder.Services.AddSingleton<ICloudflareR2ClientFactory, CloudflareR2ClientFactory>();
//builder.Services.AddSingleton(provider =>
//{
//    var options = provider.GetRequiredService<IOptions<CloudflareR2Options>>();
//    var logger = provider.GetRequiredService<ILogger<CloudflareR2Client>>();

//    return new CloudflareR2Client("r2-client", options, logger);
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SEP490.AffiliateNetwork V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
