# Mattin Project Management System

A comprehensive project management system developed for Mattin-Lassei Group AB as part of the Data Storage (Datalagring) course at EC Utbildning.

## Course Information
- **Course**: Data Storage (Datalagring)
- **Institution**: EC Utbildning
- **Assignment**: Project Management System Implementation
- **Grade Level**: VG (Väl Godkänt)

## Project Overview

This project implements a console-based project management system with the following key features:
- Project tracking with automatic project number generation
- Client management
- Project manager assignment
- Status tracking
- Budget and timeline management

### Key Requirements Implemented (VG Level)

#### Base Requirements (G)
- ✅ Frontend application with project listing, creation, and editing
- ✅ Entity Framework Core - Code First implementation
- ✅ SQLite database integration
- ✅ Multiple entities/tables (Projects, Clients, etc.)
- ✅ Service layer for project and client management
- ✅ Repository pattern implementation
- ✅ Dependency Injection
- ✅ SOLID principles application

#### Advanced Requirements (VG)
- ✅ Generic BaseClass for repositories
- ✅ Extended entity model (Clients, Project Managers, Projects, Status)
- ✅ Comprehensive CRUD operations through services
- ✅ SOLID principles implementation
- ✅ Factory pattern utilization
- ✅ Asynchronous operations with Task/async/await
- ✅ Transaction management for data consistency

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
│   ├── Common/                    # Shared utilities and patterns
│   ├── Interfaces/               # Service and repository contracts
│   ├── Models/                   # Domain models and DTOs
│   └── Mappings/                # AutoMapper profiles
├── Mattin.Project.Infrastructure/ # Data access and services
│   ├── Contexts/                # Database context
│   ├── Repositories/            # Data access implementation
│   ├── Services/                # Business logic implementation
│   └── Factories/              # Object creation patterns
└── Mattin.Project.Presentation/   # Console UI
    ├── Helpers/                 # UI utilities
    └── Menus/                   # Interactive menus
```

## Features

### Project Management
- Create and edit projects
- Automatic project number generation
- Status tracking (Not Started, In Progress, Completed)
- Budget calculation and management
- Timeline management

### Client Management
- Client information management
- Project association
- Active project tracking
- Total value calculation

### Project Manager Features
- Department-based organization
- Project assignment
- Workload tracking

## AI Assistance Acknowledgment

This project was developed with AI assistance in the following areas:
- Architecture design and implementation
- Clean code practices and SOLID principles
- Database schema design
- Transaction management
- UI/UX improvements
- Error handling patterns
- Comprehensive code documentation and comments
- Factory pattern implementations
- Repository and service layer design
- Data mapping strategies

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

The application uses SQLite with Entity Framework Core. The database will be automatically created in the `Data` directory on first run.

## Navigation

- Use arrow keys (↑/↓) to navigate menus
- Press Enter to select an option
- Follow on-screen prompts for data input

## License

MIT License