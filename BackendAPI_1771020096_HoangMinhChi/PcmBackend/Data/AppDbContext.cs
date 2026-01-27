using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // [MỚI 1] Thêm dòng này
using Microsoft.EntityFrameworkCore;
using PcmBackend.Models;

namespace PcmBackend.Data
{
    // [MỚI 2] Sửa DbContext thành IdentityDbContext
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // --- 1. Nhóm Quản trị (Operations) & Wallet ---
        public DbSet<Members_096> Members { get; set; }
        public DbSet<WalletTransactions_096> WalletTransactions { get; set; }
        public DbSet<News_096> News { get; set; }
        public DbSet<TransactionCategories_096> TransactionCategories { get; set; }
        public DbSet<Notifications_096> Notifications { get; set; }

        // --- 2. Nhóm Sân & Booking ---
        public DbSet<Courts_096> Courts { get; set; }
        public DbSet<Bookings_096> Bookings { get; set; }

        // --- 3. Nhóm Thi đấu (Sports Core) ---
        public DbSet<Tournaments_096> Tournaments { get; set; }
        public DbSet<TournamentParticipants_096> TournamentParticipants { get; set; }
        public DbSet<Matches_096> Matches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // [QUAN TRỌNG] Phải giữ dòng này để Identity cấu hình các bảng của nó

            // Cấu hình mối quan hệ giữa Matches và Tournaments
            modelBuilder.Entity<Matches_096>()
                .HasOne(m => m.Tournament)
                .WithMany(t => t.Matches)
                .HasForeignKey(m => m.TournamentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình decimal
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.GetProperties()
                    .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?));

                foreach (var property in properties)
                {
                    property.SetColumnType("decimal(18,2)");
                }
            }
        }
    }
}