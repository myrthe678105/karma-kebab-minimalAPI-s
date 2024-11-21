namespace DutyAPI.Models
{
    public class Duty
    {

        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public Guid RoleId { get; set; }

        public string? ImageURL { get; set; }

        public DateTime? DateCompleted { get; set; }

        public string? Status { get; set; }


    }
}