namespace TaskTrackerMiniAPI.Contracts;
    public record CreateTaskRequest
    (
        string Title,
        string? Description
    );

