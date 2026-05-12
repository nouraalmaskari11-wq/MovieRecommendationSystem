using MovieRecommendationSystem.Console.Models;

namespace MovieRecommendationSystem.Console.Recommendation
{
    public class CollaborativeFiltering
    {
        private readonly List<User> _users;
        private readonly List<Movie> _movies;

        public CollaborativeFiltering(List<User> users, List<Movie> movies)
        {
            _users = users;
            _movies = movies;
        }

        public List<(Movie movie, double score)> GetRecommendations(User currentUser, int topN = 5)
        {
            var similarUsers = new List<(User user, double similarity)>();

            foreach (var user in _users)
            {
                if (user.Id == currentUser.Id) continue;

                double similarity = SimilarityCalculator.CosineSimilarity(
                    currentUser.Ratings, user.Ratings);

                if (similarity > 0)
                {
                    similarUsers.Add((user, similarity));
                }
            }

            similarUsers = similarUsers.OrderByDescending(x => x.similarity).Take(10).ToList();

            if (similarUsers.Count == 0) return new List<(Movie, double)>();

            var scores = new Dictionary<int, double>();
            var weightedCount = new Dictionary<int, double>();

            foreach (var (similarUser, similarity) in similarUsers)
            {
                foreach (var rating in similarUser.Ratings)
                {
                    int movieId = rating.Key;
                    int score = rating.Value;

                    if (currentUser.Ratings.ContainsKey(movieId)) continue;

                    if (!scores.ContainsKey(movieId))
                    {
                        scores[movieId] = 0;
                        weightedCount[movieId] = 0;
                    }

                    scores[movieId] += score * similarity;
                    weightedCount[movieId] += similarity;
                }
            }

            var finalScores = new Dictionary<int, double>();
            foreach (var movieId in scores.Keys)
            {
                if (weightedCount[movieId] > 0)
                {
                    finalScores[movieId] = scores[movieId] / weightedCount[movieId];
                }
            }

            var recommendations = finalScores
                .OrderByDescending(x => x.Value)
                .Take(topN)
                .Select(x => (_movies.First(m => m.MovieId == x.Key), x.Value))
                .ToList();

            return recommendations;
        }
    }
}