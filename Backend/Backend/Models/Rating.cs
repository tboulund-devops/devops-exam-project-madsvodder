using System.Text.Json.Serialization;

namespace Backend.Models;

public class Rating
{
    public int Id { get; set; }
    public int Score { get; set; } //1-10
    public string? Comment { get; set; } //Voluntary comment
    
    //FK to Movie
    public int MovieId { get; set; }
    
    [JsonIgnore]
    public Movie? Movie { get; set; }
}