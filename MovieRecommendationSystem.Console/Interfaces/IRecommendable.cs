using System.Collections.Generic;

namespace MovieRecommendationSystem.Console.Interfaces
{
	public interface IRecommendable
	{
		List<(Models.Movie movie, double score)> GetRecommendations(Models.User user, int topN = 5);
	}
}