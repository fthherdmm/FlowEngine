using FlowEngine.Application.Interfaces;
using FlowEngine.Application.Interfaces.Repositories;
using FlowEngine.Infrastructure.Persistence;
using FlowEngine.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlowEngine.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Veritabanı Bağlantısı
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            // 2. Repository ve UnitOfWork Bağlantıları
            services.AddScoped<IWorkflowRepository, WorkflowRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // İleride RabbitMQ, EmailService vs. buraya eklenecek.

            return services;
        }
    }
}