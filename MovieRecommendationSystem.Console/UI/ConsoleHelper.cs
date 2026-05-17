using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MovieRecommendationSystem.Console.Models;

namespace MovieRecommendationSystem.Console.UI
{
    public static class ConsoleHelper
    {
        // ============================================================
        // BASIC FUNCTIONS
        // ============================================================

        public static void Write(string message)
        {
            System.Console.Write(message);
        }

        public static void WriteLine(string message)
        {
            System.Console.WriteLine(message);
        }

        public static void Clear()
        {
            System.Console.Clear();
        }

        public static void PrintHeader(string title)
        {
            Clear();
            WriteLine("╔══════════════════════════════════════════════════════════════════════════════════════════╗");
            WriteLine($"║{title,-74}                ║");
            WriteLine("╚══════════════════════════════════════════════════════════════════════════════════════════╝");
            WriteLine("");
        }

        public static int GetIntInput(string prompt, int min, int max)
        {
            int result;
            do
            {
                Write(prompt);
                string input = System.Console.ReadLine();

                if (int.TryParse(input, out result) && result >= min && result <= max)
                    return result;

                WriteLine($" Please enter a number between {min} and {max}");
            } while (true);
        }

        public static string GetStringInput(string prompt, bool required = true)
        {
            string input;
            do
            {
                Write(prompt);
                input = System.Console.ReadLine()?.Trim();

                if (!required || !string.IsNullOrEmpty(input))
                    return input ?? string.Empty;

                WriteLine(" This field is required!");
            } while (true);
        }

        public static void PressAnyKey()
        {
            WriteLine("");
            WriteLine("Press any key to continue...");
            System.Console.ReadKey();
        }

        // ============================================================
        // COLOR FUNCTIONS (للتوافق مع الكود القديم)
        // ============================================================

        public static void WriteColor(string message, ConsoleColor color)
        {
            System.Console.Write(message);
        }

        public static void WriteLineColor(string message, ConsoleColor color)
        {
            System.Console.WriteLine(message);
        }

        // ============================================================
        // SPLASH SCREEN
        // ============================================================

        public static void ShowSplash()
        {
            Clear();

            string[] frame = {
                "",
                "",
                "           ██████╗██╗███╗   ██╗███████╗███╗   ███╗ █████╗",
                "          ██╔════╝██║████╗  ██║██╔════╝████╗ ████║██╔══██╗",
                "          ██║     ██║██╔██╗ ██║█████╗  ██╔████╔██║███████║",
                "          ██║     ██║██║╚██╗██║██╔══╝  ██║╚██╔╝██║██╔══██║",
                "          ╚██████╗██║██║ ╚████║███████╗██║ ╚═╝ ██║██║  ██║",
                "           ╚═════╝╚═╝╚═╝  ╚═══╝╚══════╝╚═╝     ╚═╝╚═╝  ╚═╝",
                "",
                "",
                "              W E L C O M E   T O   T H E   C I N E M A",
                "",
                "                Your Next Favorite Movie Awaits You",
                "",
                "                    Initializing System...",
                "",
                "              ┌─────────────────────────────────────────┐",
                "              │               L O A D I N G             │",
                "              └─────────────────────────────────────────┘",
                ""
            };

            foreach (string line in frame)
            {
                System.Console.WriteLine(line);
                Thread.Sleep(100);
            }

            // Progress bar
            for (int percent = 0; percent <= 100; percent += 10)
            {
                int filled = percent / 2;
                string bar = new string('█', filled) + new string('░', 50 - filled);
                System.Console.SetCursorPosition(18, 18);
                System.Console.Write($"{bar} {percent}%");
                Thread.Sleep(60);
            }

            Thread.Sleep(800);
            Clear();
        }

        // ============================================================
        // MAIN MENU
        // ============================================================

