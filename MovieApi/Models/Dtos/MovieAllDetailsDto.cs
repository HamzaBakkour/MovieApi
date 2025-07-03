namespace MovieApi.Models.Dtos;

public record MovieAllDetailsDto(int Id,
                                string Title,
                                int Year,
                                int Duration,
                                MovieDetailesDto? Detailes = null,
                                List<ActorDto>? Actors = null,
                                List<GenreDto>? Genres = null,
                                List<ReviewDto>? Reviews = null);

