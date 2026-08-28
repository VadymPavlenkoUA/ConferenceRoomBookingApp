using ConferenceRoomBooking.Application.DTOs.Reports;
using ConferenceRoomBooking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceRoomBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("bookings/statistics")]
        public async Task<ActionResult<BookingStatisticsResponse>> GetBookingStatistics([FromQuery] ReportPeriodRequest request, CancellationToken cancellationToken)
        {
            var result = await _reportService.GetBookingStatisticsAsync(request, cancellationToken);

            return Ok(result);
        }

        [HttpGet("halls/popularity")]
        public async Task<ActionResult<List<HallPopularityResponse>>> GetHallPopularity([FromQuery] ReportPeriodRequest request, CancellationToken cancellationToken)
        {
            var result = await _reportService.GetHallPopularityAsync(request, cancellationToken);

            return Ok(result);
        }

        [HttpGet("services/popularity")]
        public async Task<ActionResult<List<ServicePopularityResponse>>> GetServicePopularity([FromQuery] ReportPeriodRequest request, CancellationToken cancellationToken)
        {
            var result = await _reportService.GetServicePopularityAsync(request, cancellationToken);

            return Ok(result);
        }
    }
}
