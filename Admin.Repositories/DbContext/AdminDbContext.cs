using Microsoft.EntityFrameworkCore;

namespace Admin.Repositories.DbContext
{
    public class AdminDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public AdminDbContext()
        {
        }

        public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Admin.Domain.Model.User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            const string dateTimeColumnType = "DATETIME";
            const int guidColumnLength = 36;

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Admin.Domain.Model.User>().ToTable("Users");
            modelBuilder.Entity<Admin.Domain.Model.User>().HasIndex(pu => new { pu.UserId }).IsUnique();
            modelBuilder.Entity<Admin.Domain.Model.User>(entity =>
            {
                entity.HasKey(m => m.UserId);
                entity.Property(m => m.UserId).HasMaxLength(guidColumnLength);
                entity.Property(m => m.CreatedDate).HasColumnType(dateTimeColumnType);
                entity.Property(m => m.UpdatedDate).HasColumnType(dateTimeColumnType);
            });
        }
    }
}
