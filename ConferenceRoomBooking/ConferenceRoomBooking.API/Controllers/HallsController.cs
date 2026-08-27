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

        [HttpGet]
        public async Task<ActionResult<List<HallResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var halls = await _hallService.GetAllAsync(cancellationToken);

            return Ok(halls);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<HallResponse>> GetById(int id, CancellationToken cancellationToken)
        {
            var hall = await _hallService.GetByIdAsync(id, cancellationToken);

            if (hall is null)
            {
                return NotFound();
            }

            return Ok(hall);
        }

        [HttpGet("available")]
        public async Task<ActionResult<List<HallResponse>>> GetAvailable([FromQuery] AvailableHallsRequest request, CancellationToken cancellationToken)
        {
            var halls = await _hallService.GetAvailableAsync(request, cancellationToken);

            return Ok(halls);
        }

        [HttpPost]
        public async Task<ActionResult<HallResponse>> Create(CreateHallRequest request, CancellationToken cancellationToken)
        {
            var hall = await _hallService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = hall.Id }, hall);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<HallResponse>> Update(int id, UpdateHallRequest request, CancellationToken cancellationToken)
        {
            var hall = await _hallService.UpdateAsync(id, request, cancellationToken);

            if (hall is null)
            {
                return NotFound();
            }

            return Ok(hall);
        }

        [HttpDelete("{id:int}")]
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
