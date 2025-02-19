Project Management System Assignment
Overview
Develop a console application for Mattin-Lassei Group AB using a layered architecture with base classes, factories, and SQLite database integration.
Project Structure
CopyMattin.Project/
├── Mattin.Project.Core/
│   ├── Interfaces/
│   │   ├── Base/
│   │   │   ├── IBaseRepository.cs
│   │   │   └── IBaseService.cs
│   │   ├── Factories/
│   │   │   ├── IRepositoryFactory.cs
│   │   │   └── IServiceFactory.cs
│   │   ├── IProjectRepository.cs
│   │   ├── IClientRepository.cs
│   │   ├── IProjectService.cs
│   │   └── IClientService.cs
│   ├── Models/
│   │   ├── Entities/
│   │   │   ├── BaseEntity.cs
│   │   │   ├── Project.cs
│   │   │   └── Client.cs
│   │   └── DTOs/
│   ├── Factories/
│   │   ├── RepositoryFactory.cs
│   │   └── ServiceFactory.cs
│   └── Contexts/
│       └── ApplicationDbContext.cs
│
├── Mattin.Project.Infrastructure/
│   ├── Repositories/
│   │   ├── Base/
│   │   │   └── BaseRepository.cs
│   │   ├── ProjectRepository.cs
│   │   └── ClientRepository.cs
│   └── Services/
│       ├── Base/
│       │   └── BaseService.cs
│       ├── ProjectService.cs
│       └── ClientService.cs
│
└── Mattin.Project.Presentation/
    ├── Program.cs
    ├── Factories/
    │   └── MenuFactory.cs
    └── Menus/
        ├── MainMenu.cs
        ├── ProjectMenu.cs
        └── ClientMenu.cs

Requirements
1. Database Implementation
Use Entity Framework Core with SQLite
Implement Code First migrations
Create seed data for testing
2. Architecture
Use base classes for repositories and services
Implement factory pattern for creating instances
Follow SOLID principles
Use dependency injection
3. Features
CRUD operations for projects and clients
Automatic project number generation
Status management for projects
Data validation and error handling
4. Console Interface
Clear menu structure
Input validation
Error messages
Confirmation prompts
Assessment Criteria
Required for Pass Grade:
Working CRUD operations using base classes
Proper implementation of factory pattern
SQLite database integration
Basic error handling
Console menu navigation
Project number generation
Input validation
Code Quality Requirements:
Follow C# coding conventions
Use meaningful names
Implement error handling
Add comments where necessary
Follow SOLID principles
Notes
The database will be created on first run
Include setup instructions in README.md
Document any AI-generated code
Include example data for testing
Last edited just now


