namespace availabilityAPI
{
    public class Availability
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
