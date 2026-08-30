namespace ConferenceRoomBooking.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }

        public int HallId { get; set; }

        public Hall Hall { get; set; } = null!;

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public decimal RentalPrice { get; set; }

        public decimal ServicesPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<BookingServiceItem> BookingServices { get; set; } = new List<BookingServiceItem>();
    }
}
