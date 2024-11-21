using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<EventService>(new EventService("db/event.json"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Event API V1");
        c.RoutePrefix = string.Empty; 
    });
}

app.UseRouting();

// Define the endpoints
app.UseEndpoints(endpoints =>
{
    // Event Endpoints
    endpoints.MapGet("/events", (EventService service) => service.GetAllEvents());

    endpoints.MapGet("/events/{id}", (EventService service, String id) =>
    {
        var ev = service.GetEventById(id);
        return ev != null ? Results.Ok(ev) : Results.NotFound();
    });

    endpoints.MapPost("/events", (EventService service, Event newEvent) =>
    {
        service.AddEvent(newEvent);
        return Results.Created($"/events/{newEvent.Id}", newEvent);
    });

    endpoints.MapPut("/events/{id}", (EventService service, String id, Event updatedEvent) =>
    {
        if (id != updatedEvent.Id)
        {
            return Results.BadRequest();
        }

        service.UpdateEvent(updatedEvent);
        return Results.NoContent();
    });

    endpoints.MapDelete("/events/{id}", (EventService service, String id) =>
    {
        service.DeleteEvent(id);
        return Results.NoContent();
    });

    // Shift Endpoints
    // TO DO
});

app.Run();