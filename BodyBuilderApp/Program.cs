
using BodyBuilder.Application.Extensions;
using BodyBuilder.Application.Utilities.JWT;
using BodyBuilder.Application.ValidationRules.User;
using BodyBuilder.Infrastructure.Persistence.Extensions;
using BodyBuilderApp.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace BodyBuilderApp {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region Cors Policy
            builder.Services.AddCors(options => {
                options.AddPolicy("ReactAppPolicy",
                    builder => {
                        builder.WithOrigins("http://localhost:5173") // React UI'nin adresi (örnek olarak)
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });
            #endregion

            #region Jwt
            var token = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt => {
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    

                    ValidAudience = token.Audience,
                    ValidIssuer = token.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token.SecurityKey))
                };
            });

            #endregion

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo {
                    Version = "v1",
                    Title = "BodyBuilder App Token Based Auth Project",
                    Description = "BodyBuilder App Token Based Auth Project",
                    TermsOfService = new Uri("https://www.linkedin.com/in/mreyuce/"),
                    Contact = new OpenApiContact {
                        Email = "emreyuce9039@gmail.com",
                        Name = "Emre Yüce",
                        Url = new Uri("https://www.linkedin.com/in/mreyuce/")
                    },
                    License = new OpenApiLicense { Name = "Emre Yüce Licence", Url = new Uri("https://www.linkedin.com/in/mreyuce/") }

                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement{    {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    new string[] {}
                        }
                    });
            });
            builder.Services.AddInfraDependencies();
            builder.Services.AddApplicationDependencies();
            var app = builder.Build();
            app.ConfigureExceptionHandler();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("ReactAppPolicy");
            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}