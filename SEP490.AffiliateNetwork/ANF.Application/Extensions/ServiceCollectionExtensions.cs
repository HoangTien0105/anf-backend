using ANF.Core;
using ANF.Core.Commons;
using ANF.Core.Services;
using ANF.Infrastructure;
using ANF.Service;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ANF.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection Register(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                opt.JsonSerializerOptions.WriteIndented = true;
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            services.AddEndpointsApiExplorer();
            // Options pattern: Must add this line to run properly when injecting the configuration class
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            
            // Override the default configuration of 400 HttpStatusCode for all controllers
            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });

            var connectionString = configuration.GetConnectionString("Default") ?? string.Empty;
            var jwtConfig = configuration.GetRequiredSection("Jwt");
            //var googleConfig = configuration.GetRequiredSection("Google");

            services.ConfigureSwagger();
            services.ConfigureCors();
            services.ConfigureAuthentication(jwtConfig);
            services.ConfigureDatabase(connectionString);

            services.AddAutoMapper(typeof(MappingProfileExtension));
            services.AddApiVersioning();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddApplicationService();

            return services;
        }

        /// <summary>
        /// Configures Cross-Origin Resource Sharing (CORS) for the application.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the CORS policy to.</param>
        /// <returns>The IServiceCollection with the CORS policy added.</returns>
        private static IServiceCollection ConfigureCors(this IServiceCollection services)
        {
            // TODO: Add ports for local and production
            services.AddCors(opt =>
            {
                opt.AddPolicy("ANF", builder =>
                {
                    builder.WithOrigins("http://localhost:3000", "production-port")
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
            return services;
        }
                
        /// <summary>
        /// Configures Swagger for the application.
        /// </summary>
        /// <param name="services">The IServiceCollection to add Swagger to.</param>
        /// <returns>The IServiceCollection with Swagger configured.</returns>
        private static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "SEP490.AffiliateNetwork",
                    Description = "API for Affiliate Network Platform",
                    //TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Support",
                        Email = "support@example.com",
                        Url = new Uri("https://example.com/contact")
                    },
                    //License = new OpenApiLicense
                    //{
                    //    Name = "Use under LICX",
                    //    Url = new Uri("https://example.com/license")
                    //}
                });

                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                // Define JWT Bearer Token support
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token in the format: `Bearer {token}`"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>() // No specific scopes required
                    }
                });
            });

            return services;
        }

        /// <summary>
        /// Configures the database context for the application.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the database context to.</param>
        /// <param name="connection">The connection string for the database.</param>
        /// <returns>The IServiceCollection with the database context configured.</returns>
        private static IServiceCollection ConfigureDatabase(this IServiceCollection services, string connection)
        {
            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseSqlServer(connection, options =>
                {
                    options.CommandTimeout(120);
                });
            });
            return services;
        }

        /// <summary>
        /// Adds API versioning to the application.
        /// </summary>
        /// <param name="services">The IServiceCollection to add API versioning to.</param>
        /// <returns>The IServiceCollection with API versioning added.</returns>
        private static IServiceCollection AddApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new ApiVersion(1);
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-Api-Version"));
            })
            .AddApiExplorer(opt =>
            {
                opt.GroupNameFormat = "'v'V";
                opt.SubstituteApiVersionInUrl = true;
            });

            return services;
        }

        /// <summary>
        /// Adds application-specific services to the IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the services to.</param>
        /// <returns>The IServiceCollection with the application-specific services added.</returns>
        private static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<ICampaignService, CampaignService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPublisherService, PublisherService>();
            services.AddScoped<IAdvertiserService, AdvertiserService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped(typeof(TokenService));

            return services;
        }

        private static IServiceCollection ConfigureAuthentication(this IServiceCollection services, 
            IConfigurationSection jwtSection)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
            {
                var configKey = jwtSection["Key"] ?? string.Empty;
                var key = Encoding.UTF8.GetBytes(configKey);

                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtSection["Issuer"],
                    ValidAudience = jwtSection["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                };
            });  // NOTE: Can add more authentication schema with configurations
            return services;
        }
    }
}
