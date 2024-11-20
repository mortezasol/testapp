using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TaskRepository : IRepository<TaskCore>
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "Database context cannot be null.");
        }

        public async Task<TaskCore> GetByIdAsync(int id)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(id);
                if (task == null)
                {
                    throw new KeyNotFoundException($"Task with ID {id} was not found.");
                }

                return task;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching Task by ID {id}: {ex.Message}");
                throw new Exception($"Unable to retrieve Task with ID {id}. inner exception for details.", ex);
            }
        }

        public async Task<IEnumerable<TaskCore>> GetAllAsync()
        {
            try
            {
                return await _context.Tasks.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching all Tasks: {ex.Message}");
                throw new Exception("Unable to retrieve all Tasks. look at the inner exception for details.", ex);
            }
        }

        public async Task AddAsync(TaskCore task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task), "Task cannot be null.");
            }

            try
            {
                await _context.Tasks.AddAsync(task);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Database error adding Task: {dbEx.Message}");
                throw new Exception("Error adding Task to the database. See inner exception for details.", dbEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding Task: {ex.Message}");
                throw new Exception("An error occurred while adding the Task. See inner exception for details.", ex);
            }
        }

        public async Task UpdateAsync(TaskCore task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task), "Task cannot be null.");
            }

            try
            {
                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is TaskCore)
                    {
                        var proposedValues = entry.CurrentValues;
                        var databaseValues = await entry.GetDatabaseValuesAsync();

                        if (databaseValues == null)
                        {
                            throw new Exception("The task was deleted by another user.");
                        }

                        // Resolve the conflict (you can log the conflict or prompt the user to resolve it)
                        foreach (var property in proposedValues.Properties)
                        {
                            var proposedValue = proposedValues[property];
                            var databaseValue = databaseValues[property];
                            // Example: Keep database value or apply custom conflict resolution logic
                            proposedValues[property] = databaseValue;
                        }

                        entry.OriginalValues.SetValues(databaseValues);
                    }
                }

                throw new Exception("A concurrency conflict occurred. Please reload the task and try again.");
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var task = await GetByIdAsync(id);
                if (task == null)
                {
                    throw new KeyNotFoundException($"Task with ID {id} was not found and cannot be deleted.");
                }

                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Database error deleting Task: {dbEx.Message}");
                throw new Exception($"Error deleting Task with ID {id}. See inner exception for details.", dbEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting Task with ID {id}: {ex.Message}");
                throw new Exception($"An error occurred while deleting the Task with ID {id}. See inner exception for details.", ex);
            }
        }
    }
}
