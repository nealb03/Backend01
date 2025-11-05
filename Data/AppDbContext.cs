using Microsoft.EntityFrameworkCore;
using MyWebApi.Models;

namespace MyWebApi.Data
{
    /// <summary>
    /// Legacy compatibility context kept only to satisfy any remaining
    /// service registrations that reference AppDbContext.  
    /// Mirrors Cloud495Context mappings so both target the same database schema.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users => Set<User>();
        public virtual DbSet<Account> Accounts => Set<Account>();
        public virtual DbSet<Transaction> Transactions => Set<Transaction>();
        public virtual DbSet<EfmigrationsHistory> EfmigrationsHistories => Set<EfmigrationsHistory>();
        public virtual DbSet<WeatherForecast> WeatherForecasts => Set<WeatherForecast>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            // ------------------------------
            // Users
            // ------------------------------
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("PRIMARY");
                entity.ToTable("Users");

                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.FirstName).HasMaxLength(50);
                entity.Property(e => e.LastName).HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.Password).HasMaxLength(255);

                entity.HasMany(e => e.Accounts)
                      .WithOne(a => a.User)
                      .HasForeignKey(a => a.UserId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_Accounts_Users_UserId");
            });

            // ------------------------------
            // Accounts
            // ------------------------------
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AccountId).HasName("PRIMARY");
                entity.ToTable("Accounts");

                entity.Property(e => e.AccountId).HasColumnName("AccountId");
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.AccountType).HasMaxLength(100);
                entity.Property(e => e.Balance).HasPrecision(18, 2);
                entity.Property(e => e.CreatedAt)
                      .HasColumnType("timestamp")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasMany(e => e.Transactions)
                      .WithOne(t => t.Account)
                      .HasForeignKey(t => t.AccountId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_Transactions_Accounts_AccountId");
            });

            // ------------------------------
            // Transactions
            // ------------------------------
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId).HasName("PRIMARY");
                entity.ToTable("Transactions");

                entity.Property(e => e.TransactionId).HasColumnName("TransactionId");
                entity.Property(e => e.AccountId).HasColumnName("AccountId");
                entity.Property(e => e.Type).HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.Amount).HasPrecision(18, 2);
                entity.Property(e => e.CreatedAt)
                      .HasColumnType("timestamp")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // ------------------------------
            // EF migrations history
            // ------------------------------
            modelBuilder.Entity<EfmigrationsHistory>(entity =>
            {
                entity.HasKey(e => e.MigrationId).HasName("PRIMARY");
                entity.ToTable("__EFMigrationsHistory");

                entity.Property(e => e.MigrationId).HasMaxLength(150);
                entity.Property(e => e.ProductVersion).HasMaxLength(32);
            });

            // ------------------------------
            // Weather Forecast
            // ------------------------------
            modelBuilder.Entity<WeatherForecast>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");
                entity.ToTable("WeatherForecast");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}