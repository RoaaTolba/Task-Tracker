using Microsoft.EntityFrameworkCore;
using TaskTrackerMiniAPI.Contracts;
using TaskTrackerMiniAPI.Data;
using TaskTrackerMiniAPI.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("TaskTracker")));

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


app.MapGet("/tasks", async (TaskDbContext context) =>
{
    var tasks = await context.Tasks.AsNoTracking().ToListAsync();
    return Results.Ok(tasks);
});

app.MapGet("/tasks/{id:int}", async (int id, TaskDbContext context) =>
{
    var tasks = await context.Tasks.AsNoTracking().FirstOrDefaultAsync(x=>x.Id == id);

    return tasks is null ? Results.NotFound() : Results.Ok(tasks);
});

app.MapPost("/tasks",async(CreateTaskRequest request , TaskDbContext context) =>
{
    if (string.IsNullOrWhiteSpace(request.Title))
        return Results.BadRequest(new { message = "Title is required." });

    var task = new TaskItem
    {
        Title = request.Title.Trim(),
        Description = request.Description,
        IsCompleted = false,
        CreatedAtUtc = DateTimeOffset.UtcNow,
    };
    context.Tasks.Add(task);
    await context.SaveChangesAsync();

    return Results.Created($"/tasks/{task.Id}", task);
});

app.MapPut("/tasks/{id:int}", async(int id, UpdateTaskRequest request, TaskDbContext context) =>
{
    if (string.IsNullOrWhiteSpace(request.Title))
        return Results.BadRequest(new { message = "Title is required." });
    var task = await context.Tasks.FindAsync(id);
    if (task is null)
        return Results.NotFound();

    task.Title = request.Title.Trim();
    task.Description = request.Description;
    task.IsCompleted = request.IsCompleted;

    await context.SaveChangesAsync();

    return Results.Ok(task);
});

app.MapDelete("/tasks/{id:int}", async (int id, TaskDbContext context) =>
{
    var task = await context.Tasks.FindAsync(id);
    if (task is null)
        return Results.NotFound();

    context.Tasks.Remove(task);
    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();
