using ShiftAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ShiftService
{
    private readonly List<ShiftDTO> _shifts = new List<ShiftDTO>();

    public Task<List<ShiftDTO>> GetShiftsAsync(DateTime? date = null, Guid? employeeId = null, string? shiftType = null)
    {
        var query = _shifts.AsQueryable();

        if (date.HasValue)
            query = query.Where(s => s.StartTime.Date == date.Value.Date);

        if (employeeId.HasValue)
            query = query.Where(s => s.EmployeeId == employeeId);

        if (!string.IsNullOrEmpty(shiftType))
            query = query.Where(s => s.ShiftType == shiftType);

        return Task.FromResult(query.ToList());
    }

    public Task<ShiftDTO?> GetShiftByIdAsync(Guid shiftId) =>
        Task.FromResult(_shifts.FirstOrDefault(s => s.ShiftId == shiftId));

    public Task<bool> CreateShiftAsync(ShiftDTO shift)
    {
        _shifts.Add(shift);
        return Task.FromResult(true);
    }

    public Task<bool> UpdateShiftAsync(Guid shiftId, ShiftDTO updatedShift)
    {
        var shift = _shifts.FirstOrDefault(s => s.ShiftId == shiftId);
        if (shift == null) return Task.FromResult(false);

        shift.StartTime = updatedShift.StartTime;
        shift.EndTime = updatedShift.EndTime;
        shift.ClockInTime = updatedShift.ClockInTime;
        shift.ClockOutTime = updatedShift.ClockOutTime;
        shift.Status = updatedShift.Status;
        shift.ShiftHours = updatedShift.ShiftHours;

        return Task.FromResult(true);
    }

    public Task<bool> DeleteShiftAsync(Guid shiftId)
    {
        var shift = _shifts.FirstOrDefault(s => s.ShiftId == shiftId);
        if (shift == null) return Task.FromResult(false);

        _shifts.Remove(shift);
        return Task.FromResult(true);
    }
}
