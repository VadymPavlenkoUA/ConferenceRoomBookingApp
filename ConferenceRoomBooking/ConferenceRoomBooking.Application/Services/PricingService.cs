using ConferenceRoomBooking.Application.Interfaces.Services;

namespace ConferenceRoomBooking.Application.Services
{
    public class PricingService : IPricingService
    {
        private const decimal MorningMultiplier = 0.90m;
        private const decimal StandardMultiplier = 1.00m;
        private const decimal PeakMultiplier = 1.15m;
        private const decimal EveningMultiplier = 0.80m;

        private static readonly TimeSpan OpeningTime = new(6, 0, 0);

        private static readonly TimeSpan ClosingTime = new(23, 0, 0);

        public decimal CalculateRentalPrice(decimal hourlyRate, DateTime startTime, DateTime endTime)
        {
            ValidateInput(hourlyRate, startTime, endTime);

            var start = startTime.TimeOfDay;
            var end = endTime.TimeOfDay;

            decimal totalPrice = 0;

            while (start < end)
            {
                var nextBoundary = GetNextBoundary(start, end);
                var multiplier = GetPriceMultiplier(start);

                var duration = nextBoundary - start;

                // Розраховуємо вартість окремими часовими інтервалами, оскільки різні періоди дня мають різні тарифи
                totalPrice += (decimal)duration.TotalHours * hourlyRate * multiplier;

                start = nextBoundary;
            }

            return decimal.Round(totalPrice, 2, MidpointRounding.AwayFromZero);
        }

        private static void ValidateInput(decimal hourlyRate, DateTime startTime, DateTime endTime)
        {
            if (hourlyRate <= 0)
            {
                throw new ArgumentException("Hourly rate must be greater than zero.", nameof(hourlyRate));
            }

            if (startTime >= endTime)
            {
                throw new ArgumentException("Start time must be earlier than end time.");
            }

            if (startTime.Date != endTime.Date)
            {
                throw new ArgumentException("Booking must start and end on the same day.");
            }

            var start = startTime.TimeOfDay;
            var end = endTime.TimeOfDay;

            // Бронювання дозволені лише в межах робочого часу конференц-залу
            if (start < OpeningTime || end > ClosingTime)
            {
                throw new ArgumentException("Booking time must be between 06:00 and 23:00.");
            }
        }

        private static TimeSpan GetNextBoundary(TimeSpan currentTime, TimeSpan endTime)
        {
            var boundaries = new[]
            {
                new TimeSpan(9, 0, 0),
                new TimeSpan(12, 0, 0),
                new TimeSpan(14, 0, 0),
                new TimeSpan(18, 0, 0),
                ClosingTime
            };

            foreach (var boundary in boundaries)
            {
                if (boundary > currentTime)
                {
                    return boundary < endTime ? boundary : endTime;
                }
            }

            return endTime;
        }

        private static decimal GetPriceMultiplier(TimeSpan time)
        {
            // Різні часові періоди використовують різні коефіцієнти вартості відповідно до тарифної політики
            if (time >= OpeningTime && time < new TimeSpan(9, 0, 0))
            {
                return MorningMultiplier;
            }

            if (time >= new TimeSpan(12, 0, 0) && time < new TimeSpan(14, 0, 0))
            {
                return PeakMultiplier;
            }

            if (time >= new TimeSpan(18, 0, 0) && time < ClosingTime)
            {
                return EveningMultiplier;
            }

            return StandardMultiplier;
        }
    }
}
