using ConferenceRoomBooking.Application.Interfaces.Repositories;
using ConferenceRoomBooking.Domain.Entities;
using ConferenceRoomBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ConferenceRoomBooking.Infrastructure.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly AppDbContext _context;

        public ServiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Service?> GetByIdAsync(int id)
        {
            return await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Service>> GetByIdsAsync(IEnumerable<int> ids)
        {
            var serviceIds = ids.Distinct().ToList();

            return await _context.Services.Where(s => serviceIds.Contains(s.Id)).ToListAsync();
        }

        public async Task AddAsync(Service service)
        {
            await _context.Services.AddAsync(service);
        }

        public Task UpdateAsync(Service service)
        {
            _context.Services.Update(service);

            return Task.CompletedTask;
        }

        public Task DeleteAsync(Service service)
        {
            _context.Services.Remove(service);

            return Task.CompletedTask;
        }
    }
}
