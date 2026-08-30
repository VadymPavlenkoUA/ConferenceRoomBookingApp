using ConferenceRoomBooking.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceRoomBooking.Tests.Tests
{
    public class PricingServiceTests
    {
        // Стандартний час

        [Fact]
        public void CalculateRentalPrice_StandardHours_ReturnsCorrectPrice()
        {
            var service = new PricingService();

            var result = service.CalculateRentalPrice(
                2000,
                new DateTime(2026, 9, 1, 10, 0, 0),
                new DateTime(2026, 9, 1, 12, 0, 0));

            Assert.Equal(4000, result);
        }

        // Піковий час

        [Fact]
        public void CalculateRentalPrice_PeakHours_AppliesSurcharge()
        {
            var service = new PricingService();

            var result = service.CalculateRentalPrice(
                2000,
                new DateTime(2026, 9, 1, 12, 0, 0),
                new DateTime(2026, 9, 1, 14, 0, 0));

            Assert.Equal(4600, result);
        }

        // Ранкові години

        [Fact]
        public void CalculateRentalPrice_MorningHours_AppliesDiscount()
        {
            var service = new PricingService();

            var result = service.CalculateRentalPrice(
                2000,
                new DateTime(2026, 9, 1, 7, 0, 0),
                new DateTime(2026, 9, 1, 9, 0, 0));

            Assert.Equal(3600, result);
        }

        // Вечірні години

        [Fact]
        public void CalculateRentalPrice_EveningHours_AppliesDiscount()
        {
            var service = new PricingService();

            var result = service.CalculateRentalPrice(
                2000,
                new DateTime(2026, 9, 1, 18, 0, 0),
                new DateTime(2026, 9, 1, 21, 0, 0));

            Assert.Equal(4800, result);
        }

        // Перевірка змішаного тарифу (наприклад 10:00 - 14:00)

        [Fact]
        public void CalculateRentalPrice_MixedHours_AppliesCorrectRates()
        {
            var service = new PricingService();

            var result = service.CalculateRentalPrice(
                2000,
                new DateTime(2026, 9, 1, 10, 0, 0),
                new DateTime(2026, 9, 1, 14, 0, 0));

            Assert.Equal(8600, result);
        }

        // Перевірка дробових годин (наприклад 10:30 - 13:45)

        [Fact]
        public void CalculateRentalPrice_PartialHours_ReturnsCorrectPrice()
        {
            var service = new PricingService();

            var result = service.CalculateRentalPrice(
                2000,
                new DateTime(2026, 9, 1, 10, 30, 0),
                new DateTime(2026, 9, 1, 13, 45, 0));

            Assert.Equal(7025, result);
        }



        // НЕГАТИВНІ СЦЕНАРІЇ

        // Нульова ціна
        [Fact]
        public void CalculateRentalPrice_ZeroHourlyRate_ThrowsException()
        {
            var service = new PricingService();

            Assert.Throws<ArgumentException>(() => service.CalculateRentalPrice(
                0,
                new DateTime(2026, 9, 1, 10, 0, 0),
                new DateTime(2026, 9, 1, 12, 0, 0)));
        }

        // Початок після кінця (наприклад 14:00 - 10:00)

        [Fact]
        public void CalculateRentalPrice_InvalidTimeRange_ThrowsException()
        {
            var service = new PricingService();

            Assert.Throws<ArgumentException>(() => service.CalculateRentalPrice(
                2000,
                new DateTime(2026, 9, 1, 14, 0, 0),
                new DateTime(2026, 9, 1, 10, 0, 0)));
        }

        // Раніше 6:00 (наприклад 05:00 - 08:00)

        [Fact]
        public void CalculateRentalPrice_BeforeOpeningTime_ThrowsException()
        {
            var service = new PricingService();

            Assert.Throws<ArgumentException>(() => service.CalculateRentalPrice(
                2000,
                new DateTime(2026, 9, 1, 5, 0, 0),
                new DateTime(2026, 9, 1, 8, 0, 0)));
        }

        // Після 23:00 (наприклад 20:00 - 23:30)

        [Fact]
        public void CalculateRentalPrice_AfterClosingTime_ThrowsException()
        {
            var service = new PricingService();

            Assert.Throws<ArgumentException>(() => service.CalculateRentalPrice(
                2000,
                new DateTime(2026, 9, 1, 20, 0, 0),
                new DateTime(2026, 9, 1, 23, 30, 0)));
        }

        // В неробочий час в різні дні

        [Fact]
        public void CalculateRentalPrice_CrossesMidnight_ThrowsException()
        {
            var service = new PricingService();
            Assert.Throws<ArgumentException>(() => service.CalculateRentalPrice(
                2000,
                new DateTime(2026, 9, 1, 22, 0, 0),
                new DateTime(2026, 9, 2, 2, 0, 0)));
        }
    }
}
