using MovieRecommendationSystem.Console.Models;
using MovieRecommendationSystem.Console.Interfaces;

namespace MovieRecommendationSystem.Console.Recommendation
{
    public class RecommendationEngine : IRecommendable
    {
        private readonly ContentBasedFiltering _contentBased;
        private readonly CollaborativeFiltering _collaborative;

        public RecommendationEngine(List<User> users, List<Movie> movies, List<Genre> genres)
        {
            _contentBased = new ContentBasedFiltering(movies, genres);
            _collaborative = new CollaborativeFiltering(users, movies);
        }

        public List<(Movie movie, double finalScore)> GetHybridRecommendations(User user, int topN = 5)
        {
            var contentBased = _contentBased.GetRecommendations(user, topN * 2);
            var collaborative = _collaborative.GetRecommendations(user, topN * 2);

            var combinedScores = new Dictionary<int, double>();

            foreach (var (movie, score) in contentBased)
            {
                combinedScores[movie.MovieId] = score * 0.4;
            }

            foreach (var (movie, score) in collaborative)
            {
                if (combinedScores.ContainsKey(movie.MovieId))
                    combinedScores[movie.MovieId] += score * 0.6;
                else
                    combinedScores[movie.MovieId] = score * 0.6;
            }

            var allMovies = contentBased.Select(x => x.movie)
                .Concat(collaborative.Select(x => x.movie))
                .DistinctBy(m => m.MovieId)
                .ToList();

            var results = allMovies
                .Select(m => (m, combinedScores.GetValueOrDefault(m.MovieId, 0)))
                .Where(x => x.Item2 > 0)
                .OrderByDescending(x => x.Item2)
                .Take(topN)
                .ToList();

            return results;
        }
    }
}