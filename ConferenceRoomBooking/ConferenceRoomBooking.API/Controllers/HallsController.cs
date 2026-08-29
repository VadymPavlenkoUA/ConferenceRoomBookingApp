using ConferenceRoomBooking.Application.DTOs.Halls;
using ConferenceRoomBooking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceRoomBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HallsController : ControllerBase
    {
        private readonly IHallService _hallService;

        public HallsController(IHallService hallService)
        {
            _hallService = hallService;
        }

        /// <summary>
        /// Returns all halls
        /// </summary>
        /// <returns>A list of halls</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<HallResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var halls = await _hallService.GetAllAsync(cancellationToken);

            return Ok(halls);
        }

        /// <summary>
        /// Returns a hall by its identifier
        /// </summary>
        /// <param name="id">The hall identifier</param>
        /// <returns>The requested hall</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HallResponse>> GetById(int id, CancellationToken cancellationToken)
        {
            var hall = await _hallService.GetByIdAsync(id, cancellationToken);

            if (hall is null)
            {
                return NotFound();
            }

            return Ok(hall);
        }

        /// <summary>
        /// Returns halls available for the specified time and capacity
        /// </summary>
        /// <param name="request">The availability search parameters</param>
        /// <returns>A list of available halls</returns>
        [HttpGet("available")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<HallResponse>>> GetAvailable([FromQuery] AvailableHallsRequest request, CancellationToken cancellationToken)
        {
            var halls = await _hallService.GetAvailableAsync(request, cancellationToken);

            return Ok(halls);
        }

        /// <summary>
        /// Creates a new hall
        /// </summary>
        /// <param name="request">The data used to create the hall</param>
        /// <returns>The created hall</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HallResponse>> Create(CreateHallRequest request, CancellationToken cancellationToken)
        {
            var hall = await _hallService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = hall.Id }, hall);
        }

        /// <summary>
        /// Updates an existing hall
        /// </summary>
        /// <param name="id">The hall identifier</param>
        /// <param name="request">The data used to update the hall</param>
        /// <returns>The updated hall</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HallResponse>> Update(int id, UpdateHallRequest request, CancellationToken cancellationToken)
        {
            var hall = await _hallService.UpdateAsync(id, request, cancellationToken);

            if (hall is null)
            {
                return NotFound();
            }

            return Ok(hall);
        }

        /// <summary>
        /// Deletes an existing hall
        /// </summary>
        /// <param name="id">The hall identifier</param>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var deleted = await _hallService.DeleteAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
