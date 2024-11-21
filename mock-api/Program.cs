using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<EventService>(new EventService("db/event.json"));
builder.Services.AddSingleton<AuthService>(new AuthService("db/login.json"));
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
    // Login Endpoints
    endpoints.MapPost("/api/auth/token", (AuthService authService, LoginRequest loginRequest) =>
    {
        var token = authService.Authenticate(loginRequest.Username, loginRequest.Password);
        if (token == null)
        {
            return Results.BadRequest(new { error = "Invalid credentials" });
        }
        return Results.Ok(new { accessToken = token });
    });

    // Event Endpoints
    endpoints.MapGet("/api/events", (EventService service) => service.GetAllEvents());

    endpoints.MapGet("/api/events/{id}", (EventService service, String id) =>
    {
        var ev = service.GetEventById(id);
        return ev != null ? Results.Ok(ev) : Results.NotFound();
    });

    endpoints.MapPost("/api/events", (EventService service, Event newEvent) =>
    {
        service.AddEvent(newEvent);
        return Results.Created($"/api/events/{newEvent.Id}", newEvent);
    });

    endpoints.MapPut("/api/events/{id}", (EventService service, String id, Event updatedEvent) =>
    {
        if (id != updatedEvent.Id)
        {
            return Results.BadRequest();
        }

        service.UpdateEvent(updatedEvent);
        return Results.NoContent();
    });

    endpoints.MapDelete("/api/events/{id}", (EventService service, String id) =>
    {
        service.DeleteEvent(id);
        return Results.NoContent();
    });

    // Shift Endpoints
    // TO DO
});

app.Run();

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}