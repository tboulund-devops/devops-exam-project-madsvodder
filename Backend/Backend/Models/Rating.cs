namespace Backend.Models;

public class Rating
{
    public int Id { get; set; }
    public int Score { get; set; } //1-10 ?? maybe 1-5??
    public string? Comment { get; set; } //Voluntary comment
    
    //FK to Movie
    public int MovieId { get; set; }
    public Movie Movie { get; set; }
}