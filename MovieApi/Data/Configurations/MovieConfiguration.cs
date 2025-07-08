using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieApi.Models.Entities;

namespace MovieApi.Data.Configurations;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasOne(m => m.Detailes)
                .WithOne(d => d.Movie)
                .HasForeignKey<MovieDetailes>(d => d.MovieId);

        builder.Property(m => m.Title)
            .HasMaxLength(150);

    }
}
