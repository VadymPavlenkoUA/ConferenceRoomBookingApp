using ConferenceRoomBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConferenceRoomBooking.Infrastructure.Data.Configurations
{
    public class HallConfiguration : IEntityTypeConfiguration<Hall>
    {
        public void Configure(EntityTypeBuilder<Hall> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);

            builder.Property(x => x.HourlyRate).HasPrecision(18, 2);

            builder.HasMany(x => x.Bookings)
                .WithOne(x => x.Hall)
                .HasForeignKey(x => x.HallId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.HallServices)
                .WithOne(x => x.Hall)
                .HasForeignKey(x => x.HallId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
