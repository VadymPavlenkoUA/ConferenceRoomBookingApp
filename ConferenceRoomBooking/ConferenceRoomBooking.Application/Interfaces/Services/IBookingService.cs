using ConferenceRoomBooking.Application.DTOs.Bookings;

namespace ConferenceRoomBooking.Application.Interfaces.Services
{
    public interface IBookingService
    {
        Task<BookingResponse> CreateAsync(CreateBookingRequest request, CancellationToken cancellationToken = default);

        Task<BookingResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<List<BookingResponse>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<BookingResponse?> UpdateAsync(int id, UpdateBookingRequest request, CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
