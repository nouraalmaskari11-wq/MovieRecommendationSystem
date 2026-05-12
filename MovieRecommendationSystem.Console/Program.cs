using System;
using System.Collections.Generic;
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
            System.Console.Title = "🎬 AI Movie Recommendation System";

            _storage = new DataStorageService();
            _authService = new AuthService(_storage);
            _movieService = new MovieService(_storage);
            _ratingService = new RatingService(_storage, _movieService);

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
            int choice = ConsoleHelper.GetIntInput("👉 Choose an option: ", 1, 3);

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
                    ConsoleHelper.WriteLineColor("👋 Goodbye!", ConsoleColor.Cyan);
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
            int choice = ConsoleHelper.GetIntInput("👉 Choose an option: ", 1, 6);

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
            int choice = ConsoleHelper.GetIntInput("👉 Choose search type: ", 1, 6);

            List<Movie> results = new List<Movie>();

            switch (choice)
            {
                case 1:
                    string title = ConsoleHelper.GetStringInput("🔍 Enter movie title: ");
                    results = _movieService.SearchByTitle(title);
                    break;
                case 2:
                    System.Console.WriteLine("\n📂 Available Genres:");
                    foreach (var genre in _movieService.GetAllGenres())
                    {
                        System.Console.WriteLine($"   {genre.GenreId}. {genre.GenreName}");
                    }
                    int genreId = ConsoleHelper.GetIntInput("\n👉 Enter genre number: ", 1, 8);
                    results = _movieService.SearchByGenre(genreId);
                    break;
                case 3:
                    int year = ConsoleHelper.GetIntInput("📅 Enter year: ", 1900, 2026);
                    results = _movieService.SearchByYear(year);
                    break;
                case 4:
                    string director = ConsoleHelper.GetStringInput("🎬 Enter director name: ");
                    results = _movieService.SearchByDirector(director);
                    break;
                case 5:
                    double minRating = ConsoleHelper.GetIntInput("⭐ Minimum rating (1-5): ", 1, 5);
                    results = _movieService.SearchByRating(minRating);
                    break;
                case 6:
                    return;
            }

            if (results.Any())
            {
                ConsoleHelper.PrintMovies(results, "🔍 SEARCH RESULTS");
            }
            else
            {
                ConsoleHelper.WriteLineColor("❌ No movies found!", ConsoleColor.Yellow);
            }
            ConsoleHelper.PressAnyKey();
        }

        static void RateMovie()
        {
            if (_movieService == null || _ratingService == null || _currentUser == null) return;

            var movies = _movieService.GetAllMovies();
            ConsoleHelper.PrintMovies(movies.Take(20).ToList(), "⭐ RATE A MOVIE");

            int movieId = ConsoleHelper.GetIntInput("🎬 Enter movie ID to rate: ", 1, movies.Count);
            var movie = _movieService.GetMovieById(movieId);

            if (movie != null)
            {
                _movieService.DisplayMovieDetails(movie);
                int score = ConsoleHelper.GetIntInput("⭐ Enter rating (1-5): ", 1, 5);
                _ratingService.RateMovie(_currentUser, movieId, score);
                InitializeRecommendationEngine();
            }
            else
            {
                ConsoleHelper.WriteLineColor("❌ Movie not found!", ConsoleColor.Red);
            }
            ConsoleHelper.PressAnyKey();
        }

        static void ShowRecommendations()
        {
            if (_recommendationEngine == null || _currentUser == null)
            {
                ConsoleHelper.WriteLineColor("⚠️ Please rate some movies first to get recommendations!", ConsoleColor.Yellow);
                ConsoleHelper.PressAnyKey();
                return;
            }

            ConsoleHelper.WriteLineColor("🤖 Analyzing your preferences...", ConsoleColor.Cyan);
            System.Threading.Thread.Sleep(1000);

            var recommendations = _recommendationEngine.GetHybridRecommendations(_currentUser, 5);

            if (recommendations.Any())
            {
                ConsoleHelper.PrintRecommendations(recommendations);
            }
            else
            {
                ConsoleHelper.WriteLineColor("❌ No recommendations available. Try rating more movies!", ConsoleColor.Yellow);
            }
            ConsoleHelper.PressAnyKey();
        }

        static void ShowWatchHistory()
        {
            if (_ratingService == null || _movieService == null || _currentUser == null) return;
            _ratingService.ShowUserRatingHistory(_currentUser, _movieService);
            ConsoleHelper.PressAnyKey();
        }

        static void Logout()
        {
            _currentUser = null;
            _recommendationEngine = null;
            ConsoleHelper.WriteLineColor("✅ You have been logged out.", ConsoleColor.Yellow);
            ConsoleHelper.PressAnyKey();
        }
    }
}