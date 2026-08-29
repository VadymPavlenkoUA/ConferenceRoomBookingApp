using ConferenceRoomBooking.Application.Interfaces.Repositories;
using ConferenceRoomBooking.Domain.Entities;
using ConferenceRoomBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ConferenceRoomBooking.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Bookings
                .Include(b => b.Hall)
                .Include(b => b.BookingServices).ThenInclude(bs => bs.Service)
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task<List<Booking>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Bookings
                .Include(b => b.Hall)
                .Include(b => b.BookingServices).ThenInclude(bs => bs.Service)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
        {
            await _context.Bookings.AddAsync(booking, cancellationToken);
        }

        public Task UpdateAsync(Booking booking)
        {
            _context.Bookings.Update(booking);

            return Task.CompletedTask;
        }

        public Task DeleteAsync(Booking booking)
        {
            _context.Bookings.Remove(booking);

            return Task.CompletedTask;
        }

        public async Task<bool> HasOverlappingBookingAsync(int hallId, DateTime startTime, DateTime endTime, int? excludedBookingId = null, CancellationToken cancellationToken = default)
        {
            // Перевіряємо перетин часових інтервалів, щоб один зал не можна було забронювати одночасно кількома клієнтами
            return await _context.Bookings.AnyAsync(b => b.HallId == hallId && b.Id != excludedBookingId &&
                startTime < b.EndTime && endTime > b.StartTime, cancellationToken);
        }
    }
}
