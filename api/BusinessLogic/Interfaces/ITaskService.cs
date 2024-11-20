using BusinessLogic.DTOs;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskCore>> GetTasksAsync();
        Task<TaskDto> GetTaskByIdAsync(int id);
        Task AddTaskAsync(Task task);
        Task UpdateTaskAsync(int id, TaskDto taskDto);
        Task DeleteTaskAsync(int id);
        Task AddTaskAsync(CreateTaskDto createTaskDto);
    }
}
