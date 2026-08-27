using ConferenceRoomBooking.Application.DTOs.Services;
using ConferenceRoomBooking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceRoomBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public ServicesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<ServiceResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var services = await _serviceManager.GetAllAsync(cancellationToken);

            return Ok(services);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ServiceResponse>> GetById(int id, CancellationToken cancellationToken)
        {
            var service = await _serviceManager.GetByIdAsync(id, cancellationToken);

            if (service is null)
            {
                return NotFound();
            }

            return Ok(service);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse>> Create(CreateServiceRequest request, CancellationToken cancellationToken)
        {
            var service = await _serviceManager.CreateAsync(request, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = service.Id }, service);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ServiceResponse>> Update(int id, UpdateServiceRequest request, CancellationToken cancellationToken)
        {
            var service = await _serviceManager.UpdateAsync(id, request, cancellationToken);

            if (service is null)
            {
                return NotFound();
            }

            return Ok(service);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var deleted = await _serviceManager.DeleteAsync(id, cancellationToken);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
