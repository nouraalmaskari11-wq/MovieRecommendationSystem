using System.Collections.Generic;

namespace MovieRecommendationSystem.Console.Interfaces
{
	public interface IDataStorage<T>
	{
		void Save(List<T> items);
		List<T> Load();
	}
}