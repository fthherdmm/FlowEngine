using FlowEngine.Application.Interfaces;
using FlowEngine.Application.Interfaces.Repositories;
using FlowEngine.Infrastructure.Persistence;
using FlowEngine.Infrastructure.Persistence.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlowEngine.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, Action<IBusRegistrationConfigurator>? configureMassTransit = null)
        {
            // 1. Veritabanı Bağlantısı
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            // 2. Repository ve UnitOfWork Bağlantıları
            services.AddScoped<IWorkflowRepository, WorkflowRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            // 3. MassTransit (RabbitMQ) Ayarları (Ortak + Özelleştirilebilir)
            services.AddMassTransit(x =>
            {
                // Eğer çağıran yer (Worker) extra ayar yapmak isterse (Consumer eklemek gibi)
                // buradaki action'ı çalıştır.
                if (configureMassTransit != null)
                {
                    configureMassTransit(x);
                }

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    // MassTransit'in otomatik endpoint yapılandırması
                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}