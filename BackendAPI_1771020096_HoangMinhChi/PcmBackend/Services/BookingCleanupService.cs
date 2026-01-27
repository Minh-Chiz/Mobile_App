using PcmBackend.Data; // [Mới]
using PcmBackend.Models;
using Microsoft.EntityFrameworkCore; // [Mới]

namespace PcmBackend.Services
{
    public class BookingCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public BookingCleanupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Chạy mỗi 1 phút
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

                using (var scope = _serviceProvider.CreateScope())
                {
                    // [Sửa] Dùng AppDbContext thay vì ApplicationDbContext
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    // Tìm các booking trạng thái "PendingPayment" quá 5 phút
                    var thresholdTime = DateTime.Now.AddMinutes(-5);

                    // [Sửa] Dùng context.Bookings (tên trong AppDbContext) thay vì Bookings_096
                    var expiredBookings = context.Bookings
                        .Where(b => b.Status == BookingStatus.PendingPayment
                                    && b.CreatedDate < thresholdTime)
                        .ToList();

                    if (expiredBookings.Any())
                    {
                        foreach (var booking in expiredBookings)
                        {
                            booking.Status = BookingStatus.Cancelled;
                            // Logic hoàn slot...
                        }
                        await context.SaveChangesAsync();
                        Console.WriteLine($"[Auto-Cleanup] Đã hủy {expiredBookings.Count} booking quá hạn.");
                    }
                }
            }
        }
    }
}