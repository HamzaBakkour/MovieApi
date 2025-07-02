using System.Globalization;
using Bogus;
using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;
using MovieApi.Models.Entities;
namespace MovieApi.Data;



public class SeedData
{
    private static Faker faker = new Faker("en");

    internal static async Task InitAsync(MovieContext context)
    {

        if (await context.Movies.AnyAsync()) return;

        var genres = GenerateGenres(10);
        await context.AddRangeAsync(genres);

        var actors = GenerateActors(100);
        await context.AddRangeAsync(actors);

        var movies = GenerateMovies(30, actors, genres);
        await context.AddRangeAsync(movies);

        await context.SaveChangesAsync();



    }


    private static List<Review> GenerateReviews(int count)
    {
        var reviews = new List<Review>();

        for (int i = 0; i < count; i++)
        {
            reviews.Add(new Review
            {
                ReviewerName = faker.Name.FullName(),
                Comment = faker.Lorem.Sentence(),
                Rating = faker.Random.Int(1, 5)
                // Movie navigation set by EF Core when added to movie.Reviews
            });
        }

        return reviews;
    }



    private static IEnumerable<Genre> GenerateGenres(int numberOfGenres)
    {
        var genres = new List<Genre>(numberOfGenres);

        for (int i = 0; i < numberOfGenres; i++) {
            var genre = new Genre()
            {
                Name = faker.Music.Genre()
            };
            genres.Add(genre);
        }
        return genres;

    }

    private static IEnumerable<Actor> GenerateActors(int numberOfActors)
    {
        var actors = new List<Actor>(numberOfActors);

        var currentYear = DateTime.UtcNow.Year;
        for (int i = 0; i < numberOfActors; i++)
        {
            var birthYear = faker.Date.Between(
                new DateTime(currentYear - 90, 1, 1),
                new DateTime(currentYear - 12, 1, 1)
            ).Year;

            var actor = new Actor()
            {
                Name = faker.Name.FullName(),
                BirthYear = birthYear
            };

            actors.Add(actor);
        }
        
        return actors;
    }


    private static IEnumerable<Movie> GenerateMovies(int numberOfMovies, IEnumerable<Actor> allActors,
                                                                        IEnumerable<Genre> allGenres)
    {
        var movies = new List<Movie>();
        var actorList = allActors.ToList();
        var genreList = allGenres.ToList();
        var currentYear = DateTime.UtcNow.Year;

        for (int i = 0; i < numberOfMovies; i++)
        {
            var genre = faker.PickRandom(genreList);
            var movieActors = faker.PickRandom(actorList, faker.Random.Int(2, 4)).ToList();


            var movie = new Movie
            {
                Title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(faker.Commerce.ProductName()),
                Year = faker.Date.Between(new DateTime(currentYear - 40, 1, 1), DateTime.UtcNow).Year,
                Duration = faker.Random.Int(1, 150),
                Genres = faker.PickRandom(genreList, faker.Random.Int(1, 3)).ToList(),
                Actors = movieActors,
                Detailes = new MovieDetailes
                {
                    Synopsis = faker.Lorem.Paragraph(),
                    Language = faker.Random.ArrayElement(new[] { "sv", "en", "fr", "de", "ar" }),
                    Budget = faker.Random.Int(1_000_000, 100_000_000)
                },
                Reviews = GenerateReviews(faker.Random.Int(1, 3))

            }; 

            movies.Add(movie);
        }

        return movies;
    }

}