        public static void ShowMainMenu()
        {
            Clear();

            WriteLine("╔══════════════════════════════════════════════════════════════════════════════════════════╗");
            WriteLine("║                                      M A I N   M E N U                                   ║");
            WriteLine("╠══════════════════════════════════════════════════════════════════════════════════════════╣");
            WriteLine("║                                                                                          ║");
            WriteLine("║                                   1.  CREATE NEW ACCOUNT                                 ║");
            WriteLine("║                                   2.  LOGIN                                              ║");
            WriteLine("║                                   3.  EXIT                                               ║");
            WriteLine("║                                                                                          ║");
            WriteLine("╚══════════════════════════════════════════════════════════════════════════════════════════╝");
            WriteLine("");
            WriteLine("                                   Please select an option");
        }

        // ============================================================
        // USER DASHBOARD
        // ============================================================

        public static void ShowDashboard()
        {
            Clear();

            WriteLine("╔══════════════════════════════════════════════════════════════════════════════════════════════╗");
            WriteLine("║                                      D A S H B O A R D                                       ║");
            WriteLine("╠══════════════════════════════════════════════════════════════════════════════════════════════╣");
            WriteLine("║                                                                                              ║");
            WriteLine("║   ┌─────────────────────────────────────┐          ┌─────────────────────────────────────┐   ║");
            WriteLine("║   │           MAIN MENU                 │          │           EXTRA MENU                │   ║");
            WriteLine("║   ├─────────────────────────────────────┤          ├─────────────────────────────────────┤   ║");
            WriteLine("║   │                                     │          │                                     │   ║");
            WriteLine("║   │   1.  BROWSE ALL MOVIES             │          │   6.  REMOVE A RATING               │   ║");
            WriteLine("║   │   2.  SEARCH MOVIES                 │          │   7.  TRENDING MOVIES               │   ║");
            WriteLine("║   │   3.  RATE A MOVIE                  │          │   8.  RECENTLY WATCHED              │   ║");
            WriteLine("║   │   4.  AI RECOMMENDATIONS            │          │   9.  SAVE RECOMMENDATIONS          │   ║");
            WriteLine("║   │   5.  WATCH HISTORY                 │          │   10. LOGOUT                        │   ║");
            WriteLine("║   │                                     │          │                                     │   ║");
            WriteLine("║   └─────────────────────────────────────┘          └─────────────────────────────────────┘   ║");
            WriteLine("║                                                                                              ║");
            WriteLine("╚══════════════════════════════════════════════════════════════════════════════════════════════╝");
            WriteLine("");
            WriteLine("                                   What would you like to watch?");
        }

        // ============================================================
        // SEARCH MENU
        // ============================================================

        public static void ShowSearchMenu()
        {
            Clear();

            WriteLine("╔══════════════════════════════════════════════════════════════════════════════════╗");
            WriteLine("║                                    S E A R C H                                   ║");
            WriteLine("╠══════════════════════════════════════════════════════════════════════════════════╣");
            WriteLine("║                                                                                  ║");
            WriteLine("║                           1.  Search by Title                                    ║");
            WriteLine("║                           2.  Search by Genre                                    ║");
            WriteLine("║                           3.  Search by Year                                     ║");
            WriteLine("║                           4.  Search by Director                                 ║");
            WriteLine("║                           5.  Search by Rating (min 1-5)                         ║");
            WriteLine("║                           6.  Back to Dashboard                                  ║");
            WriteLine("║                                                                                  ║");
            WriteLine("╚══════════════════════════════════════════════════════════════════════════════════╝");
        }

        // ============================================================
        // PRINT MOVIES
        // ============================================================

        public static void PrintMovies(List<Movie> movies, string title = "MOVIE COLLECTION")
        {
            Clear();

            WriteLine($"╔═════════════════════════════════════════════════════════════════════════════════════╗");
            WriteLine($"║{title,-84} ║");
            WriteLine($"╠═════════════════════════════════════════════════════════════════════════════════════╣");

            WriteLine($"║  ID  │ TITLE                                            │ YEAR  │ RATING            ║");
            WriteLine($"╠══════╪══════════════════════════════════════════════════╪═══════╪═══════════════════╣");

            foreach (var movie in movies.Take(50))
            {
                string titleShort = movie.Title.Length > 48 ? movie.Title.Substring(0, 45) + "..." : movie.Title;
                int stars = (int)Math.Round(movie.AverageRating);
                string starDisplay = new string('*', stars) + new string('-', 5 - stars);
                WriteLine($"║ {movie.MovieId,3}  │ {titleShort,-48} │ {movie.ReleaseYear,5} │ {movie.AverageRating:F1}/5 {starDisplay,-12}║");

            }

            WriteLine($"╚═════════════════════════════════════════════════════════════════════════════════════╝");
            WriteLine("");
        }

