using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlowEngine.Application.DTOs;
using FlowEngine.Domain.Entities;

namespace FlowEngine.Application.Interfaces
{
    public interface IWorkflowService
    {
        // Yeni bir akış oluşturur ve oluşturulan ID'yi döner
        Task<Guid> CreateWorkflowAsync(CreateWorkflowDto dto);
        
        // Tüm akışları listeler
        Task<List<Workflow>> GetAllWorkflowsAsync();
        
        // ID'ye göre tek bir akış getirir
        Task<Workflow> GetWorkflowByIdAsync(Guid id);
    }
}