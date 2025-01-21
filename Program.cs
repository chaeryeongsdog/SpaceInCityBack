using Microsoft.EntityFrameworkCore;
using MyApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 啟用 CORS（跨來源資源共享）
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
        builder.WithOrigins("http://127.0.0.1:5501")  // 允許你的前端來源
               .AllowAnyHeader()
               .AllowAnyMethod());
});

// 設置資料庫連接
var connectionString = Environment.GetEnvironmentVariable("SpaceInCityDatabase"); // 從環境變數讀取資料庫連接字串
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)); // 使用環境變數中讀取的連接字串

builder.Services.AddControllers();

var app = builder.Build();

// 配置 HTTP 請求管道
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 啟用 CORS 中介軟體
app.UseCors("AllowFrontend");

app.MapControllers();

app.Run();
