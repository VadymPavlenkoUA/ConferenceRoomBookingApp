using ConferenceRoomBooking.Application.DTOs.Reports;
using ConferenceRoomBooking.Application.Interfaces.Repositories;
using ConferenceRoomBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ConferenceRoomBooking.Infrastructure.Repositories
{

    // Агрегація звітів виконується на рівні БД, щоб не завантажувати всі записи в пам'ять
    // та ефективно використовувати можливості SQL для групування й підрахунку даних

    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _context;

        public ReportRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BookingStatisticsResponse> GetBookingStatisticsAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default)
        {
            var statistics = await _context.Bookings.Where(b => b.StartTime >= from && b.StartTime < to).GroupBy(_ => 1)
                .Select(g => new BookingStatisticsResponse
                {
                    TotalBookings = g.Count(),
                    TotalRevenue = g.Sum(b => b.TotalPrice),
                    AverageBookingPrice = g.Average(b => b.TotalPrice)
                })
                .FirstOrDefaultAsync(cancellationToken);

            return statistics ?? new BookingStatisticsResponse();
        }

        public async Task<List<HallPopularityResponse>> GetHallPopularityAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default)
        {
            return await _context.Bookings.Where(b => b.StartTime >= from && b.StartTime < to)
                .GroupBy(b => new
                {
                    b.HallId,
                    b.Hall.Name
                })
                .Select(g => new HallPopularityResponse
                {
                    HallId = g.Key.HallId,
                    HallName = g.Key.Name,
                    BookingCount = g.Count()
                })
                .OrderByDescending(x => x.BookingCount).ToListAsync(cancellationToken);
        }

        public async Task<List<ServicePopularityResponse>> GetServicePopularityAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default)
        {
            return await _context.BookingServices.Where(bs => bs.Booking.StartTime >= from && bs.Booking.StartTime < to)
                .GroupBy(bs => new
                {
                    bs.ServiceId,
                    bs.Service.Name
                })
                .Select(g => new ServicePopularityResponse
                {
                    ServiceId = g.Key.ServiceId,
                    ServiceName = g.Key.Name,
                    UsageCount = g.Count()
                })
                .OrderByDescending(x => x.UsageCount).ToListAsync(cancellationToken);
        }
    }
}
