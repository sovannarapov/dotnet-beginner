using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);

// Register service
builder.Services.AddSingleton<ITaskService>(new InMemoryTaskService());

var app = builder.Build();

// Middleware
// Run in the application pipeline and on all the requests
// Built in middlware
// $1 is used to capture everything behind the tasks/...
// When someone send a request tasks/1 will redirect to todos/1
app.UseRewriter(new RewriteOptions().AddRedirect("tasks/(.*)", "todos/$1"));
// Custom middlware
// The custom middleware will be register and run after the built in middlware
app.Use(async (context, next) =>
{
    Console.WriteLine($"[{context.Request.Method} {context.Request.Path} {DateTime.UtcNow}] Started.");
    await next();
    Console.WriteLine($"[{context.Request.Method} {context.Request.Path} {DateTime.UtcNow}] Finished.");
});

// CRUD API
List<Todo> todos = [];
app.MapGet("/todos", (ITaskService service) => service.GetTodos());
app.MapGet("/todos/{id}", Results<Ok<Todo>, NotFound> (int id, ITaskService service) =>
{
    var targetTodo = service.GetTodoById(id);
    return targetTodo is null ? TypedResults.NotFound() : TypedResults.Ok(targetTodo);
});
app.MapPost("/todos", (Todo todo, ITaskService service) =>
{
    service.AddTodo(todo);
    return TypedResults.Created("/todos/{id}", todo);
})
// Endpoint filters
// Is a validation to validate body of the request
// Run in the context of endpoint or a set of endpoints
.AddEndpointFilter(async (context, next) =>
{
    var taskArgument = context.GetArgument<Todo>(0);
    var errors = new Dictionary<string, string[]>();

    if (taskArgument.DueDate < DateTime.UtcNow)
    {
        errors.Add(nameof(Todo.DueDate), ["Cannot have due date in the past."]);
    }
    if (taskArgument.IsCompleted)
    {
        errors.Add(nameof(Todo.IsCompleted), ["Cannot add completed todo."]);
    }

    if (errors.Count > 0)
    {
        return Results.ValidationProblem(errors);
    }

    return await next(context);
});
app.MapDelete("/todos/{id}", (int id, ITaskService service) =>
{
    service.DeleteTodoById(id);
    return TypedResults.NoContent();
});

app.Run();

public record Todo
(
    int Id,
    string Name,
    DateTime DueDate,
    bool IsCompleted
);

interface ITaskService
{
    Todo? GetTodoById(int id);
    List<Todo> GetTodos();
    Todo AddTodo(Todo task);
    void DeleteTodoById(int id);
}

class InMemoryTaskService : ITaskService
{
    private readonly List<Todo> _todos = [];

    public Todo AddTodo(Todo task)
    {
        _todos.Add(task);
        return task;
    }

    public Todo? GetTodoById(int id)
    {
        return _todos.SingleOrDefault(task => id == task.Id);
    }

    public List<Todo> GetTodos()
    {
        return _todos;
    }

    public void DeleteTodoById(int id)
    {
        _todos.RemoveAll(task => id == task.Id);
    }
}
