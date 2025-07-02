namespace MovieApi.Models.Dtos;

public record ReviewMovieDto(MovieDto Movie,
                                List<ReviewDto> Reviews);
