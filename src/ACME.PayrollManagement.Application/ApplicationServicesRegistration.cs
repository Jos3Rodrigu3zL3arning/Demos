using ACME.PayrollManagement.Application.Managers;
using ACME.PayrollManagement.Application.Managers.Implementation;
using ACME.PayrollManagement.Domain.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ACME.PayrollManagement.Application
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FileSettingsDTO>(configuration.GetSection("FileSettings"));

            services.AddTransient<IFileManager, FileManager>();
            services.AddTransient<IPaymentManager, PaymentManager>();
            return services;
        }
    }
}