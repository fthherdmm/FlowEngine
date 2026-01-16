using FlowEngine.Application.DTOs;
using FlowEngine.Application.IntegrationEvents;
using FlowEngine.Application.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace FlowEngine.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkflowsController : ControllerBase
    {
        private readonly IWorkflowService _service;
        private readonly IPublishEndpoint _publishEndpoint;

        public WorkflowsController(IWorkflowService service, IPublishEndpoint publishEndpoint)
        {
            _service = service;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateWorkflowDto dto)
        {
            try 
            {
                var id = await _service.CreateWorkflowAsync(dto);
                
                await _publishEndpoint.Publish(new WorkflowCreatedEvent(id, dto.Name));
                
                return CreatedAtAction(nameof(GetById), new { id = id }, new { id = id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var workflow = await _service.GetWorkflowByIdAsync(id);
            if (workflow == null) return NotFound();
            return Ok(workflow);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllWorkflowsAsync());
        }
    }
}