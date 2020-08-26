namespace ContactViewAPI.App.Helpers.Auth
{
    using ContactViewAPI.Data;
    using ContactViewAPI.Data.Models;
    using ContactViewAPI.Service.Identity;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.Text;

    public static class AuthExtension
    {
        public static IServiceCollection AddAuth(this IServiceCollection services, JwtOptions jwtOptions)
        {
            services.AddIdentity<User, Role>(opt => opt.SignIn.RequireConfirmedEmail = true)
                    .AddEntityFrameworkStores<ContactDbContext>()
                    .AddDefaultTokenProviders();

            services.AddAuthorization()
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            return services;
        }

        public static IApplicationBuilder UseAuth(this IApplicationBuilder builder)
        {
            builder.UseAuthentication();
            builder.UseAuthorization();
            return builder;
        }
    }
}
