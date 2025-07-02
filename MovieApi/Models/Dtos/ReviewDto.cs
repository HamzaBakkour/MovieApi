namespace MovieApi.Models.Dtos;



public record ReviewDto( string ReviewerName,
                         string Comment,
                         int Rating);