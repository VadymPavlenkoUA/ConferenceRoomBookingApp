using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ConferenceRoomBooking.Application.DTOs.Halls
{
    public class UpdateHallRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int Capacity { get; set; }

        [Range(0, 1_000_000)]
        public decimal HourlyRate { get; set; }

        public List<int> ServiceIds { get; set; } = [];
    }
}
