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

// 設置資料庫連接（改為 PostgreSQL）
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DB")));  // 使用 Npgsql 提供者

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
