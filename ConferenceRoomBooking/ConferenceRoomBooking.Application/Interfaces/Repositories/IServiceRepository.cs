using ConferenceRoomBooking.Domain.Entities;

namespace ConferenceRoomBooking.Application.Interfaces.Repositories
{
    public interface IServiceRepository
    {
        Task<Service?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<List<Service>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);

        Task<List<Service>> GetAllAsync(CancellationToken cancellationToken = default);

        Task AddAsync(Service service, CancellationToken cancellationToken = default);

        Task UpdateAsync(Service service);

        Task DeleteAsync(Service service);
    }
}
