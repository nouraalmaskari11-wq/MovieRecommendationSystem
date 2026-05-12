using System;
using MovieRecommendationSystem.Console.Interfaces;
using System.Collections.Generic;
using System.Linq;
using MovieRecommendationSystem.Console.Models;
using MovieRecommendationSystem.Console.UI;

namespace MovieRecommendationSystem.Console.Services
{
    public class MovieService : ISearchable<Movie>
    {
        private readonly DataStorageService _storage;
        private List<Movie> _movies;
        private List<Genre> _genres;

        public MovieService(DataStorageService storage)
        {
            _storage = storage;
            _movies = _storage.LoadMovies();
            _genres = _storage.LoadGenres();

            if (_movies.Count == 0)
            {
                LoadSampleData();
            }
        }

        private void LoadSampleData()
        {
            _genres = new List<Genre>
            {
                new Genre { GenreId = 1, GenreName = "Action" },
                new Genre { GenreId = 2, GenreName = "Comedy" },
                new Genre { GenreId = 3, GenreName = "Sci-Fi" },
                new Genre { GenreId = 4, GenreName = "Drama" },
                new Genre { GenreId = 5, GenreName = "Thriller" },
                new Genre { GenreId = 6, GenreName = "Romance" },
                new Genre { GenreId = 7, GenreName = "Horror" },
                new Genre { GenreId = 8, GenreName = "Adventure" }
            };
            _storage.SaveGenres(_genres);

            _movies = new List<Movie>();

            var sampleMovies = new[]
            {
                new Movie { MovieId = 1, Title = "Inception", ReleaseYear = 2010, Director = "Christopher Nolan", AverageRating = 4.8,
                    Genres = new List<int> { 1, 3, 5 }, Tags = new List<string> { "dream", "heist" },
                    Cast = new List<string> { "Leonardo DiCaprio", "Joseph Gordon-Levitt" },
                    Description = "A thief who steals corporate secrets through dream-sharing technology." },

                new Movie { MovieId = 2, Title = "The Dark Knight", ReleaseYear = 2008, Director = "Christopher Nolan", AverageRating = 4.9,
                    Genres = new List<int> { 1, 5 }, Tags = new List<string> { "superhero", "dark" },
                    Cast = new List<string> { "Christian Bale", "Heath Ledger" },
                    Description = "Batman faces the Joker, a criminal mastermind." },

                new Movie { MovieId = 3, Title = "Interstellar", ReleaseYear = 2014, Director = "Christopher Nolan", AverageRating = 4.7,
                    Genres = new List<int> { 3, 4 }, Tags = new List<string> { "space", "time travel" },
                    Cast = new List<string> { "Matthew McConaughey", "Anne Hathaway" },
                    Description = "A team of explorers travel through a wormhole in space." }
            };

            _movies.AddRange(sampleMovies);

            for (int i = 4; i <= 50; i++)
            {
                _movies.Add(new Movie
                {
                    MovieId = i,
                    Title = $"Sample Movie {i}",
                    ReleaseYear = 2000 + (i % 25),
                    Director = $"Director {(i % 10) + 1}",
                    AverageRating = 3 + (i % 20) / 10.0,
                    Genres = new List<int> { (i % 8) + 1 },
                    Tags = new List<string> { "sample" },
                    Cast = new List<string> { "Actor 1", "Actor 2" },
                    Description = "This is a sample movie for demonstration purposes."
                });
            }

            _storage.SaveMovies(_movies);
        }

        public List<Movie> GetAllMovies() => _movies;
        public Movie GetMovieById(int id) => _movies.FirstOrDefault(m => m.MovieId == id);
        public List<Genre> GetAllGenres() => _genres;
        public string GetGenreName(int genreId) => _genres.FirstOrDefault(g => g.GenreId == genreId)?.GenreName ?? "Unknown";

        public List<Movie> SearchByTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return _movies;
            title = title.ToLower();
            return _movies.Where(m => m.Title.ToLower().Contains(title)).ToList();
        }

        public List<Movie> SearchByGenre(int genreId) => _movies.Where(m => m.Genres.Contains(genreId)).ToList();
        public List<Movie> SearchByYear(int year) => _movies.Where(m => m.ReleaseYear == year).ToList();
        public List<Movie> SearchByDirector(string director) => _movies.Where(m => m.Director.ToLower().Contains(director.ToLower())).ToList();
        public List<Movie> SearchByRating(double minRating) => _movies.Where(m => m.AverageRating >= minRating).ToList();

        public void UpdateMovieRating(int movieId, double newAverage)
        {
            var movie = GetMovieById(movieId);
            if (movie != null)
            {
                movie.AverageRating = newAverage;
                _storage.SaveMovies(_movies);
            }
        }

        public void DisplayMovieDetails(Movie movie)
        {
            System.Console.WriteLine($"\n📽️  {movie.Title} ({movie.ReleaseYear})");
            System.Console.WriteLine($"   🎬 Director: {movie.Director}");
            System.Console.WriteLine($"   ⭐ Rating: {movie.AverageRating:F1}/5");
            System.Console.WriteLine($"   📝 Description: {movie.Description}");
            System.Console.WriteLine($"   🏷️  Genres: {string.Join(", ", movie.Genres.Select(g => GetGenreName(g)))}");
            System.Console.WriteLine();
        }
    }
}