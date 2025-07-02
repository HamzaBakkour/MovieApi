using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.Dtos;

public class MovieCreateDto
{
    [Required]
    [StringLength(150)]
    public string Title { get; set; } = null!;

    [Range(1800, 3000)]
    public int Year { get; set; }

    [Range(1, 600)]
    public int Duration { get; set; }
}
