using EmployeeManagement.Web.Repositories.Abstractions;
using EmployeeManagement.Web.Repositories.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement.Web.Repositories
{
    public static class Extensions
    {
        public static IServiceCollection AddMockRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IEmployeeRepository, MockEmployeeRepository>();
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            return services;
        }
    }
}
