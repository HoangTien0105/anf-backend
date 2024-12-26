using Anf.Service;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Anf.Platform.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection Register(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.WriteIndented = true;
                opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

            // API documentation: NSwag
            services.AddOpenApiDocument(opt =>
            {
                opt.PostProcess = doc =>
                {
                    doc.Info = new NSwag.OpenApiInfo
                    {
                        Version = "v1",
                        Title = "ANF-Platform",
                        Description = "An ASP.NET Core Web API for operations in ANF-Platform",
                        //Contact = new OpenApiContact
                        //{
                        //    Name = "Example Contact",
                        //    Url = "https://example.com/contact"
                        //},
                        //License = new OpenApiLicense
                        //{
                        //    Name = "Example License",
                        //    Url = "https://example.com/license"
                        //}
                    };
                };
                opt.AddSecurity("JWT", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme
                {
                    Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = NSwag.OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}."
                });

                opt.OperationProcessors.Add(new NSwag.Generation.Processors.Security.AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                // TODO: Add value for issuer, audience and key
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    //ValidIssuer = configuration["JwtSettings:Issuer"],
                    //ValidAudience = configuration["JwtSettings:Audience"],
                    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]))
                };
            });

            // Database configuration
            services.RegisterDatabase(configuration);

            // AutoMapper configuration
            services.AddAutoMapper(typeof(MappingProfileExtension));

            services.RegisterCors();

            services.RegisterApiVersion();

            return services;
        }

        private static IServiceCollection RegisterDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }

        private static IServiceCollection RegisterCors(this IServiceCollection services)
        {
            // TODO: Add ports for local and production
            services.AddCors(opt =>
            {
                opt.AddPolicy("ANF", builder =>
                {
                    builder.WithOrigins("local-port", "production-port")
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
            return services;
        }

        private static IServiceCollection RegisterApiVersion(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-Api-Version"));
            })
            .AddMvc() // This is needed for controllers
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
            return services;
        }

    }
}
