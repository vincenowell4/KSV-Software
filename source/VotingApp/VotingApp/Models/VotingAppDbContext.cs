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

        public virtual DbSet<CreatedVote> CreatedVotes { get; set; } = null!;
        public virtual DbSet<Option> Options { get; set; } = null!;
        public virtual DbSet<SubmittedVote> SubmittedVotes { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<VoteType> VoteTypes { get; set; } = null!;

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

            modelBuilder.Entity<Option>(entity =>
            {
                entity.HasOne(d => d.CreatedVote)
                    .WithMany(p => p.Options)
                    .HasForeignKey(d => d.CreatedVoteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_Options_Created_Vote_ID");
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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
