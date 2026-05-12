namespace MovieRecommendationSystem.Console.Models
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        public double AverageRating { get; set; }
        public string Director { get; set; } = string.Empty;
        public List<string> Cast { get; set; } = new();
        public List<string> Tags { get; set; } = new();
        public List<int> Genres { get; set; } = new();
    }
}