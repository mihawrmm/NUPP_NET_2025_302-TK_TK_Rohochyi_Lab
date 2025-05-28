using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Animals.Infrastructure.Models;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Animals.Infrastructure
{
    public class AnimalsContext : IdentityDbContext<AppUser>
    {
        public DbSet<AnimalModel> Animals { get; set; }
        public DbSet<MammalModel> Mammals { get; set; }
        public DbSet<BirdModel> Birds { get; set; }
        public DbSet<EnclosureModel> Enclosures { get; set; }

        public AnimalsContext(DbContextOptions<AnimalsContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MammalModel>().ToTable("Mammals");
            modelBuilder.Entity<BirdModel>().ToTable("Birds");
            modelBuilder.Entity<AnimalModel>().ToTable("Animals");

            modelBuilder.Entity<EnclosureModel>()
                .HasMany(e => e.Animals)
                .WithOne(a => a.Enclosure)
                .HasForeignKey(a => a.EnclosureId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

