using Microsoft.EntityFrameworkCore;
using Tasky.Api.Data;
using Tasky.Api.Model;

var builder = WebApplication.CreateBuilder(args);

// EF Core + SQL Server
builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// CORS for React dev server
const string CorsPolicy = "ReactDev";
builder.Services.AddCors(opts =>
{
    opts.AddPolicy(CorsPolicy, p =>
        p.WithOrigins("http://localhost:5173")
         .AllowAnyHeader()
         .AllowAnyMethod());
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseCors(CorsPolicy);


// Minimal API group
var group = app.MapGroup("/api/tasks");


/// <summary>
/// Gets all tasks.
/// </summary>
/// <returns>List of TaskItem objects.</returns>

group.MapGet("/", async (AppDbContext db) =>
    await db.Tasks.OrderByDescending(t => t.CreatedAt).ToListAsync());

// Get by id
group.MapGet("/{id:int}", async (int id, AppDbContext db) =>
    await db.Tasks.FindAsync(id) is TaskItem t ? Results.Ok(t) : Results.NotFound());

// Create
group.MapPost("/", async (TaskItem input, AppDbContext db) =>
{
    db.Tasks.Add(input);
    await db.SaveChangesAsync();
    return Results.Created($"/api/tasks/{input.Id}", input);
});

// Update
group.MapPut("/{id:int}", async (int id, TaskItem input, AppDbContext db) =>
{
    var existing = await db.Tasks.FindAsync(id);
    if (existing is null) return Results.NotFound();
    existing.Title = input.Title;
    existing.IsCompleted = input.IsCompleted;
    existing.DueDate = input.DueDate;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Delete
group.MapDelete("/{id:int}", async (int id, AppDbContext db) =>
{
    var existing = await db.Tasks.FindAsync(id);
    if (existing is null) return Results.NotFound();
    db.Tasks.Remove(existing);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
