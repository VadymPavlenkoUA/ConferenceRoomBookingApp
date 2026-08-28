using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ConferenceRoomBooking.Application.DTOs.Reports
{
    public class ReportPeriodRequest
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}
