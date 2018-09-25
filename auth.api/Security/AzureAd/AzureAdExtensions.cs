using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace auth.api.Security.AzureAd
{
    public static class AzureAdExtensions
    {
        public static IServiceCollection AddAzureAdAuthorization(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection
            .AddAuthorization(options =>
                {
                    options.AddAzureAdPolicy();
                })
            .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = String.Format(configuration["Authentication:Authority"], configuration["Authentication:Tenant"]);
                    options.Audience = configuration["Authentication:ClientId"];
                });

            return serviceCollection;
        }

        public static AuthorizationOptions AddAzureAdPolicy(this AuthorizationOptions options)
        {
            var policy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();

            options.AddPolicy(Constants.AzureAdPolicy, policy);
            return options;
        }
    }
}