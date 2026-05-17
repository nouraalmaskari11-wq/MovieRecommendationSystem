using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
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
            System.Console.Title = "CINEMA - Movie Recommendation System";
            System.Console.WindowWidth = 120;
            System.Console.WindowHeight = 50;

            ConsoleHelper.ShowSplash();

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
            int choice = ConsoleHelper.GetIntInput("\nEnter your choice: ", 1, 3);

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
                    ConsoleHelper.WriteLine("\n[ GOODBYE ] Thanks for visiting the cinema!");
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
            ConsoleHelper.ShowDashboard();
            int choice = ConsoleHelper.GetIntInput("\nEnter your choice: ", 1, 10);

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
            int choice = ConsoleHelper.GetIntInput("\nEnter your choice: ", 1, 6);

            List<Movie> results = new List<Movie>();

            switch (choice)
            {
                case 1:
                    string title = ConsoleHelper.GetStringInput("\nEnter movie title: ");
                    results = _movieService.SearchByTitle(title);
                    break;
                case 2:
                    ConsoleHelper.WriteLine("\nAvailable Genres:");
                    foreach (var genre in _movieService.GetAllGenres())
                    {
                        ConsoleHelper.WriteLine($"   {genre.GenreId}. {genre.GenreName}");
                    }
                    int genreId = ConsoleHelper.GetIntInput("\nEnter genre number: ", 1, 8);
                    results = _movieService.SearchByGenre(genreId);
                    break;
                case 3:
                    int year = ConsoleHelper.GetIntInput("\nEnter year: ", 1900, 2026);
                    results = _movieService.SearchByYear(year);
                    break;
                case 4:
                    string director = ConsoleHelper.GetStringInput("\nEnter director name: ");
                    results = _movieService.SearchByDirector(director);
                    break;
                case 5:
                    double minRating = ConsoleHelper.GetIntInput("\nMinimum rating (1-5): ", 1, 5);
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
                ConsoleHelper.ShowWarning("No movies found!");
            }
            ConsoleHelper.PressAnyKey();
        }

        static void RateMovie()
        {
            if (_movieService == null || _ratingService == null || _currentUser == null) return;

            var movies = _movieService.GetAllMovies();
            ConsoleHelper.PrintMovies(movies.Take(20).ToList(), "RATE A MOVIE");

            int movieId = ConsoleHelper.GetIntInput("\nEnter movie ID to rate: ", 1, movies.Count);
            var movie = _movieService.GetMovieById(movieId);

            if (movie != null)
            {
                ConsoleHelper.PrintMovieDetails(movie);
                int score = ConsoleHelper.GetIntInput("\nEnter rating (1-5): ", 1, 5);
                _ratingService.RateMovie(_currentUser, movieId, score);
                ConsoleHelper.ShowSuccess($"You rated '{movie.Title}' {score}/5!");
                InitializeRecommendationEngine();
            }
            else
            {
                ConsoleHelper.ShowError("Movie not found!");
            }
            ConsoleHelper.PressAnyKey();
        }

        static void RemoveRating()
        {
            if (_movieService == null || _ratingService == null || _currentUser == null) return;

            var movies = _movieService.GetAllMovies();
            ConsoleHelper.PrintMovies(movies.Take(20).ToList(), "REMOVE A RATING");

            int movieId = ConsoleHelper.GetIntInput("\nEnter movie ID to remove rating: ", 1, movies.Count);
            _ratingService.RemoveRating(_currentUser, movieId);
            ConsoleHelper.ShowSuccess("Rating removed successfully!");
            InitializeRecommendationEngine();
            ConsoleHelper.PressAnyKey();
        }

        static void ShowRecommendations()
        {
            if (_recommendationEngine == null || _currentUser == null)
            {
                ConsoleHelper.ShowWarning("Please rate some movies first to get recommendations!");
                ConsoleHelper.PressAnyKey();
                return;
            }

            ConsoleHelper.ShowInfo("AI is analyzing your preferences...");
            Thread.Sleep(1500);

            var recommendations = _recommendationEngine.GetRecommendationsWithConfidence(_currentUser, 5);

            if (recommendations.Any())
            {
                ConsoleHelper.PrintRecommendations(recommendations);
            }
            else
            {
                ConsoleHelper.ShowWarning("No recommendations available. Try rating more movies!");
            }
            ConsoleHelper.PressAnyKey();
        }

        static void ShowWatchHistory()
        {
            if (_ratingService == null || _movieService == null || _currentUser == null) return;
            _ratingService.ShowUserRatingHistory(_currentUser, _movieService);
            ConsoleHelper.PressAnyKey();
        }

        static void ShowTrendingMovies()
        {
            var trending = _movieService?.GetTrendingMovies(5);
            ConsoleHelper.PrintMovies(trending, "TRENDING MOVIES");
            ConsoleHelper.PressAnyKey();
        }

        static void ShowRecentlyWatched()
        {
            if (_ratingService == null || _currentUser == null) return;

            var recent = _ratingService.GetRecentlyRated(_currentUser.Id, 5);

            ConsoleHelper.PrintHeader("RECENTLY WATCHED");

            if (recent.Count == 0)
            {
                ConsoleHelper.ShowWarning("You haven't rated any movies yet!");
            }
            else
            {
                foreach (var rating in recent)
                {
                    var movie = _movieService?.GetMovieById(rating.MovieId);
                    ConsoleHelper.WriteLine($"  {movie?.Title} - Rating: {rating.Score}/5 on {rating.RatedAt:yyyy-MM-dd HH:mm}");
                }
            }
            ConsoleHelper.PressAnyKey();
        }

        static void ExportRecommendations()
        {
            if (_recommendationEngine == null || _currentUser == null)
            {
                ConsoleHelper.ShowWarning("No recommendations available!");
                ConsoleHelper.PressAnyKey();
                return;
            }

            var recommendations = _recommendationEngine.GetHybridRecommendations(_currentUser, 10);
            string fileName = $"Recommendations_{_currentUser.Username}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("CINEMA - AI RECOMMENDATIONS");
                writer.WriteLine($"User: {_currentUser.Username}");
                writer.WriteLine($"Date: {DateTime.Now}");
                writer.WriteLine(new string('-', 60));
                writer.WriteLine();

                int rank = 1;
                foreach (var (movie, finalScore) in recommendations)
                {
                    writer.WriteLine($"{rank++}. {movie.Title} ({movie.ReleaseYear})");
                    writer.WriteLine($"   Director: {movie.Director}");
                    writer.WriteLine($"   Rating: {movie.AverageRating}/5");
                    writer.WriteLine($"   AI Match: {finalScore * 100:F0}%");
                    writer.WriteLine();
                }
            }

            ConsoleHelper.ShowSuccess($"Recommendations saved to: {fileName}");
            ConsoleHelper.PressAnyKey();
        }

        static void Logout()
        {
            _currentUser = null;
            _recommendationEngine = null;
            ConsoleHelper.ShowSuccess("You have been logged out.");
            ConsoleHelper.PressAnyKey();
        }
    }
}