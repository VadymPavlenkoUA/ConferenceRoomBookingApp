using ConferenceRoomBooking.Domain.Entities;

namespace ConferenceRoomBooking.Application.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<List<Booking>> GetAllAsync(CancellationToken cancellationToken = default);

        Task AddAsync(Booking booking, CancellationToken cancellationToken = default);

        Task UpdateAsync(Booking booking);

        Task DeleteAsync(Booking booking);

        Task<bool> HasOverlappingBookingAsync(int hallId, DateTime startTime, DateTime endTime, int? excludedBookingId = null, CancellationToken cancellationToken = default);
    }
}
