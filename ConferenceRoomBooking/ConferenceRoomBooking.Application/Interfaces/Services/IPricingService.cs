namespace ConferenceRoomBooking.Application.Interfaces.Services
{
    public interface IPricingService
    {
        decimal CalculateRentalPrice(decimal hourlyRate, DateTime startTime, DateTime endTime);
    }
}
