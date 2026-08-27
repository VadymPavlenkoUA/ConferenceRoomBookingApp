using ConferenceRoomBooking.Application.DTOs.Halls;

namespace ConferenceRoomBooking.Application.Interfaces.Services
{
    public interface IHallService
    {
        Task<HallResponse> CreateAsync(CreateHallRequest request, CancellationToken cancellationToken = default);

        Task<HallResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<List<HallResponse>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<List<HallResponse>> GetAvailableAsync(AvailableHallsRequest request, CancellationToken cancellationToken = default);

        Task<HallResponse?> UpdateAsync(int id, UpdateHallRequest request, CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
