using ConferenceRoomBooking.Application.DTOs.Bookings;
using ConferenceRoomBooking.Application.DTOs.Services;
using ConferenceRoomBooking.Application.Interfaces.Repositories;
using ConferenceRoomBooking.Application.Interfaces.Services;
using ConferenceRoomBooking.Domain.Entities;

namespace ConferenceRoomBooking.Application.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IHallRepository _hallRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IPricingService _pricingService;
    private readonly IUnitOfWork _unitOfWork;

    public BookingService(
        IBookingRepository bookingRepository,
        IHallRepository hallRepository,
        IServiceRepository serviceRepository,
        IPricingService pricingService,
        IUnitOfWork unitOfWork)
    {
        _bookingRepository = bookingRepository;
        _hallRepository = hallRepository;
        _serviceRepository = serviceRepository;
        _pricingService = pricingService;
        _unitOfWork = unitOfWork;
    }

    public async Task<BookingResponse> CreateAsync(CreateBookingRequest request, CancellationToken cancellationToken = default)
    {
        ValidateTimeRange(request.StartTime, request.EndTime);

        var hall = await _hallRepository.GetByIdAsync(request.HallId, cancellationToken);

        if (hall is null)
        {
            throw new KeyNotFoundException($"Hall with ID {request.HallId} was not found.");
        }

        var hasOverlap = await _bookingRepository.HasOverlappingBookingAsync(
                request.HallId,
                request.StartTime,
                request.EndTime,
                cancellationToken: cancellationToken);

        if (hasOverlap)
        {
            throw new InvalidOperationException("The hall is already booked for the selected time.");
        }

        var services = await GetServicesAsync(request.ServiceIds, cancellationToken);

        var rentalPrice = _pricingService.CalculateRentalPrice(hall.HourlyRate, request.StartTime, request.EndTime);

        var servicesPrice = services.Sum(s => s.Price);

        var totalPrice = rentalPrice + servicesPrice;

        var booking = new Booking
        {
            HallId = hall.Id,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            RentalPrice = rentalPrice,
            ServicesPrice = servicesPrice,
            TotalPrice = totalPrice,
            CreatedAt = DateTime.UtcNow
        };

        foreach (var service in services)
        {
            booking.BookingServices.Add(
                new BookingServiceItem
                {
                    Booking = booking,
                    ServiceId = service.Id,
                    Price = service.Price
                });
        }

        await _bookingRepository.AddAsync(booking, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToResponse(booking, hall);
    }

    public async Task<BookingResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByIdAsync(id, cancellationToken);

        if (booking is null)
        {
            return null;
        }

        return MapToResponse(booking, booking.Hall);
    }

    public async Task<List<BookingResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var bookings = await _bookingRepository.GetAllAsync(cancellationToken);

        return bookings.Select(booking => MapToResponse(booking, booking.Hall)).ToList();
    }

    public async Task<BookingResponse?> UpdateAsync(int id, UpdateBookingRequest request, CancellationToken cancellationToken = default)
    {
        ValidateTimeRange(request.StartTime, request.EndTime);

        var booking = await _bookingRepository.GetByIdAsync(id, cancellationToken);

        if (booking is null)
        {
            return null;
        }

        var hall = await _hallRepository.GetByIdAsync(request.HallId, cancellationToken);

        if (hall is null)
        {
            throw new KeyNotFoundException($"Hall with ID {request.HallId} was not found.");
        }

        var hasOverlap = await _bookingRepository.HasOverlappingBookingAsync(
                request.HallId,
                request.StartTime,
                request.EndTime,
                id,
                cancellationToken);

        if (hasOverlap)
        {
            throw new InvalidOperationException("The hall is already booked for the selected time.");
        }

        var services = await GetServicesAsync(request.ServiceIds, cancellationToken);

        var rentalPrice = _pricingService.CalculateRentalPrice(hall.HourlyRate, request.StartTime, request.EndTime);

        var servicesPrice = services.Sum(s => s.Price);

        var totalPrice = rentalPrice + servicesPrice;

        booking.HallId = hall.Id;
        booking.StartTime = request.StartTime;
        booking.EndTime = request.EndTime;
        booking.RentalPrice = rentalPrice;
        booking.ServicesPrice = servicesPrice;
        booking.TotalPrice = totalPrice;

        booking.BookingServices.Clear();

        foreach (var service in services)
        {
            booking.BookingServices.Add(
                new BookingServiceItem
                {
                    BookingId = booking.Id,
                    ServiceId = service.Id,
                    Price = service.Price
                });
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToResponse(booking, hall);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByIdAsync(id, cancellationToken);

        if (booking is null)
        {
            return false;
        }

        await _bookingRepository.DeleteAsync(booking);

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

    private static void ValidateTimeRange(DateTime startTime, DateTime endTime)
    {
        if (startTime >= endTime)
        {
            throw new ArgumentException("Start time must be earlier than end time.");
        }
    }

    private static BookingResponse MapToResponse(Booking booking, Hall hall)
    {
        return new BookingResponse
        {
            Id = booking.Id,
            HallId = hall.Id,
            HallName = hall.Name,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            RentalPrice = booking.RentalPrice,
            ServicesPrice = booking.ServicesPrice,
            TotalPrice = booking.TotalPrice,
            CreatedAt = booking.CreatedAt,
            Services = booking.BookingServices
                .Select(bs => new ServiceResponse
                {
                    Id = bs.ServiceId,
                    Name = bs.Service.Name,
                    Price = bs.Price
                }).ToList()
        };
    }
}