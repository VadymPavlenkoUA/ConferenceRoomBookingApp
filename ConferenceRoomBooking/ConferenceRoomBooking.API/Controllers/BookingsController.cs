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

        /// <summary>
        /// Returns all bookings
        /// </summary>
        /// <returns>A list of bookings</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<BookingResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var bookings = await _bookingService.GetAllAsync(cancellationToken);

            return Ok(bookings);
        }

        /// <summary>
        /// Returns a booking by its identifier
        /// </summary>
        /// <param name="id">The booking identifier</param>
        /// <returns>The requested booking</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingResponse>> GetById(int id, CancellationToken cancellationToken)
        {
            var booking = await _bookingService.GetByIdAsync(id, cancellationToken);

            if (booking is null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        /// <summary>
        /// Creates a new booking
        /// </summary>
        /// <returns>The created booking</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<BookingResponse>> Create(CreateBookingRequest request, CancellationToken cancellationToken)
        {
            var booking = await _bookingService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = booking.Id }, booking);
        }

        /// <summary>
        /// Updates an existing booking
        /// </summary>
        /// <param name="id">The booking identifier</param>
        /// <returns>The updated booking</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<BookingResponse>> Update(int id, UpdateBookingRequest request, CancellationToken cancellationToken)
        {
            var booking = await _bookingService.UpdateAsync(id, request, cancellationToken);

            if (booking is null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        /// <summary>
        /// Deletes an existing booking
        /// </summary>
        /// <param name="id">The booking identifier</param>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
