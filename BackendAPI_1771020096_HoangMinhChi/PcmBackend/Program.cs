using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PcmBackend.Data;
using PcmBackend.Hubs;
using PcmBackend.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Cấu hình Database (SQL Server)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Cấu hình Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// 3. Cấu hình Authentication & JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "YourSuperSecretKey1234567890123456");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// 4. Cấu hình CORS (Cho phép Flutter kết nối)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 5. Thêm các dịch vụ hệ thống
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();

// 6. Cấu hình Swagger (Luôn hiện để bạn dễ test)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PcmBackend API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] { }
        }
    });
});

// 7. Đăng ký Background Service
builder.Services.AddHostedService<BookingCleanupService>();

var app = builder.Build();

// --- Cấu hình HTTP Request Pipeline ---

// Bật Swagger cho cả môi trường Development và Production trong Docker
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PcmBackend API v1");
    c.RoutePrefix = "swagger"; // Truy cập qua: http://<ip>:8080/swagger
});

// Quan trọng: Áp dụng CORS trước Auth
app.UseCors("AllowAll");

// app.UseHttpsRedirection(); // Tắt dòng này khi chạy Docker/HTTP để tránh lỗi SSL trên mobile

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<PcmHub>("/pcmHub");

// 8. Tự động chạy Migration và Seed dữ liệu khi khởi động
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        // Tự động tạo bảng nếu chưa có
        context.Database.Migrate();
        // Chèn dữ liệu mẫu
        DbSeeder.SeedAll(services).Wait();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Lỗi xảy ra trong quá trình khởi tạo Database (Migration/Seeding).");
    }
}

app.Run();