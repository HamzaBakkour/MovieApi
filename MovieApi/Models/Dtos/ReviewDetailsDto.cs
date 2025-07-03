namespace MovieApi.Models.Dtos;

public record ReviewDetailsDto(int Id,
                        string ReviewerName,
                        string Comment,
                        int Rating,
                        MovieDto Movie);
