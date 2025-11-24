using taskapi.Domain.Entities;
using TaskApi.Infrastructure.Database;

namespace TaskApi.Features.Tasks;

public static class CreateTask
{
    public class CreateTaskRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? Date { get; set; }
        public string? Time { get; set; }
        public string Section { get; set; } = "today";
        public string Color { get; set; } = "#ffffff";
        public bool Completed { get; set; }
    }
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/tasks", HandleAsync)
           .WithTags("Tasks");
    }
    private static async Task<IResult> HandleAsync(CreateTaskRequest request, AppDbContext db)
    {
        DateTime finalDueDate = DateTime.Now;

        if (!string.IsNullOrEmpty(request.Date))
        {
            string timePart = string.IsNullOrEmpty(request.Time) ? "00:00" : request.Time;
            string dateString = $"{request.Date} {timePart}";

            if (!DateTime.TryParse(dateString, out finalDueDate))
            {
                finalDueDate = DateTime.Now;
            }
        }
        var newTask = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            Category = request.Category,
            DueDate = finalDueDate,
            Section = request.Section,
            Color = request.Color,
            Completed = request.Completed
        };
        db.Tasks.Add(newTask);
        await db.SaveChangesAsync();

        return Results.Ok(new { id = newTask.Id });
    }
}