using System.Threading.Tasks;
using FlowEngine.Application.Interfaces.Repositories;

namespace FlowEngine.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IWorkflowRepository Workflows { get; }
        
        // SaveChangesAsync: Yapılan tüm ekleme/silme işlemlerini tek seferde DB'ye gömer.
        Task<int> CompleteAsync();
    }
}