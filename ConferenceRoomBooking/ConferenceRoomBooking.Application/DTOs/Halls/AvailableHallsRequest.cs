namespace ConferenceRoomBooking.Application.DTOs.Halls
{
    public class AvailableHallsRequest
    {
        public int Capacity { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
