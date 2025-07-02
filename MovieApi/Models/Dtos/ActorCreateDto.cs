using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.Dtos;

public class ActorCreateDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Range(1900, 3000)]
    public int BirthYear { get; set; }
}
