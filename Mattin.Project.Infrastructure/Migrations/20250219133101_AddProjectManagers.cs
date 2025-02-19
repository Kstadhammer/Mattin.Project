using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mattin.Project.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectManagers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectManager",
                table: "Projects");

            migrationBuilder.AddColumn<int>(
                name: "ProjectManagerId",
                table: "Projects",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2025, 2, 19, 13, 31, 1, 25, DateTimeKind.Utc).AddTicks(6860));

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2025, 2, 19, 13, 31, 1, 25, DateTimeKind.Utc).AddTicks(6860));

            migrationBuilder.InsertData(
                table: "ProjectManagers",
                columns: new[] { "Id", "Created", "Department", "Email", "IsActive", "Modified", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 19, 13, 31, 1, 25, DateTimeKind.Utc).AddTicks(6860), "Development", "anna.andersson@mattin-lassei.se", true, null, "Anna Andersson", "070-123 45 67" },
                    { 2, new DateTime(2025, 2, 19, 13, 31, 1, 25, DateTimeKind.Utc).AddTicks(6860), "Design", "erik.eriksson@mattin-lassei.se", true, null, "Erik Eriksson", "070-234 56 78" },
                    { 3, new DateTime(2025, 2, 19, 13, 31, 1, 25, DateTimeKind.Utc).AddTicks(6860), "Mobile Development", "maria.nilsson@mattin-lassei.se", true, null, "Maria Nilsson", "070-345 67 89" }
                });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "EndDate", "ProjectManagerId", "StartDate" },
                values: new object[] { new DateTime(2025, 2, 19, 13, 31, 1, 25, DateTimeKind.Utc).AddTicks(6860), new DateTime(2025, 4, 20, 13, 31, 1, 25, DateTimeKind.Utc).AddTicks(6860), 1, new DateTime(2025, 1, 20, 13, 31, 1, 25, DateTimeKind.Utc).AddTicks(6860) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Created", "EndDate", "ProjectManagerId", "StartDate" },
                values: new object[] { new DateTime(2025, 2, 19, 13, 31, 1, 25, DateTimeKind.Utc).AddTicks(6860), new DateTime(2025, 6, 4, 13, 31, 1, 25, DateTimeKind.Utc).AddTicks(6860), 2, new DateTime(2025, 3, 6, 13, 31, 1, 25, DateTimeKind.Utc).AddTicks(6860) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Created", "EndDate", "ProjectManagerId", "StartDate" },
                values: new object[] { new DateTime(2025, 2, 19, 13, 31, 1, 25, DateTimeKind.Utc).AddTicks(6860), new DateTime(2025, 4, 5, 13, 31, 1, 25, DateTimeKind.Utc).AddTicks(6860), 3, new DateTime(2025, 2, 4, 13, 31, 1, 25, DateTimeKind.Utc).AddTicks(6860) });

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2025, 2, 19, 13, 31, 1, 25, DateTimeKind.Utc).AddTicks(6860));

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2025, 2, 19, 13, 31, 1, 25, DateTimeKind.Utc).AddTicks(6860));

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "Created",
                value: new DateTime(2025, 2, 19, 13, 31, 1, 25, DateTimeKind.Utc).AddTicks(6860));

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectManagerId",
                table: "Projects",
                column: "ProjectManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ProjectManagers_ProjectManagerId",
                table: "Projects",
                column: "ProjectManagerId",
                principalTable: "ProjectManagers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ProjectManagers_ProjectManagerId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "ProjectManagers");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ProjectManagerId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectManagerId",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "ProjectManager",
                table: "Projects",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2025, 2, 19, 13, 10, 39, 906, DateTimeKind.Utc).AddTicks(4310));

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2025, 2, 19, 13, 10, 39, 906, DateTimeKind.Utc).AddTicks(4310));

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "EndDate", "ProjectManager", "StartDate" },
                values: new object[] { new DateTime(2025, 2, 19, 13, 10, 39, 906, DateTimeKind.Utc).AddTicks(4310), new DateTime(2025, 4, 20, 13, 10, 39, 906, DateTimeKind.Utc).AddTicks(4310), "Anna Andersson", new DateTime(2025, 1, 20, 13, 10, 39, 906, DateTimeKind.Utc).AddTicks(4310) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Created", "EndDate", "ProjectManager", "StartDate" },
                values: new object[] { new DateTime(2025, 2, 19, 13, 10, 39, 906, DateTimeKind.Utc).AddTicks(4310), new DateTime(2025, 6, 4, 13, 10, 39, 906, DateTimeKind.Utc).AddTicks(4310), "Erik Eriksson", new DateTime(2025, 3, 6, 13, 10, 39, 906, DateTimeKind.Utc).AddTicks(4310) });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Created", "EndDate", "ProjectManager", "StartDate" },
                values: new object[] { new DateTime(2025, 2, 19, 13, 10, 39, 906, DateTimeKind.Utc).AddTicks(4310), new DateTime(2025, 4, 5, 13, 10, 39, 906, DateTimeKind.Utc).AddTicks(4310), "Maria Nilsson", new DateTime(2025, 2, 4, 13, 10, 39, 906, DateTimeKind.Utc).AddTicks(4310) });

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2025, 2, 19, 13, 10, 39, 906, DateTimeKind.Utc).AddTicks(4310));

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2025, 2, 19, 13, 10, 39, 906, DateTimeKind.Utc).AddTicks(4310));

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "Created",
                value: new DateTime(2025, 2, 19, 13, 10, 39, 906, DateTimeKind.Utc).AddTicks(4310));
        }
    }
}
