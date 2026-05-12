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
	}
}