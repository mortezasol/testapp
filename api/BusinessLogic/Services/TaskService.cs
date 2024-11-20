using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using Core.Entities;
using Core.Interfaces;

namespace BusinessLogic.Services
{
    public class TaskService : ITaskService
    {
        private readonly IRepository<TaskCore> _taskRepository;

        public TaskService(IRepository<TaskCore> taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskDto>> GetTasksAsync()
        {
            // here we get tasks from repository
            var tasks = await _taskRepository.GetAllAsync();                
            // Map to DTOs
            return tasks.Select(task => new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Category = task.Category,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            });
        }

        public async Task AddTaskAsync(CreateTaskDto createTaskDto)
        {
            var task = new TaskCore
            {
                Title = createTaskDto.Title,
                Description = createTaskDto.Description,
                Category = createTaskDto.Category,
                CreatedAt = DateTime.UtcNow
            };

            await _taskRepository.AddAsync(task);
        }

        public async Task UpdateTaskAsync(int id,TaskDto taskDto)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
            {
                throw new KeyNotFoundException($"Task with ID {taskDto.Id} not found.");
            }

            task.Title = taskDto.Title;
            task.Description = taskDto.Description;
            task.Category = taskDto.Category;
            task.IsCompleted = taskDto.IsCompleted;
            task.UpdatedAt = DateTime.UtcNow;

            await _taskRepository.UpdateAsync(task);
        }

        public async Task DeleteTaskAsync(int id)
        {
            await _taskRepository.DeleteAsync(id);
        }



        public async Task<TaskDto> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id); 
            if (task == null)
            {
                throw new KeyNotFoundException($"Task with ID {id} not found.");
            }

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Category = task.Category,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };
        }

        public Task AddTaskAsync(TaskCore task)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTaskAsync(Task task)
        {
            throw new NotImplementedException();
        }


        public Task AddTaskAsync(Task task)
        {
            throw new NotImplementedException();
        }

        async Task<IEnumerable<TaskCore>> ITaskService.GetTasksAsync()
        {
           return await _taskRepository.GetAllAsync();
        }
    }

}

