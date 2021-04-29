using Microsoft.EntityFrameworkCore;
using System;
using AlterDeep.DBOperations.Model;

namespace AlterDeep.DBOperations
{
    public class DeepContext : DbContext
    {
        public DbSet<TransactionPage> TransactionPages { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Flow> Flows { get; set; }
        public DbSet<TransactionPageContents> TransactionPageContents { get; set; }
        
        public DeepContext(DbContextOptions<DeepContext> options) : base(options)
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(false);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TransactionPage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Content>(entity => { entity.HasKey(e => e.Id); });
            modelBuilder.Entity<Flow>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<TransactionPageContents>().HasKey(sc => new { sc.TransactionPageId, sc.ContentId });

            modelBuilder.Entity<TransactionPageContents>()
                .HasOne<TransactionPage>(sc => sc.TransactionPage)
                .WithMany(s => s.TransactionPageContents)
                .HasForeignKey(sc => sc.TransactionPageId);


            modelBuilder.Entity<TransactionPageContents>()
                .HasOne<Content>(sc => sc.Content)
                .WithMany(s => s.TransactionPageContents)
                .HasForeignKey(sc => sc.ContentId);
        }       
    }
}