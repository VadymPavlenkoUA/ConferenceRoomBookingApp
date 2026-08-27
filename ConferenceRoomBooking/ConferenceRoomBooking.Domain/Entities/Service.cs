using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceRoomBooking.Domain.Entities
{
    public class Service
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public ICollection<HallServiceItem> HallServices { get; set; } = new List<HallServiceItem>();

        public ICollection<BookingServiceItem> BookingServices { get; set; } = new List<BookingServiceItem>();
    }
}
