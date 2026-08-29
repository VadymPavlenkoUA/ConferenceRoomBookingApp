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

        /// <summary>
        /// Returns all services
        /// </summary>
        /// <returns>A list of services</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ServiceResponse>>> GetAll(CancellationToken cancellationToken)
        {
            var services = await _serviceManager.GetAllAsync(cancellationToken);

            return Ok(services);
        }

        /// <summary>
        /// Returns a service by its identifier
        /// </summary>
        /// <param name="id">The service identifier</param>
        /// <returns>The requested service</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServiceResponse>> GetById(int id, CancellationToken cancellationToken)
        {
            var service = await _serviceManager.GetByIdAsync(id, cancellationToken);

            if (service is null)
            {
                return NotFound();
            }

            return Ok(service);
        }

        /// <summary>
        /// Creates a new service
        /// </summary>
        /// <param name="request">The data used to create the service</param>
        /// <returns>The created service</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ServiceResponse>> Create(CreateServiceRequest request, CancellationToken cancellationToken)
        {
            var service = await _serviceManager.CreateAsync(request, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = service.Id }, service);
        }

        /// <summary>
        /// Updates an existing service
        /// </summary>
        /// <param name="id">The service identifier</param>
        /// <param name="request">The data used to update the service</param>
        /// <returns>The updated service</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServiceResponse>> Update(int id, UpdateServiceRequest request, CancellationToken cancellationToken)
        {
            var service = await _serviceManager.UpdateAsync(id, request, cancellationToken);

            if (service is null)
            {
                return NotFound();
            }

            return Ok(service);
        }

        /// <summary>
        /// Deletes an existing service
        /// </summary>
        /// <param name="id">The service identifier</param>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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
