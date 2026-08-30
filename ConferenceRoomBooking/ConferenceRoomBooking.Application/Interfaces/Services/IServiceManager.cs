using ConferenceRoomBooking.Application.DTOs.Services;

namespace ConferenceRoomBooking.Application.Interfaces.Services
{
    public interface IServiceManager
    {
        Task<ServiceResponse> CreateAsync(CreateServiceRequest request, CancellationToken cancellationToken = default);

        Task<ServiceResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<List<ServiceResponse>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<ServiceResponse?> UpdateAsync(int id, UpdateServiceRequest request, CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
