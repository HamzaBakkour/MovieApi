using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieApi.Models.Entities;

namespace MovieApi.Data.Configurations;

public class MovieDetailesConfiguration : IEntityTypeConfiguration<MovieDetailes>
{
    public void Configure(EntityTypeBuilder<MovieDetailes> builder)
    {

        builder.HasIndex(d => d.MovieId)
                .IsUnique(); //This ensures one-to-one
    }
}
