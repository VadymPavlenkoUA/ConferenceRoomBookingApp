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

        public async Task<Service?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Services.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<List<Service>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
        {
            var serviceIds = ids.Distinct().ToList();

            return await _context.Services.AsNoTracking().Where(s => serviceIds.Contains(s.Id)).ToListAsync(cancellationToken);
        }

        public async Task<List<Service>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Services.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Service service, CancellationToken cancellationToken = default)
        {
            await _context.Services.AddAsync(service, cancellationToken);
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

        // Не дозволяємо видаляти послугу, якщо вона використовується залом або існуючим бронюванням, щоб не порушити цілісність пов'язаних даних
        public async Task<bool> IsUsedAsync(int serviceId, CancellationToken cancellationToken = default)
        {
            return await _context.HallServices.AnyAsync(hs => hs.ServiceId == serviceId, cancellationToken) ||
                await _context.BookingServices.AnyAsync(bs => bs.ServiceId == serviceId, cancellationToken);
        }
    }
}
