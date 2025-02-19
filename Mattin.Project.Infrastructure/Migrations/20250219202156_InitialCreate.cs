using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mattin.Project.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modified = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectManagers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Department = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modified = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectManagers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    BasePrice = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    HourlyRate = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    Category = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modified = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    IsDefault = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modified = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProjectNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HourlyRate = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    TotalPrice = table.Column<decimal>(type: "TEXT", precision: 15, scale: 2, nullable: false),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false),
                    StatusId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProjectManagerId = table.Column<int>(type: "INTEGER", nullable: false),
                    ServiceId = table.Column<int>(type: "INTEGER", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modified = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_ProjectManagers_ProjectManagerId",
                        column: x => x.ProjectManagerId,
                        principalTable: "ProjectManagers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Projects_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Address", "Created", "Email", "Modified", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "Kungsgatan 1, 111 43 Stockholm", new DateTime(2025, 2, 19, 20, 21, 56, 723, DateTimeKind.Utc).AddTicks(5090), "info@mattin-lassei.se", null, "Mattin-Lassei Group AB", "08-123 45 67" },
                    { 2, "Sveavägen 10, 111 57 Stockholm", new DateTime(2025, 2, 19, 20, 21, 56, 723, DateTimeKind.Utc).AddTicks(5090), "contact@techinnovators.se", null, "Tech Innovators AB", "08-987 65 43" }
                });

            migrationBuilder.InsertData(
                table: "ProjectManagers",
                columns: new[] { "Id", "Created", "Department", "Email", "IsActive", "Modified", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 19, 20, 21, 56, 723, DateTimeKind.Utc).AddTicks(5090), "Development", "anna.andersson@mattin-lassei.se", true, null, "Anna Andersson", "070-123 45 67" },
                    { 2, new DateTime(2025, 2, 19, 20, 21, 56, 723, DateTimeKind.Utc).AddTicks(5090), "Design", "erik.eriksson@mattin-lassei.se", true, null, "Erik Eriksson", "070-234 56 78" },
                    { 3, new DateTime(2025, 2, 19, 20, 21, 56, 723, DateTimeKind.Utc).AddTicks(5090), "Mobile Development", "maria.nilsson@mattin-lassei.se", true, null, "Maria Nilsson", "070-345 67 89" }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "BasePrice", "Category", "Created", "Description", "HourlyRate", "IsActive", "Modified", "Name" },
                values: new object[,]
                {
                    { 1, 50000m, "Development", new DateTime(2025, 2, 19, 20, 21, 56, 723, DateTimeKind.Utc).AddTicks(5090), "Full-stack web development services including frontend and backend", 1200m, true, null, "Web Development" },
                    { 2, 75000m, "Development", new DateTime(2025, 2, 19, 20, 21, 56, 723, DateTimeKind.Utc).AddTicks(5090), "Native and cross-platform mobile application development", 1300m, true, null, "Mobile App Development" },
                    { 3, 35000m, "Design", new DateTime(2025, 2, 19, 20, 21, 56, 723, DateTimeKind.Utc).AddTicks(5090), "User interface and experience design for digital products", 1100m, true, null, "UI/UX Design" },
                    { 4, 65000m, "Infrastructure", new DateTime(2025, 2, 19, 20, 21, 56, 723, DateTimeKind.Utc).AddTicks(5090), "Setup and management of cloud infrastructure and DevOps practices", 1400m, true, null, "Cloud Infrastructure" },
                    { 5, 45000m, "Security", new DateTime(2025, 2, 19, 20, 21, 56, 723, DateTimeKind.Utc).AddTicks(5090), "Comprehensive security assessment and penetration testing", 1500m, true, null, "Security Audit" },
                    { 6, 55000m, "Analytics", new DateTime(2025, 2, 19, 20, 21, 56, 723, DateTimeKind.Utc).AddTicks(5090), "Business intelligence and data analytics solutions", 1250m, true, null, "Data Analytics" }
                });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "Created", "Description", "IsDefault", "Modified", "Name", "SortOrder" },
                values: new object[] { 1, new DateTime(2025, 2, 19, 20, 21, 56, 723, DateTimeKind.Utc).AddTicks(5090), "Project has not been started yet", true, null, "Not Started", 1 });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "Created", "Description", "Modified", "Name", "SortOrder" },
                values: new object[,]
                {
                    { 2, new DateTime(2025, 2, 19, 20, 21, 56, 723, DateTimeKind.Utc).AddTicks(5090), "Project is under development", null, "In Progress", 2 },
                    { 3, new DateTime(2025, 2, 19, 20, 21, 56, 723, DateTimeKind.Utc).AddTicks(5090), "Project is finished", null, "Completed", 3 }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "ClientId", "Created", "Description", "EndDate", "HourlyRate", "Modified", "ProjectManagerId", "ProjectNumber", "ServiceId", "StartDate", "StatusId", "Title", "TotalPrice" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 2, 19, 20, 21, 56, 723, DateTimeKind.Utc).AddTicks(5090), "Komplett redesign av företagets webbplats med fokus på användarupplevelse", new DateTime(2025, 4, 20, 20, 21, 56, 723, DateTimeKind.Utc).AddTicks(5090), 1200m, null, 1, "2024-001", 1, new DateTime(2025, 1, 20, 20, 21, 56, 723, DateTimeKind.Utc).AddTicks(5090), 2, "Webbplats Redesign", 480000m },
                    { 2, 2, new DateTime(2025, 2, 19, 20, 21, 56, 723, DateTimeKind.Utc).AddTicks(5090), "Utveckling av ny e-handelsplattform med integration mot befintliga system", new DateTime(2025, 6, 4, 20, 21, 56, 723, DateTimeKind.Utc).AddTicks(5090), 1100m, null, 2, "2024-002", 1, new DateTime(2025, 3, 6, 20, 21, 56, 723, DateTimeKind.Utc).AddTicks(5090), 1, "E-handelsplattform", 880000m },
                    { 3, 1, new DateTime(2025, 2, 19, 20, 21, 56, 723, DateTimeKind.Utc).AddTicks(5090), "Utveckling av mobilapp för intern kommunikation", new DateTime(2025, 4, 5, 20, 21, 56, 723, DateTimeKind.Utc).AddTicks(5090), 1300m, null, 3, "2024-003", 2, new DateTime(2025, 2, 4, 20, 21, 56, 723, DateTimeKind.Utc).AddTicks(5090), 2, "App-utveckling", 416000m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ClientId",
                table: "Projects",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectManagerId",
                table: "Projects",
                column: "ProjectManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ServiceId",
                table: "Projects",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_StatusId",
                table: "Projects",
                column: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "ProjectManagers");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Statuses");
        }
    }
}
