using FlowEngine.Application.Interfaces;
using FlowEngine.Application.Interfaces.Repositories;
using FlowEngine.Infrastructure.Persistence;
using FlowEngine.Infrastructure.Persistence.Repositories;
using FlowEngine.Infrastructure.Services;
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
                // 1. Worker tarafında eklediğin Consumer'ları buraya dahil ediyoruz
                if (configureMassTransit != null)
                {
                    configureMassTransit(x);
                }

                // 2. RabbitMQ Bağlantı Ayarı (DÜZELTMEN GEREKEN YER BURASI)
                x.UsingRabbitMq((context, cfg) =>
                {
                    // Docker ise "rabbitmq", local ise "localhost" olsun diye ayarlardan okuyoruz
                    var rabbitHost = configuration["RabbitMQ:HostName"] ?? "localhost";

                    cfg.Host(rabbitHost, "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
            
            services.AddTransient<IEmailSender, EmailSender>();

            return services;
        }
    }
}