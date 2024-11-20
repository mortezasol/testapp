using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IHubContext<TaskHub> _hubContext;

        public TaskController(ITaskService taskService, IHubContext<TaskHub> hubContext)
        {
            _taskService = taskService;
            _hubContext = hubContext;
        }

        // GET /api/tasks
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _taskService.GetTasksAsync();
            return Ok(tasks);
        }

        // GET /api/tasks/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound($"Task with ID {id} not found.");
            }
            return Ok(task);
        }

        // POST /api/tasks

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto createTaskDto)
        {
            await _taskService.AddTaskAsync(createTaskDto);
            await _hubContext.Clients.All.SendAsync("ReceiveTaskUpdate", $"Task '{createTaskDto.Title}' has been created.");
            return CreatedAtAction(nameof(GetTaskById), new { id = createTaskDto.Id }, createTaskDto);
        }

        // PUT /api/tasks/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskDto taskDto)
        {
            try
            {
                await _taskService.UpdateTaskAsync(id, taskDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE /api/tasks/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                await _taskService.DeleteTaskAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost("send-test-message")]
        public async Task<IActionResult> SendTestMessage(string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveTaskUpdate", message);
            return Ok("Message sent");
        }

    }
}
