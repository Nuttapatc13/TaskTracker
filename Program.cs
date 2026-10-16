using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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


List<Noted> noteds = new List<Noted>()
{
    new Noted(1, "First Note"),
    new Noted(2, "Second Note")
};


app.MapGet("/notes", () =>
{
    return noteds;
})
.WithName("GetNotes")
.WithOpenApi();

app.MapGet("/notes/{id}", (int id) =>
{
    var found = noteds.FirstOrDefault(x => x.Id == id);
    if (found == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(found);
})
.WithName("GetNotesID")
.WithOpenApi();

/*app.MapPost("/notes", (Noted NewNoteds) =>
{
    noteds.Add(NewNoteds);
    return noteds;
})
.WithOpenApi();*/

app.MapPost("/notes/", (Noted newNoteds) =>
{
    int newId;
    if (noteds.Count != 0)
    {
        var maxId = noteds.Max(n => n.Id);
        newId = maxId + 1;
    }
    else
    {
        newId = 1;
    }

    var created = newNoteds with { Id = newId };
    noteds.Add(created);

    return Results.Created($"/notes/{newId}", created);

}).WithOpenApi();

app.MapDelete("/notes/{id}", (int id) =>
{
    var found = noteds.FirstOrDefault(x => x.Id == id);
    if (found == null) 
    {
        return Results.NotFound();
    }
    noteds.Remove(found);
    return Results.NoContent();
})
.WithName("DeleteNotesByID")
.WithOpenApi();

app.MapPut("/noted/{id}", (int id, Noted updated) =>
{
    var found = noteds.FirstOrDefault(x => x.Id == id);
    if (found == null)
    {
        return Results.NotFound();
    }
    var index = noteds.IndexOf(found);
    noteds[index] = updated with { Id = id };
    return Results.Ok(noteds);

}).WithOpenApi();



app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

record Noted(int Id, string Text)
{
    
}

/*record TaskItem(int ItemId, string ItemTitle, bool IsDone)
{

}*/

/*app.MapGet("/tasks", () =>
{
    return tasks;
})
.WithName("GetTasks")
.WithOpenApi();

app.MapGet("/tasks/{id}", (int id) =>
{
    var found = tasks.FirstOrDefault(t => t.ItemId == id);
    if (found == null) {
        return Results.NotFound();
    }
    return Results.Ok(found);
})
.WithName("GetTasksByID")
.WithOpenApi();

app.MapPost("/tasks", (TaskItem newTask) =>
{
    tasks.Add(newTask);
    return tasks;
})
.WithOpenApi();

app.MapDelete("/tasks/{id}", (int id) =>
{
    var found = tasks.FirstOrDefault(t => t.ItemId == id);
    if (found == null)
    {
        return Results.NotFound();
    }
    else
    {
        tasks.Remove(found);
        return Results.NoContent();
    }
})
.WithName("DeleteTasksByID")
.WithOpenApi();

app.MapPut("/tasks/{id}", (int id, TaskItem updated) =>
{
    var found = tasks.FirstOrDefault(t => t.ItemId == id);
    if (found == null)
    {
        return Results.NotFound();
    }
        var index = tasks.IndexOf(found);
        tasks[index] = updated with {ItemId = id };
    return Results.Ok(tasks);
    
})
.WithName("PutTasksByID")
.WithOpenApi();*/
