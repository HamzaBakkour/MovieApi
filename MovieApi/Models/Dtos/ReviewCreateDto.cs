using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.Dtos;

public class ReviewCreateDto
{
    [Required]
    [StringLength(100)]
    public string ReviewerName { get; set; } = null!;
    
    [StringLength(200)]
    public string? Comment { get; set; }

    [Required]
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
    public int Rating { get; set; }

}
