using ConferenceRoomBooking.Application.DTOs.Halls;
using ConferenceRoomBooking.Application.DTOs.Services;
using ConferenceRoomBooking.Application.Interfaces.Repositories;
using ConferenceRoomBooking.Application.Interfaces.Services;
using ConferenceRoomBooking.Domain.Entities;

namespace ConferenceRoomBooking.Application.Services
{
    public class HallService : IHallService
    {
        private readonly IHallRepository _hallRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public HallService(IHallRepository hallRepository, IServiceRepository serviceRepository, IUnitOfWork unitOfWork)
        {
            _hallRepository = hallRepository;
            _serviceRepository = serviceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<HallResponse> CreateAsync(CreateHallRequest request, CancellationToken cancellationToken = default)
        {
            ValidateHall(request.Name, request.Capacity, request.HourlyRate);

            var services = await GetServicesAsync(request.ServiceIds, cancellationToken);

            var hall = new Hall
            {
                Name = request.Name,
                Capacity = request.Capacity,
                HourlyRate = request.HourlyRate
            };

            foreach (var service in services)
            {
                hall.HallServices.Add(
                    new HallServiceItem
                    {
                        ServiceId = service.Id
                    });
            }

            await _hallRepository.AddAsync(hall, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return MapToResponse(hall);
        }

        public async Task<HallResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var hall = await _hallRepository.GetByIdAsync(id, cancellationToken);

            if (hall is null)
            {
                return null;
            }

            return MapToResponse(hall);
        }

        public async Task<List<HallResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var halls = await _hallRepository.GetAllAsync(cancellationToken);

            return halls.Select(MapToResponse).ToList();
        }

        public async Task<List<HallResponse>> GetAvailableAsync(AvailableHallsRequest request, CancellationToken cancellationToken = default)
        {
            if (request.Capacity <= 0)
            {
                throw new ArgumentException("Capacity must be greater than zero.");
            }

            if (request.StartTime >= request.EndTime)
            {
                throw new ArgumentException("Start time must be earlier than end time.");
            }

            // Пошук виконується на рівні репозиторію, щоб одразу виключити з результату зали, зайняті у вказаний період
            var halls = await _hallRepository.GetAvailableAsync(request.Capacity, request.StartTime, request.EndTime, cancellationToken);

            return halls.Select(MapToResponse).ToList();
        }

        public async Task<HallResponse?> UpdateAsync(int id, UpdateHallRequest request, CancellationToken cancellationToken = default)
        {
            ValidateHall(request.Name, request.Capacity, request.HourlyRate);

            var hall = await _hallRepository.GetByIdAsync(id, cancellationToken);

            if (hall is null)
            {
                return null;
            }

            var services = await GetServicesAsync(request.ServiceIds, cancellationToken);

            hall.Name = request.Name;
            hall.Capacity = request.Capacity;
            hall.HourlyRate = request.HourlyRate;

            hall.HallServices.Clear();

            foreach (var service in services)
            {
                hall.HallServices.Add(
                    new HallServiceItem
                    {
                        HallId = hall.Id,
                        ServiceId = service.Id
                    });
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return MapToResponse(hall);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var hall = await _hallRepository.GetByIdAsync(id, cancellationToken);

            if (hall is null)
            {
                return false;
            }

            // Не дозволяємо видаляти зал, який використовується в існуючих бронюваннях, щоб не порушити цілісність даних
            var hasBookings = await _hallRepository.HasBookingsAsync(id, cancellationToken);

            if (hasBookings)
            {
                throw new InvalidOperationException("Hall cannot be deleted because it has existing bookings.");
            }

            await _hallRepository.DeleteAsync(hall);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        private async Task<List<Service>> GetServicesAsync(IEnumerable<int> serviceIds, CancellationToken cancellationToken)
        {
            var requestedIds = serviceIds.Distinct().ToList();

            if (requestedIds.Count == 0)
            {
                return [];
            }

            var services = await _serviceRepository.GetByIdsAsync(requestedIds, cancellationToken);

            if (services.Count != requestedIds.Count)
            {
                var foundIds = services.Select(s => s.Id).ToHashSet();

                var missingIds = requestedIds.Where(id => !foundIds.Contains(id));

                throw new KeyNotFoundException($"Services not found: {string.Join(", ", missingIds)}");
            }

            return services;
        }

        private static void ValidateHall(string name, int capacity, decimal hourlyRate)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Hall name is required.");
            }

            if (capacity <= 0)
            {
                throw new ArgumentException("Capacity must be greater than zero.");
            }

            if (hourlyRate < 0)
            {
                throw new ArgumentException("Hourly rate cannot be negative.");
            }
        }

        private static HallResponse MapToResponse(Hall hall)
        {
            return new HallResponse
            {
                Id = hall.Id,
                Name = hall.Name,
                Capacity = hall.Capacity,
                HourlyRate = hall.HourlyRate,

                Services = hall.HallServices
                    .Select(hs => new ServiceResponse
                    {
                        Id = hs.ServiceId,
                        Name = hs.Service.Name,
                        Price = hs.Service.Price
                    }).ToList()
            };
        }
    }
}
