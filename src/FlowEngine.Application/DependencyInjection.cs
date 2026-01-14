using FlowEngine.Application.Interfaces;
using FlowEngine.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FlowEngine.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Servisleri burada bağlıyoruz
            services.AddScoped<IWorkflowService, WorkflowService>();

            // İleride buraya AutoMapper, Validator vs. gelirse onları da buraya ekleyeceğiz.
            
            return services;
        }
    }
}