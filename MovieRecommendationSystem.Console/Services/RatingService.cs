using System;
using System.Collections.Generic;
using System.Linq;
using MovieRecommendationSystem.Console.Models;
using MovieRecommendationSystem.Console.UI;

namespace MovieRecommendationSystem.Console.Services
{
    public class RatingService
    {
        private readonly DataStorageService _storage;
        private readonly MovieService _movieService;
        private List<Rating> _ratings;

        public RatingService(DataStorageService storage, MovieService movieService)
        {
            _storage = storage;
            _movieService = movieService;
            _ratings = _storage.LoadRatings();
        }

        public void RateMovie(User user, int movieId, int score)
        {
            var movie = _movieService.GetMovieById(movieId);
            if (movie == null) return;

            var existingRating = _ratings.FirstOrDefault(r => r.UserId == user.Id && r.MovieId == movieId);

            if (existingRating != null)
            {
                existingRating.Score = score;
                existingRating.RatedAt = DateTime.Now;
            }
            else
            {
                int newId = _ratings.Count > 0 ? _ratings.Max(r => r.RatingId) + 1 : 1;
                _ratings.Add(new Rating
                {
                    RatingId = newId,
                    UserId = user.Id,
                    MovieId = movieId,
                    Score = score,
                    RatedAt = DateTime.Now
                });
            }

            if (user.Ratings.ContainsKey(movieId))
                user.Ratings[movieId] = score;
            else
                user.Ratings.Add(movieId, score);

            _storage.SaveRatings(_ratings);
            UpdateMovieAverageRating(movieId);
            ConsoleHelper.WriteLineColor($"✅ You rated '{movie.Title}' {score}/5!", ConsoleColor.Green);
        }

        private void UpdateMovieAverageRating(int movieId)
        {
            var movieRatings = _ratings.Where(r => r.MovieId == movieId).ToList();
            if (movieRatings.Any())
            {
                double newAverage = movieRatings.Average(r => r.Score);
                _movieService.UpdateMovieRating(movieId, newAverage);
            }
        }

        public void ShowUserRatingHistory(User user, MovieService movieService)
        {
            var userRatings = _ratings.Where(r => r.UserId == user.Id).ToList();
            if (!userRatings.Any())
            {
                ConsoleHelper.WriteLineColor("❌ You haven't rated any movies yet!", ConsoleColor.Yellow);
                return;
            }

            ConsoleHelper.PrintHeader("⭐ MY RATINGS");
            foreach (var rating in userRatings)
            {
                var movie = movieService.GetMovieById(rating.MovieId);
                System.Console.WriteLine($"🎬 {movie?.Title}: {rating.Score}/5 ⭐");
            }
            System.Console.WriteLine();
        }
    }
}