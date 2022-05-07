using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace VotingApp.Models
{
    public partial class VotingAppDbContext : DbContext
    {
        public VotingAppDbContext()
        {
        }

        public VotingAppDbContext(DbContextOptions<VotingAppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppLog> AppLogs { get; set; } = null!;
        public virtual DbSet<CreatedVote> CreatedVotes { get; set; } = null!;
        public virtual DbSet<SubmittedVote> SubmittedVotes { get; set; } = null!;
        public virtual DbSet<VoteAuthorizedUser> VoteAuthorizedUsers { get; set; } = null!;
        public virtual DbSet<VoteOption> VoteOptions { get; set; } = null!;
        public virtual DbSet<VoteTimeZone> VoteTimeZones { get; set; } = null!;
        public virtual DbSet<VoteType> VoteTypes { get; set; } = null!;
        public virtual DbSet<VotingUser> VotingUsers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=VotingAppConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CreatedVote>(entity =>
            {
                entity.HasOne(d => d.TimeZone)
                    .WithMany(p => p.CreatedVotes)
                    .HasForeignKey(d => d.TimeZoneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_TimeZone_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CreatedVotes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("Fk_Created_Vote_User_ID");

                entity.HasOne(d => d.VoteType)
                    .WithMany(p => p.CreatedVotes)
                    .HasForeignKey(d => d.VoteTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_Vote_Type_ID");
            });

            modelBuilder.Entity<SubmittedVote>(entity =>
            {
                entity.HasOne(d => d.CreatedVote)
                    .WithMany(p => p.SubmittedVotes)
                    .HasForeignKey(d => d.CreatedVoteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_Created_Vote_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SubmittedVotes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("Fk_Submitted_Vote_User_ID");
            });

            modelBuilder.Entity<VoteAuthorizedUser>(entity =>
            {
                entity.HasOne(d => d.CreatedVote)
                    .WithMany(p => p.VoteAuthorizedUsers)
                    .HasForeignKey(d => d.CreatedVoteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_AuthorizedUsers_Created_Vote_ID");
            });

            modelBuilder.Entity<VoteOption>(entity =>
            {
                entity.HasOne(d => d.CreatedVote)
                    .WithMany(p => p.VoteOptions)
                    .HasForeignKey(d => d.CreatedVoteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_Options_Created_Vote_ID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
