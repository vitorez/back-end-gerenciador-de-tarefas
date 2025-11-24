using MyProject.Api.Features.Tasks;

namespace TaskApi.Features.Tasks;

public static class TasksEndpoints
{
    public static void MapTasksEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tasks").WithTags("Tasks");

        CreateTask.MapEndpoint(app);
        GetAllTasks.MapEndpoint(app);
        UpdateTask.MapEndpoint(app);
        DeleteTask.MapEndpoint(app);
    }
}