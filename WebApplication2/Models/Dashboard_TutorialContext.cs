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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //optionsBuilder.UseSqlServer("server=localhost;port=3306;user=root;password=12345;database=Dashboard_Tutorial");
                optionsBuilder.UseSqlServer("Server=(localdb)\\.;Integrated Security=true;database=Dashboard_Tutorial");
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
