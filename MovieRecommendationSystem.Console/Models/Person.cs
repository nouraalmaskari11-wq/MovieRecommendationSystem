// Models/Person.cs
namespace MovieRecommendationSystem.Console.Models
{
	public abstract class Person
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
	}
}