using TaskTrackerMiniAPI.Contracts;
using TaskTrackerMiniAPI.Models;

namespace TaskTrackerMiniAPI.Services
{
    public class TaskService
    {
        public TaskItem CreateTask(CreateTaskRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                throw new ArgumentException(
                    "Title is required.",
                    nameof(request.Title));
            }

            return new TaskItem
            {
                Title = request.Title.Trim(),
                Description = request.Description,
                IsCompleted = false,
                CreatedAtUtc = DateTimeOffset.UtcNow
            };
        }
    }
}
