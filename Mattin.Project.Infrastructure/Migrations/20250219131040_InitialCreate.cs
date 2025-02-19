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
                    ProjectManager = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    HourlyRate = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    TotalPrice = table.Column<decimal>(type: "TEXT", precision: 15, scale: 2, nullable: false),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false),
                    StatusId = table.Column<int>(type: "INTEGER", nullable: false),
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
                    { 1, "Kungsgatan 1, 111 43 Stockholm", new DateTime(2025, 2, 19, 13, 10, 39, 906, DateTimeKind.Utc).AddTicks(4310), "info@mattin-lassei.se", null, "Mattin-Lassei Group AB", "08-123 45 67" },
                    { 2, "Sveavägen 10, 111 57 Stockholm", new DateTime(2025, 2, 19, 13, 10, 39, 906, DateTimeKind.Utc).AddTicks(4310), "contact@techinnovators.se", null, "Tech Innovators AB", "08-987 65 43" }
                });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "Created", "Description", "IsDefault", "Modified", "Name", "SortOrder" },
                values: new object[] { 1, new DateTime(2025, 2, 19, 13, 10, 39, 906, DateTimeKind.Utc).AddTicks(4310), "Projektet har inte påbörjats än", true, null, "Ej påbörjat", 1 });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "Created", "Description", "Modified", "Name", "SortOrder" },
                values: new object[,]
                {
                    { 2, new DateTime(2025, 2, 19, 13, 10, 39, 906, DateTimeKind.Utc).AddTicks(4310), "Projektet är under utveckling", null, "Pågående", 2 },
                    { 3, new DateTime(2025, 2, 19, 13, 10, 39, 906, DateTimeKind.Utc).AddTicks(4310), "Projektet är färdigt", null, "Avslutat", 3 }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "ClientId", "Created", "Description", "EndDate", "HourlyRate", "Modified", "ProjectManager", "ProjectNumber", "StartDate", "StatusId", "Title", "TotalPrice" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 2, 19, 13, 10, 39, 906, DateTimeKind.Utc).AddTicks(4310), "Komplett redesign av företagets webbplats med fokus på användarupplevelse", new DateTime(2025, 4, 20, 13, 10, 39, 906, DateTimeKind.Utc).AddTicks(4310), 1200m, null, "Anna Andersson", "2024-001", new DateTime(2025, 1, 20, 13, 10, 39, 906, DateTimeKind.Utc).AddTicks(4310), 2, "Webbplats Redesign", 480000m },
                    { 2, 2, new DateTime(2025, 2, 19, 13, 10, 39, 906, DateTimeKind.Utc).AddTicks(4310), "Utveckling av ny e-handelsplattform med integration mot befintliga system", new DateTime(2025, 6, 4, 13, 10, 39, 906, DateTimeKind.Utc).AddTicks(4310), 1100m, null, "Erik Eriksson", "2024-002", new DateTime(2025, 3, 6, 13, 10, 39, 906, DateTimeKind.Utc).AddTicks(4310), 1, "E-handelsplattform", 880000m },
                    { 3, 1, new DateTime(2025, 2, 19, 13, 10, 39, 906, DateTimeKind.Utc).AddTicks(4310), "Utveckling av mobilapp för intern kommunikation", new DateTime(2025, 4, 5, 13, 10, 39, 906, DateTimeKind.Utc).AddTicks(4310), 1300m, null, "Maria Nilsson", "2024-003", new DateTime(2025, 2, 4, 13, 10, 39, 906, DateTimeKind.Utc).AddTicks(4310), 2, "App-utveckling", 416000m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ClientId",
                table: "Projects",
                column: "ClientId");

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
                name: "Statuses");
        }
    }
}
