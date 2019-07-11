using Microsoft.EntityFrameworkCore;

namespace PartnerUser.Persistence.DbContext
{
    public class PartnerUserDbContext : Microsoft.EntityFrameworkCore.DbContext
    {

        public PartnerUserDbContext(DbContextOptions<PartnerUserDbContext> options) : base(options)
        {
        }

        public DbSet<Entity.PartnerUser> PartnerUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var dateTimeColumnType = "TIMESTAMP";
            var guidColumnLength = 36;

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Entity.PartnerUser>().ToTable("PartnerUsers");

            modelBuilder.Entity<Entity.PartnerUser>(entity =>
            {
                entity.HasKey(m => m.PartnerUserId);
                entity.Property(m => m.PartnerUserId).HasMaxLength(guidColumnLength);
                entity.Property(m => m.OfxUserGuid).HasMaxLength(guidColumnLength);
                entity.Property(m => m.PartnerAppId).HasMaxLength(guidColumnLength);
                entity.Property(m => m.BeneficiaryId).HasMaxLength(guidColumnLength);
                entity.Property(m => m.CreatedDate).HasColumnType(dateTimeColumnType).ValueGeneratedOnAdd();
                entity.Property(m => m.UpdatedDate).HasColumnType(dateTimeColumnType).ValueGeneratedOnAdd();
            });
        }
    }
}
