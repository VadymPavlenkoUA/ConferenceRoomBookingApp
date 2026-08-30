namespace ConferenceRoomBooking.Domain.Entities
{
    public class BookingServiceItem
    {
        public int BookingId { get; set; }

        public Booking Booking { get; set; } = null!;

        public int ServiceId { get; set; }

        public Service Service { get; set; } = null!;

        public decimal Price { get; set; }
    }
}
