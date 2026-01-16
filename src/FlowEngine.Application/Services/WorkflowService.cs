using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlowEngine.Application.DTOs;
using FlowEngine.Application.Interfaces;
using FlowEngine.Domain.Entities;

namespace FlowEngine.Application.Services
{
    public class WorkflowService : IWorkflowService
    {
        // Service, veritabanına doğrudan erişmez. UnitOfWork üzerinden erişir.
        private readonly IUnitOfWork _unitOfWork;

        // Constructor Injection
        public WorkflowService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> CreateWorkflowAsync(CreateWorkflowDto dto)
        {
            // 1. DTO -> Domain Entity Dönüşümü
            // Burada Workflow constructor'ını kullanarak kurallara uygun nesne oluşturuyoruz.
            var workflow = new Workflow(dto.Name, dto.TriggerType ?? "Manual", dto.TriggerSettings ?? "{}");

            // 2. Adımları Ekleme
            foreach (var stepDto in dto.Steps)
            {
                // Workflow içindeki AddStep metodu, "Aynı sırada iki iş olamaz" kuralını kontrol edecek.
                workflow.AddStep(stepDto.ActionType, stepDto.Settings, stepDto.Order);
            }

            // 3. Veritabanına Ekleme (Henüz DB'ye gitmedi, hafızada eklendi)
            await _unitOfWork.Workflows.AddAsync(workflow);

            // 4. İşlemi Onaylama (Commit) - İşte şimdi DB'ye SQL gider.
            await _unitOfWork.CompleteAsync();

            return workflow.Id;
        }

        public async Task<List<Workflow>> GetAllWorkflowsAsync()
        {
            return await _unitOfWork.Workflows.GetAllAsync();
        }

        public async Task<Workflow> GetWorkflowByIdAsync(Guid id)
        {
            return await _unitOfWork.Workflows.GetByIdAsync(id);
        }
    }
}