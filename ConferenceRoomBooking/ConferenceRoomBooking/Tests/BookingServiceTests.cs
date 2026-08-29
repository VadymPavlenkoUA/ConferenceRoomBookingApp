using ConferenceRoomBooking.Application.DTOs.Bookings;
using ConferenceRoomBooking.Application.Interfaces.Repositories;
using ConferenceRoomBooking.Application.Interfaces.Services;
using ConferenceRoomBooking.Application.Services;
using ConferenceRoomBooking.Domain.Entities;
using Moq;

namespace ConferenceRoomBooking.Services
{
    public class BookingServiceTests
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IHallRepository> _hallRepositoryMock;
        private readonly Mock<IServiceRepository> _serviceRepositoryMock;
        private readonly Mock<IPricingService> _pricingServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        private readonly BookingService _bookingService;

        public BookingServiceTests()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _hallRepositoryMock = new Mock<IHallRepository>();
            _serviceRepositoryMock = new Mock<IServiceRepository>();
            _pricingServiceMock = new Mock<IPricingService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _bookingService = new BookingService(
                _bookingRepositoryMock.Object,
                _hallRepositoryMock.Object,
                _serviceRepositoryMock.Object,
                _pricingServiceMock.Object,
                _unitOfWorkMock.Object);
        }

        // Успішне створення бронювання

        [Fact]
        public async Task CreateAsync_ValidRequest_CreatesBooking()
        {
            // Arrange
            var request = new CreateBookingRequest
            {
                HallId = 1,
                StartTime = new DateTime(2026, 9, 1, 10, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 12, 0, 0),
                ServiceIds = new List<int> { 1 }
            };

            var hall = new Hall
            {
                Id = 1,
                Name = "Conference Hall",
                Capacity = 20,
                HourlyRate = 2000
            };

            var service = new Service
            {
                Id = 1,
                Name = "Coffee",
                Price = 400
            };

            _hallRepositoryMock.Setup(x => x.GetByIdAsync(request.HallId, It.IsAny<CancellationToken>())).ReturnsAsync(hall);

            _bookingRepositoryMock
                .Setup(x => x.HasOverlappingBookingAsync(
                    request.HallId,
                    request.StartTime,
                    request.EndTime,
                    null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _serviceRepositoryMock
                .Setup(x => x.GetByIdsAsync(
                    It.IsAny<IEnumerable<int>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Service> { service });

            _pricingServiceMock
                .Setup(x => x.CalculateRentalPrice(
                    hall.HourlyRate,
                    request.StartTime,
                    request.EndTime))
                .Returns(4000);

            _bookingRepositoryMock
                .Setup(x => x.AddAsync(
                    It.IsAny<Booking>(),
                    It.IsAny<CancellationToken>()))
                .Callback<Booking, CancellationToken>((booking, _) =>
                {
                    foreach (var bookingService in booking.BookingServices)
                    {
                        bookingService.Service = service;
                    }
                });

            // Act
            var result = await _bookingService.CreateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.HallId);
            Assert.Equal("Conference Hall", result.HallName);
            Assert.Equal(4000, result.RentalPrice);
            Assert.Equal(400, result.ServicesPrice);
            Assert.Equal(4400, result.TotalPrice);
            Assert.Single(result.Services);

            _bookingRepositoryMock.Verify(
                x => x.AddAsync(
                    It.IsAny<Booking>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }


        // Зал не знайдено

        [Fact]
        public async Task CreateAsync_HallDoesNotExist_ThrowsKeyNotFoundException()
        {
            // Arrange
            var request = new CreateBookingRequest
            {
                HallId = 999,
                StartTime = new DateTime(2026, 9, 1, 10, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 12, 0, 0),
                ServiceIds = []
            };

            _hallRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    request.HallId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Hall?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _bookingService.CreateAsync(request));
        }


        // Некоректний часовий діапазон

        [Fact]
        public async Task CreateAsync_InvalidTimeRange_ThrowsArgumentException()
        {
            // Arrange
            var request = new CreateBookingRequest
            {
                HallId = 1,
                StartTime = new DateTime(2026, 9, 1, 14, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 10, 0, 0),
                ServiceIds = []
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _bookingService.CreateAsync(request));
        }


        // Зал уже заброньований у вибраний час

        [Fact]
        public async Task CreateAsync_HasOverlap_ThrowsInvalidOperationException()
        {
            // Arrange
            var request = new CreateBookingRequest
            {
                HallId = 1,
                StartTime = new DateTime(2026, 9, 1, 10, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 12, 0, 0),
                ServiceIds = []
            };

            var hall = new Hall
            {
                Id = 1,
                Name = "Conference Hall",
                Capacity = 20,
                HourlyRate = 2000
            };

            _hallRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    request.HallId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(hall);

            _bookingRepositoryMock
                .Setup(x => x.HasOverlappingBookingAsync(
                    request.HallId,
                    request.StartTime,
                    request.EndTime,
                    null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _bookingService.CreateAsync(request));
        }


        // Послуга не знайдена

        [Fact]
        public async Task CreateAsync_ServiceDoesNotExist_ThrowsKeyNotFoundException()
        {
            // Arrange
            var request = new CreateBookingRequest
            {
                HallId = 1,
                StartTime = new DateTime(2026, 9, 1, 10, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 12, 0, 0),
                ServiceIds = new List<int> { 1, 2 }
            };

            var hall = new Hall
            {
                Id = 1,
                Name = "Conference Hall",
                Capacity = 20,
                HourlyRate = 2000
            };

            _hallRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    request.HallId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(hall);

            _bookingRepositoryMock
                .Setup(x => x.HasOverlappingBookingAsync(
                    request.HallId,
                    request.StartTime,
                    request.EndTime,
                    null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _serviceRepositoryMock
                .Setup(x => x.GetByIdsAsync(
                    It.IsAny<IEnumerable<int>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Service>
                {
                    new Service
                    {
                        Id = 1,
                        Name = "Coffee",
                        Price = 400
                    }
                });

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _bookingService.CreateAsync(request));
        }


        // Бронювання для оновлення не знайдено

        [Fact]
        public async Task UpdateAsync_BookingDoesNotExist_ReturnsNull()
        {
            // Arrange
            var request = new UpdateBookingRequest
            {
                HallId = 1,
                StartTime = new DateTime(2026, 9, 1, 10, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 12, 0, 0),
                ServiceIds = []
            };

            _bookingRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    999,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Booking?)null);

            // Act
            var result = await _bookingService.UpdateAsync(999, request);

            // Assert
            Assert.Null(result);
        }


        // Оновлення бронювання конфліктує з іншим бронюванням

        [Fact]
        public async Task UpdateAsync_HasOverlap_ThrowsInvalidOperationException()
        {
            // Arrange
            var booking = new Booking
            {
                Id = 1,
                HallId = 1,
                StartTime = new DateTime(2026, 9, 1, 10, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 12, 0, 0)
            };

            var hall = new Hall
            {
                Id = 1,
                Name = "Conference Hall",
                Capacity = 20,
                HourlyRate = 2000
            };

            var request = new UpdateBookingRequest
            {
                HallId = 1,
                StartTime = new DateTime(2026, 9, 1, 14, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 16, 0, 0),
                ServiceIds = []
            };

            _bookingRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(booking);

            _hallRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(hall);

            _bookingRepositoryMock
                .Setup(x => x.HasOverlappingBookingAsync(
                    1,
                    request.StartTime,
                    request.EndTime,
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _bookingService.UpdateAsync(1, request));
        }


        // Успішне оновлення бронювання

        [Fact]
        public async Task UpdateAsync_ValidRequest_UpdatesBooking()
        {
            // Arrange
            var booking = new Booking
            {
                Id = 1,
                HallId = 1,
                StartTime = new DateTime(2026, 9, 1, 10, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 12, 0, 0)
            };

            var hall = new Hall
            {
                Id = 1,
                Name = "Conference Hall",
                Capacity = 20,
                HourlyRate = 2000
            };

            var request = new UpdateBookingRequest
            {
                HallId = 1,
                StartTime = new DateTime(2026, 9, 1, 14, 0, 0),
                EndTime = new DateTime(2026, 9, 1, 16, 0, 0),
                ServiceIds = []
            };

            _bookingRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(booking);

            _hallRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(hall);

            _bookingRepositoryMock
                .Setup(x => x.HasOverlappingBookingAsync(
                    1,
                    request.StartTime,
                    request.EndTime,
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _serviceRepositoryMock
                .Setup(x => x.GetByIdsAsync(
                    It.IsAny<IEnumerable<int>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Service>());

            _pricingServiceMock
                .Setup(x => x.CalculateRentalPrice(
                    hall.HourlyRate,
                    request.StartTime,
                    request.EndTime))
                .Returns(4000);

            // Act
            var result = await _bookingService.UpdateAsync(1, request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(new DateTime(2026, 9, 1, 14, 0, 0), result.StartTime);
            Assert.Equal(new DateTime(2026, 9, 1, 16, 0, 0), result.EndTime);
            Assert.Equal(4000, result.TotalPrice);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }


        // Успішне видалення бронювання

        [Fact]
        public async Task DeleteAsync_BookingExists_DeletesBooking()
        {
            // Arrange
            var booking = new Booking
            {
                Id = 1,
                HallId = 1
            };

            _bookingRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(booking);

            // Act
            var result = await _bookingService.DeleteAsync(1);

            // Assert
            Assert.True(result);

            _bookingRepositoryMock.Verify(x => x.DeleteAsync(booking), Times.Once);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }


        // Бронювання для видалення не знайдено

        [Fact]
        public async Task DeleteAsync_BookingDoesNotExist_ReturnsFalse()
        {
            // Arrange
            _bookingRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    999,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Booking?)null);

            // Act
            var result = await _bookingService.DeleteAsync(999);

            // Assert
            Assert.False(result);

            _bookingRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Booking>()), Times.Never);
        }
    }
}