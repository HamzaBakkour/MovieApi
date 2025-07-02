using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Models.Dtos;
using MovieApi.Models.Entities;

namespace MovieApi.Controllers;

[Route("api/actors")]
[ApiController]
public class ActorsController : ControllerBase
{

    private readonly MovieContext _context;

    public ActorsController(MovieContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActorDto>>> GetActors()
    {
        var actors = await _context.Actors
            .Select(a => new ActorDto(a.Id, a.Name, a.BirthYear))
            .ToListAsync();

        return Ok(actors);
    }

    [HttpPost]
    public async Task<ActionResult<Actor>> PostActor(ActorCreateDto dto)
    {
        var actor = new Actor
        {
            Name = dto.Name,
            BirthYear = dto.BirthYear,
        };

        _context.Actors.Add(actor);
        await _context.SaveChangesAsync();

        var response = new ActorDto(actor.Id, actor.Name, actor.BirthYear);

        return Ok(response);

    }

    [HttpPost("/api/movies/{movieId}/actors/{actorId}")]
    public async Task<IActionResult> AddActorToMovie(int movieId, int actorId)
    {
        var movie = await _context.Movies
            .Include(m => m.Actors)
            .FirstOrDefaultAsync(m => m.Id == movieId);

        if (movie == null)
            return NotFound($"Movie with ID {movieId} not found.");

        var actor = await _context.Actors.FindAsync(actorId);
        if (actor == null)
            return NotFound($"Actor with ID {actorId} not found.");

        if (movie.Actors.Any(a => a.Id == actorId))
            return BadRequest("Actor is already assigned to this movie.");

        movie.Actors.Add(actor);
        await _context.SaveChangesAsync();

        var response = new ActorMovieDto(movie.Id,
                                            movie.Title,
                                            movie.Year,
                                            movie.Actors
                                            .Select(a => new ActorDto(a.Id, a.Name, a.BirthYear))
                                            .ToList());
        /// ToDo: Change the method from GetMovie to the method
        /// that return Movie detailes
        return CreatedAtAction(
            nameof(MoviesController.GetMovie), 
            "Movies",                          
            new { id = movie.Id },            
            response                          
        );
    }

}