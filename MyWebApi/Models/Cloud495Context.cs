using System;
using Microsoft.EntityFrameworkCore;

namespace MyWebApi.Models
{
    public partial class Cloud495Context : DbContext
    {
        public Cloud495Context()
        {
        }

        public Cloud495Context(DbContextOptions<Cloud495Context> options)
            : base(options)
        {
        }

        // -----------------------------
        // DbSets
        // -----------------------------
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<EfmigrationsHistory> EfmigrationsHistories { get; set; } = null!;
        public virtual DbSet<WeatherForecast> WeatherForecasts { get; set; } = null!;

        // -----------------------------
        // OnConfiguring
        // -----------------------------
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Connection string pulled from configuration or environment
                var connString = Environment.GetEnvironmentVariable("CLOUD495_CONN") ??
                                 "server=cloud495.cmv0ysw00iqp.us-east-1.rds.amazonaws.com;port=3306;database=cloud495;user=admin;password=password";

                optionsBuilder.UseMySql(connString, ServerVersion.Parse("8.0.40-mysql"));
            }
        }

        // -----------------------------
        // OnModelCreating
        // -----------------------------
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            //-------------------------------------------------
            // Users
            //-------------------------------------------------
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("PRIMARY");
                entity.ToTable("Users");

                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.FirstName).HasMaxLength(50);
                entity.Property(e => e.LastName).HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.Password).HasMaxLength(255);
            });

            //-------------------------------------------------
            // Accounts
            //-------------------------------------------------
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

                entity.HasOne(d => d.User)
                      .WithMany(p => p.Accounts)
                      .HasForeignKey(d => d.UserId)
                      .HasConstraintName("FK_Accounts_Users_UserId");
            });

            //-------------------------------------------------
            // Transactions
            //-------------------------------------------------
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

                // Relationship: Transaction → Account (many‑to‑one)
                entity.HasOne(d => d.Account)
                      .WithMany(p => p.Transactions)
                      .HasForeignKey(d => d.AccountId)
                      .HasConstraintName("FK_Transactions_Accounts_AccountId");
            });

            //-------------------------------------------------
            // EF migrations history
            //-------------------------------------------------
            modelBuilder.Entity<EfmigrationsHistory>(entity =>
            {
                entity.HasKey(e => e.MigrationId).HasName("PRIMARY");
                entity.ToTable("__EFMigrationsHistory");

                entity.Property(e => e.MigrationId).HasMaxLength(150);
                entity.Property(e => e.ProductVersion).HasMaxLength(32);
            });

            //-------------------------------------------------
            // Weather Forecast
            //-------------------------------------------------
            modelBuilder.Entity<WeatherForecast>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");
                entity.ToTable("WeatherForecast");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}