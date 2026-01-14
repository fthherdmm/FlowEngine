using FlowEngine.Application.Interfaces.Repositories;
using FlowEngine.Domain.Entities;

namespace FlowEngine.Infrastructure.Persistence.Repositories
{
    public class WorkflowRepository : GenericRepository<Workflow>, IWorkflowRepository
    {
        public WorkflowRepository(AppDbContext context) : base(context)
        {
        }
    }
}