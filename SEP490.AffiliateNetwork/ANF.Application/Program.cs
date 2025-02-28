using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using ANF.Application.Extensions;
using ANF.Application.Middlewares;
using ANF.Core.Commons;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Register(builder.Configuration);
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

// Configure Cloudflare R2 (S3-compatible)
var credentials = new BasicAWSCredentials(CloudflareR2Constants.AccessKey, CloudflareR2Constants.SecretKey);
var s3Config = new AmazonS3Config
{
    ServiceURL = CloudflareR2Constants.BaseUrl,
    //SignatureVersion = "4",
    ForcePathStyle = true,
};

builder.Services.AddSingleton<IAmazonS3>(new AmazonS3Client(credentials, s3Config));

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
