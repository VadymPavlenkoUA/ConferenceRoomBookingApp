using ConferenceRoomBooking.Application.DTOs.Services;

namespace ConferenceRoomBooking.Application.DTOs.Bookings
{
    public class BookingResponse
    {
        public int Id { get; set; }

        public int HallId { get; set; }

        public string HallName { get; set; } = string.Empty;

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public decimal RentalPrice { get; set; }

        public decimal ServicesPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<ServiceResponse> Services { get; set; } = [];
    }
}
