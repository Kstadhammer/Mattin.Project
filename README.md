# Mattin Project Management System

A comprehensive project management system developed for Mattin-Lassei Group AB as part of the Data Storage (Datalagring) course at EC Utbildning. The system includes both a Console application and a Web interface.

## Course Information
- **Course**: Data Storage (Datalagring)
- **Institution**: EC Utbildning
- **Assignment**: Project Management System Implementation
- **Grade Level**: VG (Väl Godkänt)

## Project Overview

This project implements a project management system with two interfaces:
1. Console-based application for direct system management
2. Web-based interface using ASP.NET Core MVC for user-friendly access

### Key Features
- Project tracking with automatic project number generation
- Client management
- Project manager assignment
- Service catalog with pricing
- Status tracking
- Budget and timeline management
- Shared SQLite database between both interfaces

### Key Requirements Implemented (VG Level)

#### Base Requirements (G)
- ✅ Multiple user interfaces (Console and Web)
- ✅ Entity Framework Core - Code First implementation
- ✅ SQLite database integration
- ✅ Multiple entities/tables (Projects, Clients, Services, etc.)
- ✅ Service layer for business logic
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
- ✅ Shared database between applications

## Technical Stack

- .NET 9.0
- Entity Framework Core
- SQLite Database
- ASP.NET Core MVC
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
├── Mattin.Project.Presentation/   # Console UI
│   ├── Helpers/                 # UI utilities
│   └── Menus/                   # Interactive menus
└── Mattin.Project.Web/           # Web UI
    ├── Controllers/             # MVC Controllers
    ├── Views/                   # Razor views
    └── Models/                  # View models
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

### Service Management
- Service catalog
- Pricing configuration
- Category organization
- Active/Inactive status

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

4. Run the Console application:
```bash
dotnet run --project Mattin.Project.Presentation
```

5. Run the Web application:
```bash
dotnet run --project Mattin.Project.Web
```

## Database

The application uses SQLite with Entity Framework Core. The database will be automatically created in the `Data` directory on first run. Both the Console and Web applications share the same database file.

## Navigation

### Console Application
- Use arrow keys (↑/↓) to navigate menus
- Press Enter to select an option
- Follow on-screen prompts for data input

### Web Application
- Navigate using the menu bar
- Use forms to create and edit data
- Click action buttons for operations
- Responsive design for all devices

## License

MIT License