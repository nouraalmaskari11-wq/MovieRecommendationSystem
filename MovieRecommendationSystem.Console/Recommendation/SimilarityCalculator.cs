namespace MovieRecommendationSystem.Console.Recommendation
{
	public static class SimilarityCalculator
	{
		// حساب التشابه بين مستخدمين (Collaborative Filtering)
		public static double CosineSimilarity(Dictionary<int, int> vectorA, Dictionary<int, int> vectorB)
		{
			var commonKeys = vectorA.Keys.Intersect(vectorB.Keys).ToList();

			if (commonKeys.Count == 0) return 0;

			double dotProduct = 0;
			double magnitudeA = 0;
			double magnitudeB = 0;

			foreach (var key in commonKeys)
			{
				dotProduct += vectorA[key] * vectorB[key];
			}

			foreach (var value in vectorA.Values)
			{
				magnitudeA += Math.Pow(value, 2);
			}

			foreach (var value in vectorB.Values)
			{
				magnitudeB += Math.Pow(value, 2);
			}

			if (magnitudeA == 0 || magnitudeB == 0) return 0;

			return dotProduct / (Math.Sqrt(magnitudeA) * Math.Sqrt(magnitudeB));
		}

		// حساب تشابه الأنواع بين تفضيلات المستخدم والفيلم
		public static double GenreSimilarity(List<int> userGenres, List<int> movieGenres)
		{
			if (userGenres.Count == 0 || movieGenres.Count == 0) return 0;

			var common = userGenres.Intersect(movieGenres).Count();
			var total = userGenres.Union(movieGenres).Count();

			return total == 0 ? 0 : (double)common / total;
		}
	}
}