﻿namespace ContactViewAPI.Service.DependencyInjection
{
    using ContactViewAPI.Service.Email;
    using ContactViewAPI.Service.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyProvider
    {
        public static IServiceCollection AddContactViewDependencies(this IServiceCollection services)
        {
            services.AddScoped<IEmailSender, EmailSender>()
                    .AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
