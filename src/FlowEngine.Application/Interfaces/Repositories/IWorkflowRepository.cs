using FlowEngine.Domain.Entities;

namespace FlowEngine.Application.Interfaces.Repositories
{
    // IGenericRepository'den miras alıyor, yani Add, Delete vs. buna da geldi.
    public interface IWorkflowRepository : IGenericRepository<Workflow>
    {
        // İleride Workflow'a özel sorgular gerekirse buraya ekleyeceğiz.
        // Örn: Task<List<Workflow>> GetActiveWorkflowsWithStepsAsync();
    }
}