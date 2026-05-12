using System.Collections.Generic;

namespace MovieRecommendationSystem.Console.Interfaces
{
	public interface ISearchable<T>
	{
		List<T> SearchByTitle(string title);
		List<T> SearchByGenre(int genreId);
		List<T> SearchByYear(int year);
		List<T> SearchByDirector(string director);
		List<T> SearchByRating(double minRating);
	}
}