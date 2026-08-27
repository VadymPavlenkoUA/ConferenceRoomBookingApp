using ConferenceRoomBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConferenceRoomBooking.Infrastructure.Data.Seed
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (await context.Halls.AnyAsync() || await context.Services.AnyAsync())
            {
                return;
            }

            var projector = new Service
            {
                Name = "Проєктор",
                Price = 500
            };

            var wiFi = new Service
            {
                Name = "Wi-Fi",
                Price = 300
            };

            var sound = new Service
            {
                Name = "Звук",
                Price = 700
            };

            var hallA = new Hall
            {
                Name = "Зал A",
                Capacity = 50,
                HourlyRate = 2000
            };

            var hallB = new Hall
            {
                Name = "Зал B",
                Capacity = 100,
                HourlyRate = 3500
            };

            var hallC = new Hall
            {
                Name = "Зал C",
                Capacity = 30,
                HourlyRate = 1500
            };

            context.Services.AddRange(projector, wiFi, sound);
            context.Halls.AddRange(hallA, hallB, hallC);

            await context.SaveChangesAsync();

            var hallServices = new[]
            {
                new HallServiceItem
                {
                    HallId = hallA.Id,
                    ServiceId = projector.Id
                },
                new HallServiceItem
                {
                    HallId = hallA.Id,
                    ServiceId = wiFi.Id
                },

                new HallServiceItem
                {
                    HallId = hallB.Id,
                    ServiceId = projector.Id
                },
                new HallServiceItem
                {
                    HallId = hallB.Id,
                    ServiceId = wiFi.Id
                },
                new HallServiceItem
                {
                    HallId = hallB.Id,
                    ServiceId = sound.Id
                },

                new HallServiceItem
                {
                    HallId = hallC.Id,
                    ServiceId = wiFi.Id
                }
            };

            context.HallServices.AddRange(hallServices);

            await context.SaveChangesAsync();
        }
    }
}
