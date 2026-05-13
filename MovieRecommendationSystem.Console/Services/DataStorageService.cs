using Newtonsoft.Json;
using MovieRecommendationSystem.Console.Models;
using System.Text;

namespace MovieRecommendationSystem.Console.Services
{
	public class DataStorageService
	{
		private readonly string _dataPath;

		public DataStorageService()
		{
			_dataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data");
			if (!Directory.Exists(_dataPath))
				Directory.CreateDirectory(_dataPath);
		}

		// حفظ المستخدمين
		public void SaveUsers(List<User> users)
		{
			string json = JsonConvert.SerializeObject(users, Formatting.Indented);
			File.WriteAllText(Path.Combine(_dataPath, "users.json"), json);
		}

		// قراءة المستخدمين
		public List<User> LoadUsers()
		{
			string filePath = Path.Combine(_dataPath, "users.json");
			if (!File.Exists(filePath)) return new List<User>();

			string json = File.ReadAllText(filePath);
			return JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
		}

		// حفظ الأفلام
		public void SaveMovies(List<Movie> movies)
		{
			string json = JsonConvert.SerializeObject(movies, Formatting.Indented);
			File.WriteAllText(Path.Combine(_dataPath, "movies.json"), json);
		}

		// قراءة الأفلام
		public List<Movie> LoadMovies()
		{
			string filePath = Path.Combine(_dataPath, "movies.json");
			if (!File.Exists(filePath)) return new List<Movie>();

			string json = File.ReadAllText(filePath);
			return JsonConvert.DeserializeObject<List<Movie>>(json) ?? new List<Movie>();
		}

		// حفظ التقييمات
		public void SaveRatings(List<Rating> ratings)
		{
			string json = JsonConvert.SerializeObject(ratings, Formatting.Indented);
			File.WriteAllText(Path.Combine(_dataPath, "ratings.json"), json);
		}

		// قراءة التقييمات
		public List<Rating> LoadRatings()
		{
			string filePath = Path.Combine(_dataPath, "ratings.json");
			if (!File.Exists(filePath)) return new List<Rating>();

			string json = File.ReadAllText(filePath);
			return JsonConvert.DeserializeObject<List<Rating>>(json) ?? new List<Rating>();
		}

		// حفظ الأنواع
		public void SaveGenres(List<Genre> genres)
		{
			string json = JsonConvert.SerializeObject(genres, Formatting.Indented);
			File.WriteAllText(Path.Combine(_dataPath, "genres.json"), json);
		}

		// قراءة الأنواع
		public List<Genre> LoadGenres()
		{
			string filePath = Path.Combine(_dataPath, "genres.json");
			if (!File.Exists(filePath)) return new List<Genre>();

			string json = File.ReadAllText(filePath);
			return JsonConvert.DeserializeObject<List<Genre>>(json) ?? new List<Genre>();
		}
        public void SeedSampleData()
        {
            // نجيب البيانات اللي موجودة
            var users = LoadUsers();
            var movies = LoadMovies();
            var ratings = LoadRatings();
            var random = new Random();

            // 1. إذا كان عدد المستخدمين أقل من 10، نضيف 9 مستخدمين جدد
            if (users.Count < 10)
            {
                for (int i = users.Count + 1; i <= 10; i++)
                {
                    var newUser = new User
                    {
                        Id = i,
                        Username = $"user{i}",
                        Password = "pass123",
                        Name = $"User {i}",
                        IsAdmin = false,
                        RegisteredAt = DateTime.Now,
                        FavoriteGenres = new List<int> { random.Next(1, 9) },
                        WatchHistory = new List<int>(),
                        Ratings = new Dictionary<int, int>()
                    };
                    users.Add(newUser);
                }
                SaveUsers(users);
                System.Console.WriteLine("Added 9 new users!");
            }

            // 2. إذا كان عدد التقييمات أقل من 100، نضيف 100 تقييم
            if (ratings.Count < 100)
            {
                int nextId = ratings.Count > 0 ? ratings.Max(r => r.RatingId) + 1 : 1;

                for (int i = 0; i < 100; i++)
                {
                    int userId = random.Next(1, users.Count + 1);
                    int movieId = random.Next(1, movies.Count + 1);
                    int score = random.Next(1, 6);

                    // نتأكد إنه ما في تقييم مكرر
                    bool exists = ratings.Any(r => r.UserId == userId && r.MovieId == movieId);
                    if (!exists)
                    {
                        ratings.Add(new Rating
                        {
                            RatingId = nextId++,
                            UserId = userId,
                            MovieId = movieId,
                            Score = score,
                            RatedAt = DateTime.Now.AddDays(-random.Next(1, 30))
                        });
                    }
                }
                SaveRatings(ratings);
                System.Console.WriteLine("Added 100 sample ratings!");
            }

            // 3. نحدث تقييمات كل مستخدم
            foreach (var user in users)
            {
                var userRatings = ratings.Where(r => r.UserId == user.Id).ToList();
                foreach (var rating in userRatings)
                {
                    if (!user.Ratings.ContainsKey(rating.MovieId))
                    {
                        user.Ratings.Add(rating.MovieId, rating.Score);
                    }
                }
            }
            SaveUsers(users);
        }
    }
}