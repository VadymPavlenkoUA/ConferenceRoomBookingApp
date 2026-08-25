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
        public ICollection<HallService> HallServices { get; set; } = new List<HallService>();
        public ICollection<BookingService> BookingServices { get; set; } = new List<BookingService>();
    }
}
