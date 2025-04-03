using ANF.Application.Extensions;
using ANF.Application.Middlewares;

// The CreateBuilder method already sets up configuration with environment support, 
// but you can customize it if needed
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Register(builder.Configuration);

builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

builder.Services.AddScoped<UserClaimsMiddleware>();

builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SEP490.AffiliateNetwork V1");
});


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<UserClaimsMiddleware>();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseCors("ANF");

app.MapControllers();

app.Run();
