using System.ComponentModel.DataAnnotations;

namespace ConferenceRoomBooking.Application.DTOs.Services
{
    public class UpdateServiceRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(0, 1_000_000)]
        public decimal Price { get; set; }
    }
}
