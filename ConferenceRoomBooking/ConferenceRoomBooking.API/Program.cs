using ConferenceRoomBooking.API.Middleware;
using ConferenceRoomBooking.Application.Interfaces.Repositories;
using ConferenceRoomBooking.Application.Interfaces.Services;
using ConferenceRoomBooking.Application.Services;
using ConferenceRoomBooking.Infrastructure.Data;
using ConferenceRoomBooking.Infrastructure.Data.Seed;
using ConferenceRoomBooking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// IUnitOfWork забезпечує єдине збереження змін для операції, використовуючи той самий екземпляр AppDbContext
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "ConferenceRoomBooking.API.xml"));
});

builder.Services.AddScoped<IHallRepository, HallRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();

builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IHallService, HallService>();
builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddScoped<IPricingService, PricingService>();
builder.Services.AddScoped<IReportService, ReportService>();

builder.Services.AddControllers();

// Обмежуємо кількість запитів від одного клієнта, щоб захистити API від надмірного навантаження та простих DoS-атак
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
        httpContext =>
        {
            var key = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            return RateLimitPartition.GetFixedWindowLimiter(key,
                _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 100,
                    Window = TimeSpan.FromMinutes(1),
                    QueueLimit = 0
                });
        });

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseRateLimiter();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await context.Database.MigrateAsync();
    await DbSeeder.SeedAsync(context);
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
