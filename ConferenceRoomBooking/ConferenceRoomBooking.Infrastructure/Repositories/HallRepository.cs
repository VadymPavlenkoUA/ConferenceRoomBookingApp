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

        public async Task<Hall?> GetByIdAsync(int id)
        {
            return await _context.Halls
                .Include(h => h.HallServices)
                .ThenInclude(hs => hs.Service)
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<List<Hall>> GetAllAsync()
        {
            return await _context.Halls
                .Include(h => h.HallServices)
                .ThenInclude(hs => hs.Service)
                .ToListAsync();
        }

        public async Task AddAsync(Hall hall)
        {
            await _context.Halls.AddAsync(hall);
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
