using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.Dtos;

public record MovieDto (int Id, string Title, int Year, int Duration);

