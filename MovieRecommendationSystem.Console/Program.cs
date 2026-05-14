using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MovieRecommendationSystem.Console.Models;
using MovieRecommendationSystem.Console.Services;
using MovieRecommendationSystem.Console.Recommendation;
using MovieRecommendationSystem.Console.UI;

namespace MovieRecommendationSystem.Console
{
    class Program
    {
        private static DataStorageService? _storage;
        private static AuthService? _authService;
        private static MovieService? _movieService;
        private static RatingService? _ratingService;
        private static RecommendationEngine? _recommendationEngine;
        private static User? _currentUser;

        static void Main(string[] args)
        {
            System.Console.Title = "AI Movie Recommendation System";

            _storage = new DataStorageService();
            _authService = new AuthService(_storage);
            _movieService = new MovieService(_storage);
            _ratingService = new RatingService(_storage, _movieService);
            _storage.SeedSampleData();

            Run();
        }

        static void Run()
        {
            while (true)
            {
                if (_currentUser == null)
                {
                    ShowLoginMenu();
                }
                else
                {
                    ShowDashboard();
                }
            }
        }

        static void ShowLoginMenu()
        {
            ConsoleHelper.ShowMainMenu();
            int choice = ConsoleHelper.GetIntInput("Choose an option: ", 1, 3);

            switch (choice)
            {
                case 1:
                    _authService?.Register();
                    break;
                case 2:
                    _currentUser = _authService?.Login();
                    if (_currentUser != null && _movieService != null && _authService != null)
                    {
                        InitializeRecommendationEngine();
                    }
                    break;
                case 3:
                    ConsoleHelper.WriteLineColor("Goodbye!", ConsoleColor.Cyan);
                    Environment.Exit(0);
                    break;
            }
        }

        static void InitializeRecommendationEngine()
        {
            if (_authService == null || _movieService == null) return;
            var allUsers = _authService.GetAllUsers();
            var allMovies = _movieService.GetAllMovies();
            var allGenres = _movieService.GetAllGenres();
            _recommendationEngine = new RecommendationEngine(allUsers, allMovies, allGenres);
        }

        static void ShowDashboard()
        {
            ConsoleHelper.ShowUserMenu();
            int choice = ConsoleHelper.GetIntInput("Choose an option: ", 1, 10);

            switch (choice)
            {
                case 1:
                    BrowseAllMovies();
                    break;
                case 2:
                    SearchMovies();
                    break;
                case 3:
                    RateMovie();
                    break;
                case 4:
                    ShowRecommendations();
                    break;
                case 5:
                    ShowWatchHistory();
                    break;
                case 6:
                    RemoveRating();
                    break;
                case 7:
                    ShowTrendingMovies();
                    break;
                case 8:
                    ShowRecentlyWatched();
                    break;
                case 9:
                    ExportRecommendations();
                    break;
                case 10:
                    Logout();
                    break;
            }
        }

        static void BrowseAllMovies()
        {
            var movies = _movieService?.GetAllMovies() ?? new List<Movie>();
            ConsoleHelper.PrintMovies(movies);
            ConsoleHelper.PressAnyKey();
        }

        static void SearchMovies()
        {
            if (_movieService == null) return;

            ConsoleHelper.ShowSearchMenu();
            int choice = ConsoleHelper.GetIntInput("Choose search type: ", 1, 6);

            List<Movie> results = new List<Movie>();

            switch (choice)
            {
                case 1:
                    string title = ConsoleHelper.GetStringInput("Enter movie title: ");
                    results = _movieService.SearchByTitle(title);
                    break;
                case 2:
                    System.Console.WriteLine("\nAvailable Genres:");
                    foreach (var genre in _movieService.GetAllGenres())
                    {
                        System.Console.WriteLine($"   {genre.GenreId}. {genre.GenreName}");
                    }
                    int genreId = ConsoleHelper.GetIntInput("\nEnter genre number: ", 1, 8);
                    results = _movieService.SearchByGenre(genreId);
                    break;
                case 3:
                    int year = ConsoleHelper.GetIntInput("Enter year: ", 1900, 2026);
                    results = _movieService.SearchByYear(year);
                    break;
                case 4:
                    string director = ConsoleHelper.GetStringInput("Enter director name: ");
                    results = _movieService.SearchByDirector(director);
                    break;
                case 5:
                    double minRating = ConsoleHelper.GetIntInput("Minimum rating (1-5): ", 1, 5);
                    results = _movieService.SearchByRating(minRating);
                    break;
                case 6:
                    return;
            }

            if (results.Any())
            {
                ConsoleHelper.PrintMovies(results, "SEARCH RESULTS");
            }
            else
            {
                ConsoleHelper.WriteLineColor("No movies found!", ConsoleColor.Yellow);
            }
            ConsoleHelper.PressAnyKey();
        }

        static void RateMovie()
        {
            if (_movieService == null || _ratingService == null || _currentUser == null) return;

            var movies = _movieService.GetAllMovies();
            ConsoleHelper.PrintMovies(movies.Take(20).ToList(), " RATE A MOVIE");

            int movieId = ConsoleHelper.GetIntInput(" Enter movie ID to rate: ", 1, movies.Count);
            var movie = _movieService.GetMovieById(movieId);

            if (movie != null)
            {
                _movieService.DisplayMovieDetails(movie);
                int score = ConsoleHelper.GetIntInput("Enter rating (1-5): ", 1, 5);
                _ratingService.RateMovie(_currentUser, movieId, score);
                InitializeRecommendationEngine();
            }
            else
            {
                ConsoleHelper.WriteLineColor("Movie not found!", ConsoleColor.Red);
            }
            ConsoleHelper.PressAnyKey();
        }

