using ConferenceRoomBooking.Application.DTOs.Reports;
using ConferenceRoomBooking.Application.Interfaces.Repositories;
using ConferenceRoomBooking.Application.Interfaces.Services;

namespace ConferenceRoomBooking.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<BookingStatisticsResponse> GetBookingStatisticsAsync(ReportPeriodRequest request, CancellationToken cancellationToken = default)
        {
            ValidatePeriod(request.From, request.To);

            return await _reportRepository.GetBookingStatisticsAsync(request.From, request.To, cancellationToken);
        }

        public async Task<List<HallPopularityResponse>> GetHallPopularityAsync(ReportPeriodRequest request, CancellationToken cancellationToken = default)
        {
            ValidatePeriod(request.From, request.To);

            return await _reportRepository.GetHallPopularityAsync(request.From, request.To, cancellationToken);
        }

        public async Task<List<ServicePopularityResponse>> GetServicePopularityAsync(ReportPeriodRequest request, CancellationToken cancellationToken = default)
        {
            ValidatePeriod(request.From, request.To);

            return await _reportRepository.GetServicePopularityAsync(request.From, request.To, cancellationToken);
        }

        private static void ValidatePeriod(DateTime from, DateTime to)
        {
            if (from == default || to == default)
            {
                throw new ArgumentException("Both 'from' and 'to' query parameters are required.");
            }

            if (from >= to)
            {
                throw new ArgumentException("The start of the period must be earlier than the end.");
            }
        }
    }
}
