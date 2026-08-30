namespace ConferenceRoomBooking.Application.DTOs.Reports
{
    public class ServicePopularityResponse
    {
        public int ServiceId { get; set; }

        public string ServiceName { get; set; } = string.Empty;

        public int UsageCount { get; set; }
    }
}
