using DutyAPI.Models;
using DutyAPI.Services;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var dutyService = new DutyService();

// GET all duties
app.MapGet("/duty", async () =>
{
    // Get all duties from the service
    var duties = await dutyService.GetAllDutiesAsync();

    // Return the list of duties as a 200 OK response
    return Results.Ok(duties);
});


// POST create duty
app.MapPost("/duty", async (DutyDTO dutyDTO) =>
{
    // Call the service method to create a new Duty
    bool isCreated = await dutyService.CreateDutyAsync(dutyDTO);

    // Return a response based on the result
    if (isCreated)
    {
        return Results.Created($"/duty/{dutyDTO.Name}", dutyDTO);
    }
    else
    {
        return Results.BadRequest("Unable to create duty");
    }
});


// GET duties by role
app.MapGet("/duty/role/{roleId}", async (Guid roleId) =>
{
    var duties = await dutyService.GetDutiesByRoleAsync(roleId);
    return Results.Ok(duties);
});



// GET duty by ID
app.MapGet("/duty/id/{id}", async (Guid id) =>
{
    var duty = await dutyService.GetDutyByIdAsync(id);
    return Results.Ok(new ApiResponse { Success = true, Message = "Duty retrieved", Data = duty });
});


// PUT update duty
app.MapPut("/duty/id/{id}", async (Guid id, DutyDTO updatedDutyDTO) =>
{
    var isUpdated = await dutyService.UpdateDutyAsync(id, updatedDutyDTO);

    if (!isUpdated)
    {
        return Results.Ok($"Mock: Duty with ID {id} would have been updated.");
    }

    return Results.Ok($"Duty with ID {id} has been successfully updated.");
});



// DELETE duty
app.MapDelete("/duty/id/{id}", async (Guid id) =>
{
    var success = await dutyService.DeleteDutyAsync(id);

    if (!success)
    {
        return Results.Ok($"Mock: Duty with ID {id} would have been deleted.");
    }

    return Results.NoContent();
});


app.Run();
