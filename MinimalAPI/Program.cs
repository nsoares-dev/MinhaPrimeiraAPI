using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using MinimalAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoDB>(
    opt => opt.UseInMemoryDatabase("TodoList"));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


var app = builder.Build();

app.MapGet("/todoitems", async (TodoDB db) =>
    await db.todos.ToListAsync());

app.MapGet("/todoitems/complete", async (TodoDB db) =>
    await db.todos.Where(t => t.IsComplete).ToListAsync());

app.MapGet("/todoitems/{id}", async (int id, TodoDB db) =>
    await db.todos.FindAsync(id)
        is Todo todo ? Results.Ok(todo) : Results.NotFound());

app.MapPost("/todoitems", async (Todo todo, TodoDB db) =>
{
    db.todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todo.Id}", todo);
});

app.MapPut("/todoitems/{id}", async (int id, Todo inputTodo, TodoDB db) =>
{
    var todo = await db.todos.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Nome = inputTodo.Nome;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, TodoDB db) =>
{
    if (await db.todos.FindAsync(id) is Todo todo)
    {
        db.todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.MapGet("/alunos", async (TodoDB db) =>
    await db.alunos.ToListAsync());

app.MapPost("/alunos", async (Aluno alunos, TodoDB db) =>
{
    db.alunos.Add(alunos);
    await db.SaveChangesAsync(); 

    return Results.Created($"/alunos/{alunos.Id}", alunos);
});

app.Run();
