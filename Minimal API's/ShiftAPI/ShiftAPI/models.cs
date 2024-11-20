namespace ShiftAPI
{
public class ShiftDTO
    {
        public Guid ShiftId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid EmployeeId { get; set; }
        public string ShiftType { get; set; } // "standby" or "normal"
        public string Status { get; set; } // "confirmed" or "unconfirmed"
        public DateTime? ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set; }
        public float? ShiftHours { get; set; }
    }

    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
    }


}
