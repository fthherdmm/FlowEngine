using FlowEngine.Application.Interfaces.Repositories;
using FlowEngine.Domain.Entities;
using FlowEngine.Infrastructure.Persistence; // AppDbContext için
using Microsoft.EntityFrameworkCore; // <-- Include için bu ŞART

namespace FlowEngine.Infrastructure.Persistence.Repositories
{
    // GenericRepository'den miras alıyoruz
    public class WorkflowRepository : GenericRepository<Workflow>, IWorkflowRepository
    {
        private readonly AppDbContext _context;

        public WorkflowRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        // --- BU METODU EKLEMEMİZ LAZIM ---
        public override async Task<Workflow?> GetByIdAsync(Guid id)
        {
            // Include(w => w.Steps) diyerek ilişkili adımları da yüklüyoruz.
            return await _context.Workflows
                .Include(w => w.Steps) 
                .FirstOrDefaultAsync(w => w.Id == id);
        }
    }
}