        // ============================================================
        // PRINT RECOMMENDATIONS
        // ============================================================

        public static void PrintRecommendations(List<(Movie movie, double score, double confidence)> recommendations)
        {
            Clear();

            WriteLine("╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗");
            WriteLine("║                                    A I   R E C O M M E N D A T I O N S                                         ║");
            WriteLine("╠════════════════════════════════════════════════════════════════════════════════════════════════════════════════╣");

            int rank = 1;
            foreach (var rec in recommendations.Take(5))
            {
                var movie = rec.movie;
                var confidence = rec.confidence;
                int stars = (int)Math.Round(movie.AverageRating);
                string starDisplay = new string('*', stars) + new string('-', 5 - stars);

                WriteLine($"║                                                                                                                ║");
                WriteLine($"║   {rank++}.  {movie.Title} ({movie.ReleaseYear})                                                                                   ║");
                WriteLine($"║       Rating: {movie.AverageRating}/5 {starDisplay}                                                                                      ║");
                WriteLine($"║       AI Match: {confidence:F0}%                                                                                           ║");
                WriteLine($"║       Director: {movie.Director}                                                                                     ║");
                WriteLine($"║                                                                                                                ║");

                if (rank <= recommendations.Count)
                    WriteLine("╠════════════════════════════════════════════════════════════════════════════════════════════════════════════════╣");
            }

            WriteLine("╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝");
        }

        // ============================================================
        // MOVIE DETAILS
        // ============================================================

        public static void PrintMovieDetails(Movie movie)
        {
            Clear();

            WriteLine("╔══════════════════════════════════════════════════════════════════════════════════════════╗");
            WriteLine("║                                    M O V I E   D E T A I L S                             ║");
            WriteLine("╠══════════════════════════════════════════════════════════════════════════════════════════╣");
            WriteLine($"║                                                                                          ║");
            WriteLine($"║   TITLE:     {movie.Title}                                                                   ║");
            WriteLine($"║   YEAR:      {movie.ReleaseYear}                                                                        ║");
            WriteLine($"║   DIRECTOR:  {movie.Director}                                                           ║");

            int stars = (int)Math.Round(movie.AverageRating);
            string starDisplay = new string('*', stars) + new string('-', 5 - stars);
            WriteLine($"║   RATING:    {movie.AverageRating}/5 {starDisplay}                                                                 ║");

            WriteLine($"║   GENRES:    {string.Join(", ", movie.Genres.Select(g => GetGenreName(g)))}                                                    ║");
            WriteLine($"║                                                                                          ║");
            WriteLine($"║   SYNOPSIS:  {movie.Description}      ║");
            WriteLine($"║                                                                                          ║");
            WriteLine("╚══════════════════════════════════════════════════════════════════════════════════════════╝");
        }

        private static string GetGenreName(int genreId)
        {
            return genreId switch
            {
                1 => "Action",
                2 => "Comedy",
                3 => "Sci-Fi",
                4 => "Drama",
                5 => "Thriller",
                6 => "Romance",
                7 => "Horror",
                8 => "Adventure",
                _ => "Unknown"
            };
        }

        // ============================================================
        // MESSAGES
        // ============================================================

        public static void ShowSuccess(string message)
        {
            WriteLine($"\n[SUCCESS] {message}");
        }

        public static void ShowError(string message)
        {
            WriteLine($"\n[ERROR] {message}");
        }

        public static void ShowWarning(string message)
        {
            WriteLine($"\n[WARNING] {message}");
        }

        public static void ShowInfo(string message)
        {
            WriteLine($"\n[INFO] {message}");
        }
    }
}