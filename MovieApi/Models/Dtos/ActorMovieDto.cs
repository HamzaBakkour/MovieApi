using MovieApi.Models.Entities;
namespace MovieApi.Models.Dtos;


public record ActorMovieDto(int MovieId,
                                string Title,
                                int Year,
                                List<ActorDto> Actors);