namespace MovieApi.Models.Dtos;

public record ReviewDetailDto(int Id,
                        string ReviewerName,
                        string Comment,
                        int Rating,
                        MovieDto Movie);
