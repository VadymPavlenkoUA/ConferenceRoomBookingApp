using ConferenceRoomBooking.Application.DTOs.Reports;

namespace ConferenceRoomBooking.Application.Interfaces.Repositories
{
    public interface IReportRepository
    {
        Task<BookingStatisticsResponse> GetBookingStatisticsAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);

        Task<List<HallPopularityResponse>> GetHallPopularityAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);

        Task<List<ServicePopularityResponse>> GetServicePopularityAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);
    }
}
