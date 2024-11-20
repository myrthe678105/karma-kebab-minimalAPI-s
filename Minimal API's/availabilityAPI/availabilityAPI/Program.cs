using availabilityAPI;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<AvailabilityService>();

var app = builder.Build();

// Map Routes
app.MapGet("/availabilities", (AvailabilityService service) =>
{
    return Results.Ok(service.GetAll());
}).WithName("GetAllAvailabilities");

app.MapGet("/availabilities/{id:guid}", (AvailabilityService service, Guid id) =>
{
    var availability = service.GetById(id);
    return availability is not null ? Results.Ok(availability) : Results.NotFound();
}).WithName("GetAvailabilityById");

app.MapPost("/availabilities", (AvailabilityService service, Availability availability) =>
{
    service.Create(availability);
    return Results.Created($"/availabilities/{availability.Id}", availability);
}).WithName("CreateAvailability");

app.MapPut("/availabilities/{id:guid}", (AvailabilityService service, Guid id, Availability updatedAvailability) =>
{
    var success = service.Update(id, updatedAvailability);
    return success ? Results.Ok(updatedAvailability) : Results.NotFound();
}).WithName("UpdateAvailability");

app.MapDelete("/availabilities/{id:guid}", (AvailabilityService service, Guid id) =>
{
    var success = service.Delete(id);
    return success ? Results.NoContent() : Results.NotFound();
}).WithName("DeleteAvailability");

app.Run();
