using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieApi.Models.Entities;

public class MovieContext : DbContext
{
    public MovieContext (DbContextOptions<MovieContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; } = default!;
    public DbSet<MovieDetailes> MovieDetailes { get; set; } = default!;
    public DbSet<Actor> Actors { get; set; } = default!;
    public DbSet<Review> Reviews { get; set; } = default!;



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<Movie>()
            .HasOne(m => m.Detailes)
            .WithOne(d => d.Movie)
            .HasForeignKey<MovieDetailes>(d => d.MovieId);

        modelBuilder.Entity<MovieDetailes>()
                    .HasIndex(d => d.MovieId)
                    .IsUnique(); //This ensures one-to-one
    }
}
