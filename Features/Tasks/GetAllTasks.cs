using Microsoft.EntityFrameworkCore;
using taskapi.Domain.Entities;
using TaskApi.Infrastructure.Database;

namespace MyProject.Api.Features.Tasks;

public static class GetAllTasks
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? Date { get; set; }
        public string? Time { get; set; }
        public string? TimeRaw { get; set; }
        public string Section { get; set; } = "today";
        public string Color { get; set; } = string.Empty;
        public bool Completed { get; set; }
    }
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/tasks", HandleAsync);
    }
    private static async Task<IResult> HandleAsync(AppDbContext db)
    {
        var tasks = await db.Set<TaskItem>().ToListAsync();

        var response = tasks.Select(t => new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Category = t.Category,

            Date = t.DueDate.ToString("dd MMM"),
            Time = t.DueDate.ToString("HH:mm"),
            TimeRaw = t.DueDate.ToString("t"),

            Section = t.Section,
            Color = t.Color,
            Completed = t.Completed
        });
        return Results.Ok(response);
    }
}