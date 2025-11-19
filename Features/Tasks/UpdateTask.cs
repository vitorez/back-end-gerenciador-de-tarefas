using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TaskApi.Infrastructure.Database;

namespace TaskApi.Features.Tasks;

public static class UpdateTask
{
    public class UpdateTaskRequest
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
        app.MapPut("/api/tasks/{id}", HandleAsync)
           .WithTags("Tasks");
    }
    private static async Task<IResult> HandleAsync(int id, UpdateTaskRequest request, AppDbContext db)
    {
        var task = await db.Tasks.FindAsync(id);

        if (task is null)
        {
            return Results.NotFound(new { message = "Tarefa não encontrada" });
        }

        DateTime finalDueDate = task.DueDate;
        if (!string.IsNullOrEmpty(request.Date))
        {
            string timePart = string.IsNullOrEmpty(request.Time) ? "00:00" : request.Time;
            string dateString = $"{request.Date} {timePart}";
            if (DateTime.TryParse(dateString, out var parsedDate))
            {
                finalDueDate = parsedDate;
            }
        }

        task.Title = request.Title;
        task.Description = request.Description;
        task.Category = request.Category;
        task.DueDate = finalDueDate;
        task.Section = request.Section;
        task.Color = request.Color;
        task.Completed = request.Completed;

        await db.SaveChangesAsync();

        return Results.NoContent();
    }
}