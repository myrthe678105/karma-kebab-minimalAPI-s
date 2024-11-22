using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TruckDB>(opt => opt.UseInMemoryDatabase("TruckList"));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "TruckAPI";
    config.Title = "TruckAPI v1";
    config.Version = "v1";
    
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "TodoAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}


app.MapGet("/trucks", async (TruckDB db) =>
    await db.Trucks.ToListAsync());


app.MapGet("/trucks/{id}", async (int id, TruckDB db) =>
    await db.Trucks.FindAsync(id)
        is Truck truck
            ? Results.Ok(truck)
            : Results.NotFound());

app.MapPost("/trucks", async (Truck truck, TruckDB db) =>
{
    db.Trucks.Add(truck);
    await db.SaveChangesAsync();

    return Results.Created($"/trucks/{truck.Id}", truck);
});

app.MapPut("/trucks/{id}", async (int id, Truck inputTruck, TruckDB db) =>
{
    var truck = await db.Trucks.FindAsync(id);

    if (truck is null) return Results.NotFound();

    truck.Plate_number = inputTruck.Plate_number;
    truck.Name = inputTruck.Name;
    truck.Status = inputTruck.Status;
    truck.Description = inputTruck.Description;
    truck.Note = inputTruck.Note;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/trucks/{id}", async (int id, TruckDB db) =>
{
    if (await db.Trucks.FindAsync(id) is Truck truck)
    {
        db.Trucks.Remove(truck);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();
