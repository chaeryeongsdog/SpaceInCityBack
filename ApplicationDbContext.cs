using Microsoft.EntityFrameworkCore;

namespace MyApi.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // 定義資料表 Messages
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 設置 Messages 表的主鍵和自動遞增
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.Id); // 設定主鍵

                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd(); // 設定自動遞增

                entity.Property(e => e.Content)
                      .HasMaxLength(50) // 限制字元長度
                      .IsRequired();    // 必填欄位

                entity.Property(e => e.Count)
                      .IsRequired();    // 必填欄位
            });
        }
    }
}
