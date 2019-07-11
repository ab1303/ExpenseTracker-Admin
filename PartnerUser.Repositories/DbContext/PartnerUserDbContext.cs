using Microsoft.EntityFrameworkCore;

namespace PartnerUser.Repositories.DbContext
{
    public class PartnerUserDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public PartnerUserDbContext()
        {
        }

        public PartnerUserDbContext(DbContextOptions<PartnerUserDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Domain.Model.PartnerUser> PartnerUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            const string dateTimeColumnType = "DATETIME";
            const int guidColumnLength = 36;

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Domain.Model.PartnerUser>().ToTable("PartnerUsers");
            modelBuilder.Entity<Domain.Model.PartnerUser>().HasIndex(pu => new { pu.OfxUserGuid, pu.PartnerAppId }).IsUnique();
            modelBuilder.Entity<Domain.Model.PartnerUser>(entity =>
            {
                entity.HasKey(m => m.PartnerUserId);
                entity.Property(m => m.PartnerUserId).HasMaxLength(guidColumnLength);
                entity.Property(m => m.OfxUserGuid).HasMaxLength(guidColumnLength);
                entity.Property(m => m.PartnerAppId).HasMaxLength(guidColumnLength);
                entity.Property(m => m.BeneficiaryId).HasMaxLength(guidColumnLength);
                entity.Property(m => m.CreatedDate).HasColumnType(dateTimeColumnType);
                entity.Property(m => m.UpdatedDate).HasColumnType(dateTimeColumnType);
            });
        }
    }
}
