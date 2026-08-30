namespace ConferenceRoomBooking.Application.DTOs.Reports
{
    public class BookingStatisticsResponse
    {
        public int TotalBookings { get; set; }

        public decimal TotalRevenue { get; set; }

        public decimal AverageBookingPrice { get; set; }
    }
}
