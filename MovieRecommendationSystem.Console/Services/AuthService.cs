using System;
using System.Collections.Generic;
using System.Linq;
using MovieRecommendationSystem.Console.Models;
using MovieRecommendationSystem.Console.UI;

namespace MovieRecommendationSystem.Console.Services
{
    public class AuthService
    {
        private readonly DataStorageService _storage;
        private List<User> _users;
        private User _currentUser;

        public AuthService(DataStorageService storage)
        {
            _storage = storage;
            _users = _storage.LoadUsers();

            if (_users.Count == 0)
            {
                CreateDefaultAdmin();
            }
        }

        private void CreateDefaultAdmin()
        {
            var admin = new User
            {
                Id = 1,
                Username = "admin",
                Password = "admin123",
                Name = "System Administrator",
                IsAdmin = true,
                AdminLevel = "Super",
                RegisteredAt = DateTime.Now,
                FavoriteGenres = new List<int>(),
                WatchHistory = new List<int>(),
                Ratings = new Dictionary<int, int>()
            };
            _users.Add(admin);
            _storage.SaveUsers(_users);
            ConsoleHelper.WriteLineColor("Default admin created: Username = admin, Password = admin123", ConsoleColor.Yellow);
        }

        public User Register()
        {
            ConsoleHelper.PrintHeader("REGISTRATION");

            string username = ConsoleHelper.GetStringInput("Enter username: ");

            if (_users.Any(u => u.Username == username))
            {
                ConsoleHelper.WriteLineColor("Username already exists!", ConsoleColor.Red);
                ConsoleHelper.PressAnyKey();
                return null;
            }

            string password = ConsoleHelper.GetStringInput("Enter password: ");
            string name = ConsoleHelper.GetStringInput("Enter your full name: ");

            int newId = _users.Count > 0 ? _users.Max(u => u.Id) + 1 : 2;

            var newUser = new User
            {
                Id = newId,
                Username = username,
                Password = password,
                Name = name,
                IsAdmin = false,
                RegisteredAt = DateTime.Now,
                FavoriteGenres = new List<int>(),
                WatchHistory = new List<int>(),
                Ratings = new Dictionary<int, int>()
            };

            _users.Add(newUser);
            _storage.SaveUsers(_users);

            ConsoleHelper.WriteLineColor("Registration successful! Please login.", ConsoleColor.Green);
            ConsoleHelper.PressAnyKey();
            return newUser;
        }

        public User Login()
        {
            ConsoleHelper.PrintHeader("LOGIN");

            string username = ConsoleHelper.GetStringInput("Username: ");
            string password = ConsoleHelper.GetStringInput("Password: ");

            var user = _users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                ConsoleHelper.WriteLineColor("Invalid username or password!", ConsoleColor.Red);
                ConsoleHelper.PressAnyKey();
                return null;
            }

            user.LastLogin = DateTime.Now;
            _storage.SaveUsers(_users);

            ConsoleHelper.WriteLineColor($"Welcome back, {user.Name}!", ConsoleColor.Green);
            ConsoleHelper.PressAnyKey();
            return user;
        }

        public void Logout()
        {
            _currentUser = null;
        }

        public void SetCurrentUser(User user)
        {
            _currentUser = user;
        }

        public List<User> GetAllUsers() => _users;

        public void UpdateUser(User user)
        {
            var index = _users.FindIndex(u => u.Id == user.Id);
            if (index >= 0)
            {
                _users[index] = user;
                _storage.SaveUsers(_users);
            }
        }

        public void UpdateUserRatings(User user)
        {
            UpdateUser(user);
        }
    }
}