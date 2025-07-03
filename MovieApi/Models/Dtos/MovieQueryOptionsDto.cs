using MovieApi.Models.Entities;

namespace MovieApi.Models.Dtos;

public class MovieQueryOptionsDto
{
    public bool withActors { get; set; } = false;
    public bool withGenres { get; set; } = false;
    public bool withReviews { get; set; } = false;
    public bool withDetails { get; set; } = false;
}
