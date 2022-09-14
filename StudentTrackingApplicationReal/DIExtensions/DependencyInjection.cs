using StudentTrackingApplicationBackEnd.Infrastructure;
using StudentTrackingApplicationBackEnd.Services;
using StudentTrackingApplicationReal.Shared.Models;
using Microsoft.Extensions.DependencyInjection;

namespace StudentTrackingApplicationBackEnd.DIExtensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddMvc();
            services.AddScoped<Infrastructure.IRepository<Attendance>, Repository<Attendance>>();
            services.AddScoped<Infrastructure.IRepository<Course>, Repository<Course>>();
            services.AddScoped<Infrastructure.IRepository<Student>, Repository<Student>>();
            services.AddScoped<Infrastructure.IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
