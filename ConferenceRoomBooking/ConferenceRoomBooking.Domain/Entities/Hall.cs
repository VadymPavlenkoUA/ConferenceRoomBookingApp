using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceRoomBooking.Domain.Entities
{
    public class Hall
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Capacity { get; set; }

        public decimal HourlyRate { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        public ICollection<HallServiceItem> HallServices { get; set; } = new List<HallServiceItem>();
    }
}
