using ConferenceRoomBooking.Application.DTOs.Services;
using ConferenceRoomBooking.Application.Interfaces.Repositories;
using ConferenceRoomBooking.Application.Interfaces.Services;
using ConferenceRoomBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceRoomBooking.Application.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceManager(IServiceRepository serviceRepository, IUnitOfWork unitOfWork)
        {
            _serviceRepository = serviceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse> CreateAsync(CreateServiceRequest request, CancellationToken cancellationToken = default)
        {
            ValidateService(request.Name, request.Price);

            var service = new Service
            {
                Name = request.Name,
                Price = request.Price
            };

            await _serviceRepository.AddAsync(service, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return MapToResponse(service);
        }

        public async Task<ServiceResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var service = await _serviceRepository.GetByIdAsync(id, cancellationToken);

            if (service is null)
            {
                return null;
            }

            return MapToResponse(service);
        }

        public async Task<List<ServiceResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var services = await _serviceRepository.GetAllAsync(cancellationToken);

            return services.Select(MapToResponse).ToList();
        }

        public async Task<ServiceResponse?> UpdateAsync(int id, UpdateServiceRequest request, CancellationToken cancellationToken = default)
        {
            ValidateService(request.Name, request.Price);

            var service = await _serviceRepository.GetByIdAsync(id, cancellationToken);

            if (service is null)
            {
                return null;
            }

            service.Name = request.Name;
            service.Price = request.Price;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return MapToResponse(service);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var service = await _serviceRepository.GetByIdAsync(id, cancellationToken);

            if (service is null)
            {
                return false;
            }

            var isUsed = await _serviceRepository.IsUsedAsync(id, cancellationToken);

            if (isUsed)
            {
                throw new InvalidOperationException("Service cannot be deleted because it is used by a hall or booking.");
            }

            await _serviceRepository.DeleteAsync(service);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        private static void ValidateService(string name, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Service name is required.");
            }

            if (price < 0)
            {
                throw new ArgumentException("Service price cannot be negative.");
            }
        }

        private static ServiceResponse MapToResponse(Service service)
        {
            return new ServiceResponse
            {
                Id = service.Id,
                Name = service.Name,
                Price = service.Price
            };
        }
    }
}
