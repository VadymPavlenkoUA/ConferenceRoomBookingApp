using ConferenceRoomBooking.Domain.Entities;

namespace ConferenceRoomBooking.Application.Interfaces.Repositories
{
    public interface IHallRepository
    {
        Task<Hall?> GetByIdAsync(int id);
        Task<List<Hall>> GetAllAsync();
        Task AddAsync(Hall hall);
        Task UpdateAsync(Hall hall);
        Task DeleteAsync(Hall hall);

    }
}
