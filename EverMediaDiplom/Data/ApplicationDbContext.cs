using EverMediaDiplom.Models;
using Microsoft.EntityFrameworkCore;

namespace EverMediaDiplom.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<PhotoSession> PhotoSessions { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Photo>()
                .HasOne(p => p.User)
                .WithMany(u => u.Photos)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Photo>()
                .HasOne(p => p.PhotoSession)
                .WithMany(ps => ps.Photos)
                .HasForeignKey(p => p.PhotoSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.PhotoSession)
                .WithMany(ps => ps.Payments)
                .HasForeignKey(p => p.PhotoSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PhotoSession>()
                .HasOne(ps => ps.Client)
                .WithMany(u => u.ClientSessions)
                .HasForeignKey(ps => ps.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhotoSession>()
                .HasOne(ps => ps.Photographer)
                .WithMany(u => u.PhotographerSessions)
                .HasForeignKey(ps => ps.PhotographerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PhotoSession>()
                .HasOne(ps => ps.Service)
                .WithMany(s => s.PhotoSessions)
                .HasForeignKey(ps => ps.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<PhotoSession>()
                .HasIndex(ps => ps.SessionDate);

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasDefaultValue("User");

            modelBuilder.Entity<Photo>()
                .Property(p => p.UploadDate)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Payment>()
                .Property(p => p.PaymentDate)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, Name = "Администратор", Email = "admin@example.com", Password = "admin123", Role = "Admin" },
                new User { UserId = 2, Name = "Фотограф Иванов", Email = "photographer@example.com", Password = "photo123", Role = "Photographer" },
                new User { UserId = 3, Name = "Клиент Петров", Email = "client@example.com", Password = "client123", Role = "User" }
            );

            modelBuilder.Entity<Service>().HasData(
                new Service { ServiceId = 1, Name = "Свадебная фотосъемка", Description = "Полный день съемки + обработка фото", Price = 30000, DurationHours = 10 },
                new Service { ServiceId = 2, Name = "Портретная съемка", Description = "Студийная съемка 1,5 часа", Price = 8000, DurationHours = 2 }
            );

            modelBuilder.Entity<PhotoSession>()
                .HasOne(ps => ps.Client)
                .WithMany(u => u.ClientSessions)
                .HasForeignKey(ps => ps.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhotoSession>()
                .HasOne(ps => ps.Photographer)
                .WithMany(u => u.PhotographerSessions)
                .HasForeignKey(ps => ps.PhotographerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhotoSession>()
                .HasOne(ps => ps.Service)
                .WithMany(s => s.PhotoSessions)
                .HasForeignKey(ps => ps.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Photo>().HasData(
                new Photo
                {
                    PhotoId = 1,
                    PhotoSessionId = 1,
                    UserId = 2,
                    Url = "sample/preview1.jpg",
                    ThumbnailUrl = "sample/thumbs/preview1.jpg",
                    IsApproved = true,
                    UploadDate = DateTime.UtcNow
                }
            );

            modelBuilder.Entity<Payment>().HasData(
                new Payment
                {
                    PaymentId = 1,
                    PhotoSessionId = 1,
                    Amount = 15000,
                    Status = "Paid",
                    TransactionId = "INV-2023-001",
                    PaymentDate = DateTime.UtcNow.AddDays(-3)
                }
            );
        }
    }
}