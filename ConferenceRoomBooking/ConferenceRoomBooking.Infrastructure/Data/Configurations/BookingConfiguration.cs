using ConferenceRoomBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConferenceRoomBooking.Infrastructure.Data.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TotalPrice).HasPrecision(18, 2);

            builder.Property(x => x.StartTime).IsRequired();

            builder.Property(x => x.EndTime).IsRequired();

            builder.Property(x => x.CreatedAt).IsRequired();

            builder.HasOne(x => x.Hall)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.HallId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new
            {
                x.HallId,
                x.StartTime,
                x.EndTime
            });
        }
    }
}
