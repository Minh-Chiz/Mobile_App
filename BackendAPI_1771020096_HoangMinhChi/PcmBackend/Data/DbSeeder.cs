using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PcmBackend.Models;

namespace PcmBackend.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAll(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // 1. Tự động chạy Migration nếu chưa có Database
                context.Database.Migrate();

                // 2. Tạo Roles (Admin, Member, Treasurer, Referee)
                string[] roles = { "Admin", "Member", "Treasurer", "Referee" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }

                // 3. Tạo Admin (Mật khẩu: P@ssword123)
                if (await userManager.FindByEmailAsync("admin@pcm.com") == null)
                {
                    var adminUser = new IdentityUser { UserName = "admin@pcm.com", Email = "admin@pcm.com" };
                    var result = await userManager.CreateAsync(adminUser, "P@ssword123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }

                // 4. Tạo 20 Members mẫu
                if (!context.Members.Any())
                {
                    var random = new Random();
                    for (int i = 1; i <= 20; i++)
                    {
                        var email = $"member{i}@pcm.com";
                        if (await userManager.FindByEmailAsync(email) == null)
                        {
                            // Tạo tài khoản đăng nhập
                            var user = new IdentityUser { UserName = email, Email = email };
                            await userManager.CreateAsync(user, "P@ssword123");
                            await userManager.AddToRoleAsync(user, "Member");

                            // Tạo Profile Member_096
                            var member = new Members_096
                            {
                                UserId = user.Id,
                                FullName = $"Vợt Thủ {i}",
                                RankLevel = 3.0 + (random.NextDouble() * 2.0), // Rank 3.0 - 5.0
                                Tier = (RankLevel)random.Next(0, 4), // Random hạng
                                WalletBalance = random.Next(2000, 10001) * 1000, // 2tr - 10tr
                                TotalSpent = random.Next(1000, 50000) * 1000,
                                JoinDate = DateTime.Now.AddMonths(-random.Next(1, 12)),
                                IsActive = true
                            };
                            context.Members.Add(member);
                        }
                    }
                    await context.SaveChangesAsync();
                }

                // 5. Tạo Sân (Courts)
                if (!context.Courts.Any())
                {
                    context.Courts.AddRange(
                        new Courts_096 { Name = "Sân 1 (VIP)", PricePerHour = 200000, Description = "Sân thảm chuẩn quốc tế", IsActive = true },
                        new Courts_096 { Name = "Sân 2 (Standard)", PricePerHour = 150000, Description = "Sân tiêu chuẩn", IsActive = true },
                        new Courts_096 { Name = "Sân 3 (Tập luyện)", PricePerHour = 100000, Description = "Sân tập máy bắn bóng", IsActive = true }
                    );
                    await context.SaveChangesAsync();
                }

                // 6. Tạo Giải đấu (Tournaments)
                if (!context.Tournaments.Any())
                {
                    // Giải 1: Đã kết thúc
                    context.Tournaments.Add(new Tournaments_096
                    {
                        Name = "Summer Open 2025",
                        StartDate = DateTime.Now.AddMonths(-6),
                        EndDate = DateTime.Now.AddMonths(-6).AddDays(3),
                        Format = TournamentFormat.Knockout,
                        EntryFee = 500000,
                        PrizePool = 10000000,
                        Status = TournamentStatus.Finished
                    });

                    // Giải 2: Đang mở đăng ký (Để test chức năng Join)
                    context.Tournaments.Add(new Tournaments_096
                    {
                        Name = "Winter Cup 2026",
                        StartDate = DateTime.Now.AddDays(10),
                        EndDate = DateTime.Now.AddDays(15),
                        Format = TournamentFormat.RoundRobin,
                        EntryFee = 300000,
                        PrizePool = 5000000,
                        Status = TournamentStatus.Registering
                    });

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}