using ConferenceRoomBooking.Application.DTOs.Reports;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceRoomBooking.Application.Interfaces.Services
{
    public interface IReportService
    {
        Task<BookingStatisticsResponse> GetBookingStatisticsAsync(ReportPeriodRequest request, CancellationToken cancellationToken = default);

        Task<List<HallPopularityResponse>> GetHallPopularityAsync(ReportPeriodRequest request, CancellationToken cancellationToken = default);

        Task<List<ServicePopularityResponse>> GetServicePopularityAsync(ReportPeriodRequest request, CancellationToken cancellationToken = default);
    }
}
