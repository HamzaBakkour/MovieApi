using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Models.Dtos;
using MovieApi.Models.Entities;

namespace MovieApi.Controllers;



[Route("api/reviews")]
[ApiController]
public class ReviewsController : ControllerBase
{
    private readonly MovieContext _context;

    public ReviewsController(MovieContext context)
    {
        _context = context;
    }



    [HttpPost("/api/movies/{movieId}/reviews")]
    public async Task<ActionResult<ReviewDetailsDto>> PostReview([FromRoute, Range(0, int.MaxValue)] int movieId, [FromBody] ReviewCreateDto dto)
    {

        var movie = await _context.Movies.FindAsync(movieId);
        if (movie == null)
            return NotFound($"Movie with ID {movieId} not found.");


        var review = new Review
        {
            MovieId = movie.Id,
            ReviewerName = dto.ReviewerName,
            Comment = dto.Comment,
            Rating = dto.Rating,
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        var response = new ReviewDetailsDto(review.Id,
                                        review.ReviewerName,
                                        review.Comment ?? string.Empty,
                                        review.Rating,
                                        new MovieDto(movie.Id, movie.Title, movie.Year, movie.Duration)
                                        );

        return CreatedAtAction(nameof(GetMovieReviews), new { movieId = movie.Id }, response);

    }




    [HttpGet("/api/movies/{movieId}/reviews")]
    public async Task<ActionResult<ReviewDetailsDto>> GetMovieReviews([FromRoute, Range(0, int.MaxValue)] int movieId)
    {

        var movie = await _context.Movies
            .Include(m => m.Reviews)
            .FirstOrDefaultAsync(m => m.Id == movieId);

        if (movie == null)
            return NotFound($"Movie with ID {movieId} not found.");


        var response = new ReviewMovieDto(new MovieDto(movie.Id,
                                                        movie.Title,
                                                        movie.Year,
                                                        movie.Duration),
                                                        movie.Reviews
                                                            .Select(r => new ReviewDto(r.ReviewerName, r.Comment ?? string.Empty, r.Rating))
                                                            .ToList());

        return Ok(response);
    }
}
