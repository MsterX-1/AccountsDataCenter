using Application.Interfaces;
using Application.Services;
using Application.Services.AuthService;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<AccountService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();


            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            #region JWT Authentication configuration
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    // ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"])),

                    ClockSkew = TimeSpan.Zero // no extra time beyond token expiration
                };
            });
            #endregion

            builder.Services.AddControllers();

            // Swagger/OpenAPI setup
            builder.Services.AddEndpointsApiExplorer();

            #region swagger setting
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "API Account Data Center",
                    Version = "v1",
                    Description = "CRUD endpoint for user and account with authentication",
                    Contact = new OpenApiContact()
                    {
                        Name = "MsterX",
                        Email = "MsterXtheking@gmail.com",
                        Url = new Uri("https://www.facebook.com/yousef.ibrahim.94695/")
                    }
                });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token like: Bearer {your token}"

                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                     }
                });
            });
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }
    }
}
