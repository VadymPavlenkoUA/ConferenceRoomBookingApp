using ConferenceRoomBooking.Application.DTOs.Bookings;
using ConferenceRoomBooking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceRoomBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<ActionResult<List<BookingResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var bookings = await _bookingService.GetAllAsync(cancellationToken);

            return Ok(bookings);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookingResponse>> GetById(int id, CancellationToken cancellationToken)
        {
            var booking = await _bookingService.GetByIdAsync(id, cancellationToken);

            if (booking is null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        [HttpPost]
        public async Task<ActionResult<BookingResponse>> Create(CreateBookingRequest request, CancellationToken cancellationToken)
        {
            var booking = await _bookingService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = booking.Id }, booking);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<BookingResponse>> Update(int id, UpdateBookingRequest request, CancellationToken cancellationToken)
        {
            var booking = await _bookingService.UpdateAsync(id, request, cancellationToken);

            if (booking is null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var deleted = await _bookingService.DeleteAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
