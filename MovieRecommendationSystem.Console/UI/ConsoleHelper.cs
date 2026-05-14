using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieRecommendationSystem.Console.UI
{
    public static class ConsoleHelper
    {
        public static void WriteColor(string message, ConsoleColor color)
        {
            System.Console.ForegroundColor = color;
            System.Console.Write(message);
            System.Console.ResetColor();
        }

        public static void WriteLineColor(string message, ConsoleColor color)
        {
            System.Console.ForegroundColor = color;
            System.Console.WriteLine(message);
            System.Console.ResetColor();
        }

        public static void PrintHeader(string title)
        {
            System.Console.Clear();
            WriteLineColor("╔════════════════════════════════════════════════════════════════╗", ConsoleColor.Cyan);
            WriteLineColor($"║{title,-60}    ║", ConsoleColor.Cyan);
            WriteLineColor("╚════════════════════════════════════════════════════════════════╝", ConsoleColor.Cyan);
            System.Console.WriteLine();
        }

        public static void PrintMovies(List<Models.Movie> movies, string title = " MOVIE LIST")
        {
            PrintHeader(title);
            System.Console.WriteLine($"{"ID",-5} {"TITLE",-35} {"YEAR",-6} {" RATING",-10}");
            System.Console.WriteLine(new string('-', 60));

            foreach (var movie in movies.Take(20))
            {
                System.Console.WriteLine($"{movie.MovieId,-5} {Truncate(movie.Title, 35),-35} {movie.ReleaseYear,-6}  {movie.AverageRating,-10:F1}");
            }
            System.Console.WriteLine();
        }

        public static void PrintRecommendations(List<(Models.Movie movie, double score)> recommendations)
        {
            PrintHeader(" TOP RECOMMENDATIONS FOR YOU");
            System.Console.WriteLine($"{"#",-5} {"TITLE",-40} {"YEAR",-6} {" SCORE",-10}");
            System.Console.WriteLine(new string('-', 65));

            int rank = 1;
            foreach (var (movie, score) in recommendations.Take(5))
            {
                System.Console.WriteLine($"{rank++,-5} {Truncate(movie.Title, 40),-40} {movie.ReleaseYear,-6} {score * 100,-10:F0}%");
            }
            System.Console.WriteLine();
        }

        private static string Truncate(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return text.Length <= maxLength ? text : text.Substring(0, maxLength - 3) + "...";
        }

        public static int GetIntInput(string prompt, int min, int max)
        {
            int result;
            do
            {
                System.Console.Write(prompt);
                string input = System.Console.ReadLine();

                if (int.TryParse(input, out result) && result >= min && result <= max)
                    return result;

                WriteLineColor($"Please enter a number between {min} and {max}", ConsoleColor.Red);
            } while (true);
        }

        public static string GetStringInput(string prompt, bool required = true)
        {
            string input;
            do
            {
                System.Console.Write(prompt);
                input = System.Console.ReadLine()?.Trim();

                if (!required || !string.IsNullOrEmpty(input))
                    return input ?? string.Empty;

                WriteLineColor("This field is required!", ConsoleColor.Red);
            } while (true);
        }

        public static void PressAnyKey()
        {
            System.Console.WriteLine();
            WriteLineColor("Press any key to continue...", ConsoleColor.DarkGray);
            System.Console.ReadKey();
        }

        public static void ShowMainMenu()
        {
            PrintHeader(" AI MOVIE RECOMMENDATION SYSTEM");
            System.Console.WriteLine("   1. Register");
            System.Console.WriteLine("   2. Login");
            System.Console.WriteLine("   3. Exit");
            System.Console.WriteLine();
        }

        public static void ShowUserMenu()
        {
            PrintHeader(" USER DASHBOARD");
            System.Console.WriteLine("   1)  Browse All Movies");
            System.Console.WriteLine("   2)  Search Movies");
            System.Console.WriteLine("   3)  Rate a Movie");
            System.Console.WriteLine("   4)  Get AI Recommendations");
            System.Console.WriteLine("   5)  My Watch History");
            System.Console.WriteLine("   6)  Remove a Rating");
            System.Console.WriteLine("   7)  Trending Movies");
            System.Console.WriteLine("   8)  Recently Watched");
            System.Console.WriteLine("   9)  Export Recommendations to File");
            System.Console.WriteLine("   10) Logout");
            System.Console.WriteLine();
        }

        public static void ShowSearchMenu()
        {
            PrintHeader(" SEARCH MOVIES");
            System.Console.WriteLine("   1)  Search by Title");
            System.Console.WriteLine("   2)  Search by Genre");
            System.Console.WriteLine("   3)  Search by Year");
            System.Console.WriteLine("   4) Search by Director");
            System.Console.WriteLine("   5) Search by Rating (min 1-5)");
            System.Console.WriteLine("   6) Back to Dashboard");
            System.Console.WriteLine();
        }
    }
}