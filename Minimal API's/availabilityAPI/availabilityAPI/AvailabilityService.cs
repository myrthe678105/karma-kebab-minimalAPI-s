namespace availabilityAPI
{
    public class AvailabilityService
    {
        private readonly List<Availability> _availabilities = new();

        public IEnumerable<Availability> GetAll() => _availabilities;

        public Availability? GetById(Guid id) => _availabilities.FirstOrDefault(a => a.Id == id);

        public void Create(Availability availability) => _availabilities.Add(availability);

        public bool Update(Guid id, Availability updatedAvailability)
        {
            var existing = GetById(id);
            if (existing == null) return false;

            existing.EmployeeId = updatedAvailability.EmployeeId;
            existing.StartDate = updatedAvailability.StartDate;
            existing.EndDate = updatedAvailability.EndDate;
            return true;
        }

        public bool Delete(Guid id) => _availabilities.RemoveAll(a => a.Id == id) > 0;
    }

}
