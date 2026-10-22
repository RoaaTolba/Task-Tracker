using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskTrackerMiniAPI.Contracts;
using TaskTrackerMiniAPI.Data;
using TaskTrackerMiniAPI.Models;
using TaskTrackerMiniAPI.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("TaskTracker")));
builder.Services.AddScoped<TaskService>();
// Register services needed for Swagger/OpenAPI.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/tasks", async (TaskDbContext context, ILogger<Program> logger) =>
{
    logger.LogInformation("Getting all tasks.");
    var tasks = await context.Tasks.AsNoTracking().ToListAsync();

    logger.LogInformation("Retrieved {TaskCount} tasks.", tasks.Count);
    return Results.Ok(tasks);
});

app.MapGet("/tasks/{id:int}", async (int id, TaskDbContext context, ILogger<Program> logger) =>
{
    logger.LogInformation("Getting task {TaskId}.", id);
    var tasks = await context.Tasks.AsNoTracking().FirstOrDefaultAsync(x=>x.Id == id);
    
    if (tasks is null)
    {
        logger.LogWarning("Task {TaskId} was not found.", id);
        return Results.NotFound();
    }
    logger.LogInformation( "Task {TaskId} retrieved successfully.", id);

    return tasks is null ? Results.NotFound() : Results.Ok(tasks);
});

app.MapPost("/tasks", async (
    CreateTaskRequest request,
    TaskDbContext context,
    TaskService taskService,
    ILogger<Program> logger) =>
{
    TaskItem task;

    try
    {
        task = taskService.CreateTask(request);
    }
    catch (ArgumentException)
    {
        logger.LogWarning(
            "Task creation rejected because the title was empty.");

        return Results.BadRequest(
            new { message = "Title is required." });
    }

    context.Tasks.Add(task);
    await context.SaveChangesAsync();

    logger.LogInformation("Created task {TaskId}.", task.Id);

    return Results.Created($"/tasks/{task.Id}", task);
});

app.MapPut("/tasks/{id:int}", async(int id, UpdateTaskRequest request, TaskDbContext context, ILogger<Program> logger) =>
{
    logger.LogInformation("Updating task {TaskId}.", id);

    if (string.IsNullOrWhiteSpace(request.Title))
    {
        logger.LogWarning( "Task update rejected because the title was empty. TaskId: {TaskId}", id);
        return Results.BadRequest(new { message = "Title is required." });
        }
    var task = await context.Tasks.FindAsync(id);
    if (task is null)
    {
        logger.LogWarning("Cannot update task {TaskId} because it was not found.", id);
        return Results.NotFound();
    }
    task.Title = request.Title.Trim();
    task.Description = request.Description;
    task.IsCompleted = request.IsCompleted;

    await context.SaveChangesAsync();

    logger.LogInformation( "Task {TaskId} updated successfully.", id);

    return Results.Ok(task);
});

app.MapDelete("/tasks/{id:int}", async (int id, TaskDbContext context, ILogger<Program> logger) =>
{
    logger.LogInformation("Deleting task {TaskId}.", id);
    var task = await context.Tasks.FindAsync(id);
    if (task is null)
    {
        logger.LogWarning( "Cannot delete task {TaskId} because it was not found.", id);
        return Results.NotFound();
    }
    context.Tasks.Remove(task);
    await context.SaveChangesAsync();

    logger.LogInformation("Task {TaskId} deleted successfully.", id);
    return Results.NoContent();
});

app.Run();

public partial class Program { }