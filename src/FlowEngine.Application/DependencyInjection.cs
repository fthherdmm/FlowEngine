using FlowEngine.Application.Interfaces;
using FlowEngine.Application.Services;
using FlowEngine.Application.WorkflowActions;
using Microsoft.Extensions.DependencyInjection;

namespace FlowEngine.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Servisleri burada bağlıyoruz
            services.AddScoped<IWorkflowService, WorkflowService>();
            
            // Mevcut satırların altına ekle:
            services.AddScoped<IWorkflowExecutor, WorkflowExecutor>();

            services.AddScoped<IWorkflowAction, EmailAction>();
            services.AddScoped<IWorkflowAction, LogAction>();
            // İleride buraya AutoMapper, Validator vs. gelirse onları da buraya ekleyeceğiz.
            
            return services;
        }
    }
}