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

        /// <summary>
        /// Returns booking statistics for the specified period
        /// </summary>
        /// <param name="request">The report period</param>
        /// <returns>Booking statistics including total bookings, revenue and average booking price</returns>
        [HttpGet("bookings/statistics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BookingStatisticsResponse>> GetBookingStatistics([FromQuery] ReportPeriodRequest request, CancellationToken cancellationToken)
        {
            var result = await _reportService.GetBookingStatisticsAsync(request, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Returns halls ranked by booking popularity for the specified period
        /// </summary>
        /// <param name="request">The report period</param>
        /// <returns>A list of halls ordered by the number of bookings</returns>
        [HttpGet("halls/popularity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<HallPopularityResponse>>> GetHallPopularity([FromQuery] ReportPeriodRequest request, CancellationToken cancellationToken)
        {
            var result = await _reportService.GetHallPopularityAsync(request, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Returns services ranked by usage for the specified period
        /// </summary>
        /// <param name="request">The report period</param>
        /// <returns>A list of services ordered by usage count</returns>
        [HttpGet("services/popularity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<ServicePopularityResponse>>> GetServicePopularity([FromQuery] ReportPeriodRequest request, CancellationToken cancellationToken)
        {
            var result = await _reportService.GetServicePopularityAsync(request, cancellationToken);

            return Ok(result);
        }
    }
}
