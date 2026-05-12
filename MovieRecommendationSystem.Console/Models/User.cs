namespace MovieRecommendationSystem.Console.Models
{
	public class User : Person
	{
		public string Username { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public bool IsAdmin { get; set; } = false;
		public string AdminLevel { get; set; } = string.Empty;
		public DateTime LastLogin { get; set; }
		public DateTime RegisteredAt { get; set; } = DateTime.Now;
		public List<int> FavoriteGenres { get; set; } = new();
		public List<int> WatchHistory { get; set; } = new();
		public Dictionary<int, int> Ratings { get; set; } = new();
	}
}