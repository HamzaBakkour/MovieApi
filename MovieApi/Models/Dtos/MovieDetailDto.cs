using MovieApi.Models.Entities;

namespace MovieApi.Models.Dtos;

public record MovieDetailDto( int Id,
                                string Title,
                                int Year,
                                int Duration,
                                MovieDetailes Detailes,
                                Review Reviews,
                                Actor Actors,
                                Genre Genres);
