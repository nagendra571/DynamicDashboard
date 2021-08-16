using Microsoft.EntityFrameworkCore;

namespace Dynamic_User_Defined_Dashboards.Models
{
    public partial class Dashboard_TutorialContext : DbContext
    {
        public Dashboard_TutorialContext()
        {
        }

        public Dashboard_TutorialContext(DbContextOptions<Dashboard_TutorialContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DashboardLinkedElements> DashboardLinkedElements { get; set; }
        public virtual DbSet<DashboardsInfo> DashboardsInfo { get; set; }
        public virtual DbSet<Elements> Elements { get; set; }
        public virtual DbSet<Templates> Templates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=tcp:otisbiwebapp.database.windows.net,1433;Initial Catalog=WebAppDB_Dev;Persist Security Info=False;User ID=OtisBI_App;Password=Otisglobal@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<DashboardLinkedElements>(entity =>
            {
                entity.ToTable("DashboardLinkedElements", "dbo");

                entity.Property(e => e.Id).HasColumnType("int");

                entity.Property(e => e.DashboardId).HasColumnType("int");

                entity.Property(e => e.ElementId).HasColumnType("int");

                entity.Property(e => e.Placement)
                    .IsRequired()
                    .HasMaxLength(55)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DashboardsInfo>(entity =>
            {
                entity.ToTable("DashboardsInfo", "dbo");

                entity.Property(e => e.Id).HasColumnType("int");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TemplateId).HasColumnType("int");
            });

            modelBuilder.Entity<Elements>(entity =>
            {
                entity.ToTable("Elements", "dbo");

                entity.Property(e => e.Id).HasColumnType("int");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Templates>(entity =>
            {
                entity.ToTable("Templates", "dbo");

                entity.Property(e => e.Id).HasColumnType("int");

                entity.Property(e => e.ElementsCount).HasColumnType("int");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });
        }
    }
}
