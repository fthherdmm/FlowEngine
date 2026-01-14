using System.Threading.Tasks;
using FlowEngine.Application.Interfaces;
using FlowEngine.Application.Interfaces.Repositories;
using FlowEngine.Infrastructure.Persistence.Repositories;

namespace FlowEngine.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            // WorkflowRepository'yi burada new'liyoruz.
            Workflows = new WorkflowRepository(_context);
        }

        public IWorkflowRepository Workflows { get; private set; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}