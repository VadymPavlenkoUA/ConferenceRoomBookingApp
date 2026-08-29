using ConferenceRoomBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceRoomBooking.Infrastructure.Data.Configurations
{
    public class BookingServiceItemConfiguration : IEntityTypeConfiguration<BookingServiceItem>
    {
        public void Configure(EntityTypeBuilder<BookingServiceItem> builder)
        {
            builder.HasKey(x => new
            {
                x.BookingId,
                x.ServiceId
            });

            builder.Property(x => x.Price).HasPrecision(18, 2);

            builder.HasOne(x => x.Booking)
                .WithMany(x => x.BookingServices)
                .HasForeignKey(x => x.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Service)
                .WithMany(x => x.BookingServices)
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
