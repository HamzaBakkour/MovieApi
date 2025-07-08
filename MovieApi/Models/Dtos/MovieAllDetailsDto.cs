namespace MovieApi.Models.Dtos;

public class MovieAllDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int Year { get; set; }
    public int Duration { get; set; }

    public MovieDetailesDto? Detailes { get; set; }

    public List<ActorDto>? Actors { get; set; } = null;
    public List<GenreDto>? Genres { get; set; } = null;
    public List<ReviewDto>? Reviews { get; set; } = null;
}

