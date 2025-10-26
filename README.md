# 🎯 Smart Todo Planner

A modern, responsive Personal Task Manager built with **ASP.NET Core** and **Tailwind CSS**.

## ✨ Features

- ✅ Add tasks with title, description, due date, and priority
- 🎯 State management (Pending → In Progress → Completed)
- ⭐ Priority system with visual indicators
- 📱 Fully responsive design
- 🎨 Modern UI with Tailwind CSS
- 💾 SQLite database with Entity Framework Core

## 🛠️ Tech Stack

- **Backend**: ASP.NET Core 8.0, Entity Framework Core
- **Frontend**: Tailwind CSS, Razor Pages
- **Database**: SQLite
- **Architecture**: MVC, Repository Pattern

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK
- Node.js (for Tailwind CSS)

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/qatada16/todo-planner.git
   cd todo-planner

2. Restore .NET Dependencies
   ```bash
   # Restore NuGet packages
   dotnet restore

   # Verify project builds
   dotnet build

3. Set Up Frontend Dependencies
   ```bash
   # Install Node.js dependencies
   npm install

   # Build Tailwind CSS for the first time
   npm run build-css-prod

4. Database Setup
   ```bash
   # Create database migrations (when we add EF Core)
   dotnet ef database update

5. Run The Appication
   ```bash
   # Start the development server
   dotnet run
   # OR for auto-reload during development
   dotnet watch run

6. You should see something like:
   ```bash
   info: Microsoft.Hosting.Lifetime[14]
         Now listening on: http://localhost:5000
   info: Microsoft.Hosting.Lifetime[14]
         Now listening on: https://localhost:7000
