using ConferenceRoomBooking.Domain.Entities;

namespace ConferenceRoomBooking.Application.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(int id);
        Task<List<Booking>> GetAllAsync();
        Task AddAsync(Booking booking);
        Task<bool> HasOverlappingBookingAsync(int hallId, DateTime startTime, DateTime endTime);
    }
}
