namespace MovieApi.Models.Entities;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;    
    public int Year { get; set; }         
    public int Duration { get; set; }

    public MovieDetailes Detailes { get; set; } = null!;

    public ICollection<Review> Reviews { get; set; } = new List<Review>();

    public ICollection<Actor> Actors { get; set; } = new List<Actor>();

    public ICollection<Genre> Genres { get; set; } = new List<Genre>();

}
