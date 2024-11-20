using ShiftAPI;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var shiftService = new ShiftService();

app.MapGet("/shifts", async (DateTime? date, Guid? employeeId, string? shiftType) =>
{
    var shifts = await shiftService.GetShiftsAsync(date, employeeId, shiftType);
    return Results.Ok(new ApiResponse { Success = true, Message = "Shifts retrieved", Data = shifts });
});

app.MapGet("/shifts/{shiftId}", async (Guid shiftId) =>
{
    var shift = await shiftService.GetShiftByIdAsync(shiftId);
    return shift != null ?
        Results.Ok(new ApiResponse { Success = true, Message = "Shift retrieved", Data = shift }) :
        Results.NotFound(new ApiResponse { Success = false, Message = "Shift not found" });
});

app.MapPost("/shifts", async (ShiftDTO shift) =>
{
    await shiftService.CreateShiftAsync(shift);
    return Results.Created($"/shifts/{shift.ShiftId}", new ApiResponse { Success = true, Message = "Shift created", Data = shift });
});

app.MapPut("/shifts/{shiftId}", async (Guid shiftId, ShiftDTO shift) =>
{
    var success = await shiftService.UpdateShiftAsync(shiftId, shift);
    return success ?
        Results.Ok(new ApiResponse { Success = true, Message = "Shift updated", Data = shift }) :
        Results.NotFound(new ApiResponse { Success = false, Message = "Shift not found" });
});

app.MapDelete("/shifts/{shiftId}", async (Guid shiftId) =>
{
    var success = await shiftService.DeleteShiftAsync(shiftId);
    return success ?
        Results.NoContent() :
        Results.NotFound(new ApiResponse { Success = false, Message = "Shift not found" });
});

app.Run();
