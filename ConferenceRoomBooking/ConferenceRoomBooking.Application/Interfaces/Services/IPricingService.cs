using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceRoomBooking.Application.Interfaces.Services
{
    public interface IPricingService
    {
        decimal CalculateRentalPrice(decimal hourlyRate, DateTime startTime, DateTime endTime);
    }
}
