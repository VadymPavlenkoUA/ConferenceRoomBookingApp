using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ConferenceRoomBooking.Application.DTOs.Services
{
    public class CreateServiceRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(0, 1_000_000)]
        public decimal Price { get; set; }
    }
}
