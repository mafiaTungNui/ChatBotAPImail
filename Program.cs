using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("Server=DESKTOP;Database=ChatBot;Trusted_Connection=True;TrustServerCertificate=True;"));

// Đăng ký EmailService
builder.Services.AddSingleton<EmailService>();

// Thêm Controller
builder.Services.AddControllers();

var app = builder.Build();

// Sử dụng Endpoint
app.MapControllers();

app.Run();
