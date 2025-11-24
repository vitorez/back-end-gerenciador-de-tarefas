using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TaskApi.Infrastructure.Database;

namespace TaskApi.Features.Tasks;

public static class DeleteTask
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/tasks/{id}", HandleAsync)
           .WithTags("Tasks");
    }

    private static async Task<IResult> HandleAsync(int id, AppDbContext db)
    {
        var task = await db.Tasks.FindAsync(id);

        if (task is null)
        {
            return Results.NotFound(new { message = "Tarefa não encontrada" });
        }

        db.Tasks.Remove(task);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }
}