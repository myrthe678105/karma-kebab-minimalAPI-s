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
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Karma Kebab API V1");
        c.RoutePrefix = string.Empty; 
    });
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    // Login and authentication endpoints
    endpoints.MapPost("/api/auth/token", (AuthService authService, LoginRequest loginRequest) =>
    {
        var response = authService.Authenticate(loginRequest.Username, loginRequest.Password);
        if (response == null)
        {
            return Results.Unauthorized();
        }
        return Results.Ok(response);
    });

    endpoints.MapPost("/api/auth/refresh", (AuthService authService, RefreshRequest refreshRequest) =>
    {
        if (string.IsNullOrEmpty(refreshRequest.RefreshToken))
        {
            return Results.BadRequest();
        }

        var response = authService.RefreshToken(refreshRequest.RefreshToken);
        if (response == null)
        {
            return Results.BadRequest(new { error = "invalid_grant", error_description = "Invalid refresh token" });
        }
        return Results.Ok(response);
    });

    endpoints.MapPost("/api/auth/logout", (AuthService authService, LogoutRequest logoutRequest) =>
    {
        if (string.IsNullOrEmpty(logoutRequest.RefreshToken))
        {
            return Results.BadRequest(new { error = "invalid_request", error_description = "Refresh token is missing" });
        }

        authService.Logout(logoutRequest.RefreshToken);
        return Results.NoContent();
    });

    endpoints.MapGet("/api/auth/userinfo", (AuthService authService, string accessToken) =>
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            return Results.Unauthorized();
        }

        var response = authService.GetUserInfo(accessToken);
        if (response == null)
        {
            return Results.Unauthorized();
        }
        return Results.Ok(response);
    });

    // Event endpoints
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

    //TODO: Add more endpoints here
});

app.Run();

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class RefreshRequest
{
    public string RefreshToken { get; set; }
}

public class LogoutRequest
{
    public string RefreshToken { get; set; }
}