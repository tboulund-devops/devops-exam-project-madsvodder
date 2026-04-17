using Backend.Models;

namespace Backend;

public class Movie
{
    public string Title { get; set; } = String.Empty;
    public int Year { get; set; }
    public int Id { get; set; }
    public string Description { get; set; } = String.Empty;
    
    public string PosterUrl { get; set; } = String.Empty;
    
    //Navigation property
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}