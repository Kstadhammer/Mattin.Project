# Mattin Project Management System

A console-based project management system built with .NET 9.0, featuring a clean architecture and modern C# practices.

## Features

- 📋 Project Management
  - Create and edit projects
  - Track project status and progress
  - Manage project budgets and timelines
  - Assign project managers

- 👥 Client Management
  - Maintain client information
  - Track client projects
  - Monitor client project values

- 👤 Project Manager Management
  - Assign managers to projects
  - Track manager workload
  - Department-based organization

## Technical Stack

- .NET 9.0
- Entity Framework Core
- SQLite Database
- AutoMapper
- Clean Architecture

## Project Structure

```
Mattin.Project/
├── Mattin.Project.Core/           # Domain entities, interfaces, DTOs
├── Mattin.Project.Infrastructure/ # Data access, repositories, services
└── Mattin.Project.Presentation/   # Console UI, menus, user interaction
```

## Getting Started

1. Clone the repository:
```bash
git clone https://github.com/Kstadhammer/Mattin.Project.git
```

2. Navigate to the project directory:
```bash
cd Mattin.Project
```

3. Build the solution:
```bash
dotnet build
```

4. Run the application:
```bash
dotnet run --project Mattin.Project.Presentation
```

## Database

The application uses SQLite with Entity Framework Core. The database will be automatically created on first run in the application's output directory.

## Navigation

- Use arrow keys (↑/↓) to navigate menus
- Press Enter to select an option
- Follow on-screen prompts for data input

## License

MIT License 