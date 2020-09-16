using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManager.Models
{
    public class MovieManagerContext : DbContext
    {
        public MovieManagerContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<MovieActor> MovieActors { get; set; }
        public DbSet<MovieCategory> MovieCategories { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieActor>().HasKey(t => new { t.MovieId, t.ActorId });

            modelBuilder.Entity<MovieActor>()
                .HasOne(m => m.Movie)
                .WithMany(ma => ma.MovieActors)
                .HasForeignKey(m => m.MovieId);

            modelBuilder.Entity<MovieActor>()
                .HasOne(a => a.Actor)
                .WithMany(am => am.MovieActors)
                .HasForeignKey(a => a.ActorId);


            modelBuilder.Entity<MovieCategory>().HasKey(t => new { t.MovieId, t.CategoryId });

            modelBuilder.Entity<MovieCategory>()
                .HasOne(m => m.Movie)
                .WithMany(mc => mc.MovieCategories)
                .HasForeignKey(m => m.MovieId);

            modelBuilder.Entity<MovieCategory>()
                .HasOne(c => c.Category)
                .WithMany(cm => cm.MovieCategories)
                .HasForeignKey(c=>c.CategoryId);
        }
    }
}
