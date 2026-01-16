using FlowEngine.Domain.Entities;

namespace FlowEngine.Application.Interfaces
{
    public interface IWorkflowExecutor
    {
        Task ExecuteAsync(Workflow workflow);
    }
}