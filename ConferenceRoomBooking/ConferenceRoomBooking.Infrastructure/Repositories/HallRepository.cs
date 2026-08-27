using ConferenceRoomBooking.Application.Interfaces.Repositories;
using ConferenceRoomBooking.Domain.Entities;
using ConferenceRoomBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ConferenceRoomBooking.Infrastructure.Repositories
{
    public class HallRepository : IHallRepository
    {
        private readonly AppDbContext _context;

        public HallRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Hall?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Halls
                .Include(h => h.HallServices)
                .ThenInclude(hs => hs.Service)
                .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);
        }

        public async Task<List<Hall>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Halls
                .Include(h => h.HallServices)
                .ThenInclude(hs => hs.Service)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Hall>> GetAvailableAsync(int capacity, DateTime startTime, DateTime endTime, CancellationToken cancellationToken = default)
        {
            return await _context.Halls.Where(h => h.Capacity >= capacity && !h.Bookings.Any(b =>
                        startTime < b.EndTime && endTime > b.StartTime)).ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Hall hall, CancellationToken cancellationToken = default)
        {
            await _context.Halls.AddAsync(hall, cancellationToken);
        }

        public Task UpdateAsync(Hall hall)
        {
            _context.Halls.Update(hall);

            return Task.CompletedTask;
        }

        public Task DeleteAsync(Hall hall)
        {
            _context.Halls.Remove(hall);

            return Task.CompletedTask;
        }
    }
}
