using ConferenceRoomBooking.Domain.Entities;

namespace ConferenceRoomBooking.Application.Interfaces.Repositories
{
    public interface IHallRepository
    {
        Task<Hall?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<List<Hall>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<List<Hall>> GetAvailableAsync(int capacity, DateTime startTime, DateTime endTime, CancellationToken cancellationToken = default);

        Task AddAsync(Hall hall, CancellationToken cancellationToken = default);

        Task UpdateAsync(Hall hall);

        Task DeleteAsync(Hall hall);
    }
}
