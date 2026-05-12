namespace MovieRecommendationSystem.Console.Models
{
    public class Rating
    {
        public int RatingId { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public int Score { get; set; }
        public DateTime RatedAt { get; set; } = DateTime.Now;
    }
}