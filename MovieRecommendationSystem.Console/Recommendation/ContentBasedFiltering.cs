using MovieRecommendationSystem.Console.Models;

namespace MovieRecommendationSystem.Console.Recommendation
{
    public class ContentBasedFiltering
    {
        private readonly List<Movie> _movies;
        private readonly List<Genre> _genres;

        public ContentBasedFiltering(List<Movie> movies, List<Genre> genres)
        {
            _movies = movies;
            _genres = genres;
        }

        public List<(Movie movie, double score)> GetRecommendations(User user, int topN = 5)
        {
            var scores = new Dictionary<int, double>();

            var ratedMovieIds = user.Ratings.Keys.ToHashSet();

            foreach (var movie in _movies)
            {
                if (ratedMovieIds.Contains(movie.MovieId)) continue;

                double score = 0;

                // 1. نوع الفيلم (50% وزن)
                double genreMatch = SimilarityCalculator.GenreSimilarity(user.FavoriteGenres, movie.Genres);
                score += genreMatch * 0.5;

                // 2. تشابه المخرج (20% وزن)
                double directorMatch = 0;
                var userWatchedMovies = _movies.Where(m => user.WatchHistory.Contains(m.MovieId));
                if (userWatchedMovies.Any(m => m.Director == movie.Director))
                    directorMatch = 0.8;
                score += directorMatch * 0.2;

                // 3. شعبية التقييم (30% وزن)
                double popularity = movie.AverageRating / 5.0;
                score += popularity * 0.3;

                scores[movie.MovieId] = score;
            }

            var recommendations = scores
                .OrderByDescending(x => x.Value)
                .Take(topN)
                .Select(x => (_movies.First(m => m.MovieId == x.Key), x.Value))
                .ToList();

            return recommendations;
        }
    }
}