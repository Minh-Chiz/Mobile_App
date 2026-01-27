using PcmBackend.Hubs;
using PcmBackend.Services;
using PcmBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. CẤU HÌNH DB
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. CẤU HÌNH IDENTITY
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// 3. Cấu hình JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("DayLaDoanMaBiMat_DaiHon32KyTu_ChoBaoMat_@123456"))
    };
});

// --- [QUAN TRỌNG] THÊM DỊCH VỤ CORS ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()   // Cho phép mọi nguồn (Web, Mobile...)
                   .AllowAnyMethod()   // Cho phép mọi hành động (GET, POST...)
                   .AllowAnyHeader();  // Cho phép mọi Header
        });
});
// --------------------------------------

builder.Services.AddHostedService<BookingCleanupService>();
builder.Services.AddSignalR();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- 4. CHẠY DATA SEEDER (TẠO DỮ LIỆU MẪU) ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await DbSeeder.SeedAll(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Lỗi xảy ra khi tạo dữ liệu mẫu (Seeding Data).");
    }
}
// ---------------------------------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// --- [QUAN TRỌNG] TẮT HTTPS REDIRECTION ĐỂ TRÁNH LỖI TRÊN WEB ---
// app.UseHttpsRedirection(); 
// (Đã comment dòng trên để không bắt buộc chuyển sang HTTPS)

// --- [QUAN TRỌNG] KÍCH HOẠT CORS (Phải đặt trước Auth) ---
app.UseCors("AllowAll");
// ---------------------------------------------------------

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<PcmHub>("/pcmHub");

app.Run();