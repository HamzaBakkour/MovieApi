using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MovieApi.Models.Dtos;
using MovieApi.Models.Entities;

namespace MovieApi.Controllers;

[Route("api/movies")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly MovieContext _context;

    public MoviesController(MovieContext context)
    {
        _context = context;
    }

    // GET: api/Movies
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovie([FromQuery] string? genre,
                                                                    [FromQuery] int? year,
                                                                    [FromQuery] string? actor)
    {

        var query = _context.Movies
                            .Include(m => m.Genres)
                            .Include(m => m.Actors)
                            .AsQueryable();


        if (!string.IsNullOrWhiteSpace(genre))
        {
            query = query.Where(m => m.Genres.Any(g => g.Name == genre));
        }

        if (year.HasValue)
        {
            query = query.Where(m => m.Year == year.Value);
        }

        if (!string.IsNullOrWhiteSpace(actor))
        {
            query = query.Where(m => m.Actors.Any(a => a.Name.ToLower() == actor.ToLower()));
        }

        var movies = await query
            .Select(movie => new MovieDto(
                                    movie.Id,
                                    movie.Title,
                                    movie.Year,
                                    movie.Duration
            ))
            .ToListAsync();


        return Ok(movies);

    }

    // GET: api/Movies/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MovieAllDetailsDto>> GetMovie([FromRoute, Range(0, int.MaxValue)] int id,
                                                                    [FromQuery] MovieQueryOptionsDto options)
    {
        IQueryable<Movie> query = _context.Movies.Where(m => m.Id == id);

        if (options.withActors)
            query = query.Include(m => m.Actors);

        if (options.withGenres)
            query = query.Include(m => m.Genres);

        if (options.withReviews)
            query = query.Include(m => m.Reviews);

        if (options.withDetails)
            query = query.Include(m => m.Detailes);

        var movie = await query.FirstOrDefaultAsync();

        if (movie == null)
            return NotFound();

        var response = new MovieAllDetailsDto(
            movie.Id,
            movie.Title,
            movie.Year,
            movie.Duration,
            options.withDetails && movie.Detailes != null ?
                 new MovieDetailesDto(movie.Detailes.Synopsis, movie.Detailes.Language, movie.Detailes.Budget)
                : null,
            options.withActors ?
                 movie.Actors.Select(a => new ActorDto(a.Id, a.Name, a.BirthYear)).ToList()
                : null,
            options.withGenres ?
                 movie.Genres.Select(g => new GenreDto(g.Name)).ToList()
                : null,
            options.withReviews ?
                 movie.Reviews.Select(r => new ReviewDto(r.ReviewerName, r.Comment, r.Rating)).ToList()
                : null
        );

        return Ok(response);

    }



    [HttpGet("{id}/details")]
    public async Task<ActionResult<MovieAllDetailsDto>> GetMovieDetails([FromRoute, Range(0, int.MaxValue)] int id)
    {
        var movie = await _context.Movies
            .Include(m => m.Detailes)
            .Include(m => m.Actors)
            .Include(m => m.Genres)
            .Include(m => m.Reviews)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null)
            return NotFound();

        var response = new MovieAllDetailsDto(
            movie.Id,
            movie.Title,
            movie.Year,
            movie.Duration,
            movie.Detailes != null ?
                 new MovieDetailesDto(movie.Detailes.Synopsis, movie.Detailes.Language, movie.Detailes.Budget)
                : null,
            movie.Actors != null ?
                 movie.Actors.Select(a => new ActorDto(a.Id, a.Name, a.BirthYear)).ToList()
                : null,
            movie.Genres != null ?
                 movie.Genres.Select(g => new GenreDto(g.Name)).ToList()
                : null,
            movie.Reviews != null ?
                 movie.Reviews.Select(r => new ReviewDto(r.ReviewerName, r.Comment, r.Rating)).ToList()
                : null
        );

        return Ok(response);
    }





    // PUT: api/Movies/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMovie([FromRoute, Range(0, int.MaxValue)] int id, [FromBody] MovieUpdateDto dto)
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);

        if (movie is null)
            return NotFound();



        if (!string.IsNullOrWhiteSpace(dto.Title))
            movie.Title = dto.Title;

        if (dto.Year.HasValue)
            movie.Year = dto.Year.Value;

        if (dto.Duration.HasValue)
            movie.Duration = dto.Duration.Value;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MovieExists(id))
                return NotFound();
            else
                throw;
        }

        var response = new MovieDto(movie.Id, movie.Title, movie.Year, movie.Duration);
        return Ok(response);
    }

    // POST: api/Movies
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<MovieDto>> PostMovie([FromBody] MovieCreateDto dto)
    {
        var movie = new Movie
        {
            Title = dto.Title,
            Year = dto.Year,
            Duration = dto.Duration,
        };

        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        var response = new MovieDto(movie.Id, movie.Title, movie.Year, movie.Duration);

        return CreatedAtAction(nameof(GetMovie), new { id = response.Id }, response);
    }

    // DELETE: api/Movies/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie([FromRoute, Range(0, int.MaxValue)] int id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool MovieExists(int id)
    {
        return _context.Movies.Any(e => e.Id == id);
    }
}
