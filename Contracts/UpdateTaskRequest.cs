namespace TaskTrackerMiniAPI.Contracts;
    public record UpdateTaskRequest
    (
        string Title,
        string? Description,
        bool IsCompleted
        );
