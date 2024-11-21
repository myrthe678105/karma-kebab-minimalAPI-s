using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<EventService>(new EventService("db/event.json"));
builder.Services.AddSingleton<AuthService>(new AuthService("db/login.json"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Karma Kebab API",
        Version = "v1"
    });

    // Add Bearer token definition
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by a space and your token. Example: Bearer {token}"
    });

    // Require Bearer token for all endpoints
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

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

    endpoints.MapGet("/api/auth/userinfo", (AuthService authService, HttpRequest request) =>
    {
        // Extract the Authorization header
        var authorizationHeader = request.Headers["Authorization"].FirstOrDefault();

        // Check if the Authorization header is missing or invalid
        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            return Results.Unauthorized();
        }

        // Extract the access token from the Authorization header
        var accessToken = authorizationHeader.Substring("Bearer ".Length).Trim();

        // Validate the access token using AuthService
        var response = authService.GetUserInfo(accessToken);
        if (response == null)
        {
            return Results.Unauthorized();
        }

        // Return user information
        return Results.Ok(response);
    });

    // Event endpoints
    endpoints.MapGet("/api/events", (EventService service, HttpRequest request, AuthService authService) =>
    {
        // Extract the Authorization header
        var authorizationHeader = request.Headers["Authorization"].FirstOrDefault();

        // Check if the Authorization header is missing or invalid
        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            return Results.Unauthorized();
        }

        // Extract the access token from the Authorization header
        var accessToken = authorizationHeader.Substring("Bearer ".Length).Trim();

        // Validate the access token using AuthService
        var userInfo = authService.GetUserInfo(accessToken);
        if (userInfo == null)
        {
            return Results.Unauthorized();
        }

        // Return all events
        return Results.Ok(service.GetAllEvents());
    });

    endpoints.MapGet("/api/events/{id}", (EventService service, HttpRequest request, AuthService authService, string id) =>
    {
        // Extract the Authorization header
        var authorizationHeader = request.Headers["Authorization"].FirstOrDefault();

        // Check if the Authorization header is missing or invalid
        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            return Results.Unauthorized();
        }

        // Extract the access token from the Authorization header
        var accessToken = authorizationHeader.Substring("Bearer ".Length).Trim();

        // Validate the access token using AuthService
        var userInfo = authService.GetUserInfo(accessToken);
        if (userInfo == null)
        {
            return Results.Unauthorized();
        }

        // Find the event by ID
        var ev = service.GetEventById(id);
        return ev != null ? Results.Ok(ev) : Results.NotFound();
    });

    endpoints.MapPost("/api/events", (EventService service, HttpRequest request, AuthService authService, Event newEvent) =>
    {
        // Extract the Authorization header
        var authorizationHeader = request.Headers["Authorization"].FirstOrDefault();

        // Check if the Authorization header is missing or invalid
        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            return Results.Unauthorized();
        }

        // Extract the access token from the Authorization header
        var accessToken = authorizationHeader.Substring("Bearer ".Length).Trim();

        // Validate the access token using AuthService
        var userInfo = authService.GetUserInfo(accessToken);
        if (userInfo == null)
        {
            return Results.Unauthorized();
        }

        // Add the new event
        service.AddEvent(newEvent);
        return Results.Created($"/api/events/{newEvent.Id}", newEvent);
    });

    endpoints.MapPut("/api/events/{id}", (EventService service, HttpRequest request, AuthService authService, string id, Event updatedEvent) =>
    {
        // Extract the Authorization header
        var authorizationHeader = request.Headers["Authorization"].FirstOrDefault();

        // Check if the Authorization header is missing or invalid
        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            return Results.Unauthorized();
        }

        // Extract the access token from the Authorization header
        var accessToken = authorizationHeader.Substring("Bearer ".Length).Trim();

        // Validate the access token using AuthService
        var userInfo = authService.GetUserInfo(accessToken);
        if (userInfo == null)
        {
            return Results.Unauthorized();
        }

        // Validate the ID match
        if (id != updatedEvent.Id)
        {
            return Results.BadRequest();
        }

        // Update the event
        service.UpdateEvent(updatedEvent);
        return Results.NoContent();
    });

    endpoints.MapDelete("/api/events/{id}", (EventService service, HttpRequest request, AuthService authService, string id) =>
    {
        // Extract the Authorization header
        var authorizationHeader = request.Headers["Authorization"].FirstOrDefault();

        // Check if the Authorization header is missing or invalid
        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            return Results.Unauthorized();
        }

        // Extract the access token from the Authorization header
        var accessToken = authorizationHeader.Substring("Bearer ".Length).Trim();

        // Validate the access token using AuthService
        var userInfo = authService.GetUserInfo(accessToken);
        if (userInfo == null)
        {
            return Results.Unauthorized();
        }

        // Delete the event
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