        static void ShowRecommendations()
        {
            if (_recommendationEngine == null || _currentUser == null)
            {
                ConsoleHelper.WriteLineColor("  Please rate some movies first to get recommendations!", ConsoleColor.Yellow);
                ConsoleHelper.PressAnyKey();
                return;
            }

            ConsoleHelper.WriteLineColor(" AI Analyzing your preferences...", ConsoleColor.Cyan);
            System.Threading.Thread.Sleep(1000);

            var recommendations = _recommendationEngine.GetRecommendationsWithConfidence(_currentUser, 5);

            if (recommendations.Any())
            {
                ConsoleHelper.PrintHeader("  AI RECOMMENDATIONS WITH CONFIDENCE");
                int rank = 1;
                foreach (var rec in recommendations)
                {
                    var movie = rec.movie;
                    var score = rec.score;
                    var confidence = rec.confidence;

                    System.Console.WriteLine($"{rank++}. {movie.Title} ({movie.ReleaseYear})");
                    System.Console.WriteLine($"   Movie Rating: {movie.AverageRating}/5");
                    System.Console.WriteLine($"   AI Confidence: {confidence:F0}% match");
                    System.Console.WriteLine();
                }
            }
            else
            {
                ConsoleHelper.WriteLineColor(" No recommendations available. Try rating more movies!", ConsoleColor.Yellow);
            }
            ConsoleHelper.PressAnyKey();
        }

        static void ShowWatchHistory()
        {
            if (_ratingService == null || _movieService == null || _currentUser == null) return;
            _ratingService.ShowUserRatingHistory(_currentUser, _movieService);
            ConsoleHelper.PressAnyKey();
        }
        static void RemoveRating()
        {
            if (_movieService == null || _ratingService == null || _currentUser == null) return;

            var movies = _movieService.GetAllMovies();
            ConsoleHelper.PrintMovies(movies.Take(20).ToList(), "REMOVE A RATING");

            int movieId = ConsoleHelper.GetIntInput("Enter movie ID to remove rating: ", 1, movies.Count);
            _ratingService.RemoveRating(_currentUser, movieId);
            InitializeRecommendationEngine();
            ConsoleHelper.PressAnyKey();
        }
        static void ShowTrendingMovies()
        {
            var trending = _movieService?.GetTrendingMovies(5);
            ConsoleHelper.PrintMovies(trending, " TRENDING MOVIES");
            ConsoleHelper.PressAnyKey();
        }

        static void Logout()
        {
            _currentUser = null;
            _recommendationEngine = null;
            ConsoleHelper.WriteLineColor("You have been logged out.", ConsoleColor.Yellow);
            ConsoleHelper.PressAnyKey();
        }
        static void ShowRecentlyWatched()
        {
            if (_ratingService == null || _currentUser == null) return;

            var recent = _ratingService.GetRecentlyRated(_currentUser.Id, 5);

            ConsoleHelper.PrintHeader(" RECENTLY WATCHED");

            if (recent.Count == 0)
            {
                ConsoleHelper.WriteLineColor(" You haven't rated any movies yet!", ConsoleColor.Yellow);
            }
            else
            {
                foreach (var rating in recent)
                {
                    var movie = _movieService?.GetMovieById(rating.MovieId);
                    System.Console.WriteLine($" {movie?.Title} - {rating.Score}/5 on {rating.RatedAt:yyyy-MM-dd HH:mm}");
                }
            }
            ConsoleHelper.PressAnyKey();
        }
        static void ExportRecommendations()
        {
            if (_recommendationEngine == null || _currentUser == null)
            {
                ConsoleHelper.WriteLineColor(" No recommendations available!", ConsoleColor.Yellow);
                ConsoleHelper.PressAnyKey();
                return;
            }

            var recommendations = _recommendationEngine.GetHybridRecommendations(_currentUser, 10);
            string fileName = $"Recommendations_{_currentUser.Username}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("╔════════════════════════════════════════════════════════════╗");
                writer.WriteLine("║           AI MOVIE RECOMMENDATIONS                         ║");
                writer.WriteLine("╚════════════════════════════════════════════════════════════╝");
                writer.WriteLine();
                writer.WriteLine($"User: {_currentUser.Username}");
                writer.WriteLine($"Date: {DateTime.Now}");
                writer.WriteLine($"Total Recommendations: {recommendations.Count}");
                writer.WriteLine(new string('═', 60));
                writer.WriteLine();

                int rank = 1;
                foreach (var (movie, finalScore) in recommendations)
                {
                    writer.WriteLine($"   {rank++}. {movie.Title} ({movie.ReleaseYear})");
                    writer.WriteLine($"    Director: {movie.Director}");
                    writer.WriteLine($"    Rating: {movie.AverageRating}/5");
                    writer.WriteLine($"    Match: {finalScore * 100:F0}%");
                    writer.WriteLine();
                }
            }

            ConsoleHelper.WriteLineColor($" Recommendations saved to: {Path.GetFullPath(fileName)}", ConsoleColor.Green);
            ConsoleHelper.PressAnyKey();
        }
    }
}