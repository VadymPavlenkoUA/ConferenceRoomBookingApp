using ConferenceRoomBooking.Application.DTOs.Services;

namespace ConferenceRoomBooking.Application.DTOs.Halls
{

    public class HallResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Capacity { get; set; }

        public decimal HourlyRate { get; set; }

        public List<ServiceResponse> Services { get; set; } = [];
    }
}
