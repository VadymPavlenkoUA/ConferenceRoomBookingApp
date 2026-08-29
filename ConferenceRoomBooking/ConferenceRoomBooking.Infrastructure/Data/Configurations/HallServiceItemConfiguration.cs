using ConferenceRoomBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConferenceRoomBooking.Infrastructure.Data.Configurations
{
    public class HallServiceItemConfiguration : IEntityTypeConfiguration<HallServiceItem>
    {
        public void Configure(EntityTypeBuilder<HallServiceItem> builder)
        {
            builder.HasKey(x => new
            {
                x.HallId,
                x.ServiceId
            });

            builder.HasOne(x => x.Hall)
                .WithMany(x => x.HallServices)
                .HasForeignKey(x => x.HallId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Service)
                .WithMany(x => x.HallServices)
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
