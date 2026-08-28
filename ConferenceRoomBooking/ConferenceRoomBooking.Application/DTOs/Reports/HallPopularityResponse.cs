using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceRoomBooking.Application.DTOs.Reports
{
    public class HallPopularityResponse
    {
        public int HallId { get; set; }

        public string HallName { get; set; } = string.Empty;

        public int BookingCount { get; set; }
    }
}
