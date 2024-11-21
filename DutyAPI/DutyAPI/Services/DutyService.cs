using DutyAPI.Models;

namespace DutyAPI.Services
{
    public class DutyService
    {
        private List<Duty> _duties = new List<Duty>
    {
        new Duty { Id = Guid.NewGuid(), Name = "Clean the counter", RoleId = Guid.NewGuid(), Description = "Clean counter with detergent and water." },
        new Duty { Id = Guid.NewGuid(), Name = "Assemble platters and buffets", RoleId = Guid.NewGuid(), Description = "Arrange and decorate food items on platters and buffet tables to create visually appealing and appetizing displays." }
    };

        // Map DutyDTO to Duty
        private Duty MapToDuty(DutyDTO dutyDTO)
        {
            return new Duty
            {
                Id = Guid.NewGuid(),
                Name = dutyDTO.Name,
                Description = dutyDTO.Description,
                RoleId = dutyDTO.RoleId,
                DateCompleted = null,
                Status = "Pending"
            };
        }

        // Map Duty to DutyDTO
        private DutyDTO MapToDutyDTO(Duty duty)
        {
            return new DutyDTO
            {
                Name = duty.Name,
                Description = duty.Description,
                RoleId = duty.RoleId
            };
        }

        // 1. Get all duties, returning a list of DutyDTO
        public Task<List<DutyDTO>> GetAllDutiesAsync()
        {
            var dutyDTOs = _duties.Select(d => MapToDutyDTO(d)).ToList();
            return Task.FromResult(dutyDTOs);
        }

        // 2. Get duty by Id, returning DutyDTO
        public Task<Duty> GetDutyByIdAsync(Guid id)
        {
            var duty = _duties.Find(d => d.Id == id);

            // Return dummy data if not found
            if (duty == null)
            {
                duty = new Duty
                {
                    Id = id,
                    Name = "Sample Duty",
                    RoleId = Guid.NewGuid(),
                    Description = "This is a sample duty for the specified ID."
                };
            }
            return Task.FromResult(duty);
        }

        // 3. Get duties by RoleId, returning a list of DutyDTO
        public Task<List<Duty>> GetDutiesByRoleAsync(Guid roleId)
        {
            var duties = _duties.FindAll(d => d.RoleId == roleId);

            // Return dummy data if none are found
            if (duties.Count == 0)
            {
                duties.Add(new Duty
                {
                    Id = Guid.NewGuid(),
                    Name = "Sample Duty for Role",
                    RoleId = roleId,
                    Description = "This is a sample duty for the specified role."
                });
            }
            return Task.FromResult(duties);
        }

        // 4. Create Duty from DutyDTO
        public Task<bool> CreateDutyAsync(DutyDTO dutyDTO)
        {
            Duty duty = MapToDuty(dutyDTO);
            _duties.Add(duty);
            return Task.FromResult(true);
        }

        // 5. Update Duty, using DutyDTO
        public Task<bool> UpdateDutyAsync(Guid id, DutyDTO updatedDutyDTO)
        {
            var duty = _duties.Find(d => d.Id == id);

            // If not found, return false
            if (duty == null)
            {
                // Optionally, you could also add the updated data as dummy
                return Task.FromResult(false);
            }

            // Update the duty in the list
            duty.Name = updatedDutyDTO.Name;
            duty.Description = updatedDutyDTO.Description;
            duty.RoleId = updatedDutyDTO.RoleId;

            return Task.FromResult(true);
        }

        // 6. Delete Duty by Id
        public Task<bool> DeleteDutyAsync(Guid id)
        {
            var duty = _duties.Find(d => d.Id == id);

            // If not found, return false
            if (duty == null)
            {
                return Task.FromResult(false);
            }

            _duties.Remove(duty);
            return Task.FromResult(true);
        }
    }
}
