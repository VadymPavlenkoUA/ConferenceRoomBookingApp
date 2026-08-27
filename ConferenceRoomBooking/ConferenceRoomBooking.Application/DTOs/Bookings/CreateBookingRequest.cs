using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ConferenceRoomBooking.Application.DTOs.Bookings
{
    public class CreateBookingRequest
    {
        [Range(1, int.MaxValue)]
        public int HallId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public List<int> ServiceIds { get; set; } = [];
    }
}
