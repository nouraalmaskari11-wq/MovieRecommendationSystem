# 🎬 AI-Powered Movie Recommendation System

## 📝 Project Description

This is a **console-based movie recommendation system** that uses artificial intelligence to recommend movies based on user preferences. Users can register, login, rate movies, and receive personalized AI-powered recommendations.

The system implements **Content-Based Filtering** and **Collaborative Filtering** algorithms with a **Hybrid Approach** to provide accurate movie suggestions.

---

## 👥 Team Members

| Name | Role |
|------|------|
| **Noura** | Backend Development + AI Logic |
| **Elham** | UI Development + Integration |

---

## 🛠️ Technologies Used

| Technology | Purpose |
|------------|---------|
| C# / .NET 8 | Core programming language |
| LINQ | Data filtering and sorting |
| Newtonsoft.Json | JSON serialization/deserialization |
| ML.NET | AI and machine learning concepts |

---

## 📂 Project Structure


<img width="823" height="256" alt="Screenshot 2026-05-17 110341" src="https://github.com/user-attachments/assets/9e1d6b17-7a25-4893-9e00-94310c30bb57" />


---


---

## 🧠 AI Logic & Algorithms

### 1. Content-Based Filtering
- Recommends movies based on **genres**, **director**, and **tags**
- Weighted scoring: 50% genre match, 20% director match, 30% popularity

### 2. Collaborative Filtering
- Finds **similar users** based on their ratings
- Uses **Cosine Similarity** to calculate user similarity

### 3. Hybrid Recommendation
- Combines both strategies (40% Content-Based + 60% Collaborative)
- Provides more accurate recommendations

### 4. Cosine Similarity Formula

<img width="615" height="44" alt="Screenshot 2026-05-17 110425" src="https://github.com/user-attachments/assets/4b008b07-ad26-4174-92c1-bc1170b8a758" />


---

---

## ✨ Features

| Feature | Description |
|---------|-------------|
| 👤 User Authentication | Register, Login, Logout |
| 🎬 Browse Movies | View all 50 movies |
| 🔍 Search | By title, genre, year, director, rating |
| ⭐ Rate Movies | Rate from 1 to 5 |
| 🗑️ Remove Rating | Delete existing ratings |
| 🤖 AI Recommendations | Personalized movie suggestions |
| 🔥 Trending Movies | Top rated movies |
| 🕐 Recently Watched | Recently rated movies |
| 💾 Export to File | Save recommendations as .txt |
| 📜 Watch History | View all rated movies |
| 💾 Data Persistence | JSON file storage |

---

## 🎯 OOP Concepts Implemented

| Concept | Implementation |
|---------|----------------|
| **Encapsulation** | Private fields with public properties in Models |
| **Inheritance** | User class inherits from Person abstract class |
| **Abstraction** | Person abstract class with abstract methods |
| **Interfaces** | IRecommendable, ISearchable, IDataStorage |
| **Polymorphism** | Multiple recommendation strategies |

---

## 🚀 How to Run the Project

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Git](https://git-scm.com/)

### Steps


# 1. Clone the repository
git clone https://github.com/nouraalmaskari11-wq/MovieRecommendationSystem.git

# 2. Navigate to project folder
cd MovieRecommendationSystem/MovieRecommendationSystem.Console

# 3. Restore packages
dotnet restore

# 4. Run the application
dotnet run


<img width="919" height="539" alt="Screenshot 2026-05-17 110504" src="https://github.com/user-attachments/assets/0100140b-be13-49a2-b683-c53bd0a25e0f" />

<img width="950" height="638" alt="Screenshot 2026-05-17 110549" src="https://github.com/user-attachments/assets/7d003030-85a3-442e-81ba-28cf74ee4777" />




