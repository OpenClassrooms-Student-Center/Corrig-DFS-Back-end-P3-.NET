using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EquipIT.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddBorrowingDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    IsAvailable = table.Column<bool>(type: "INTEGER", nullable: false),
                    BorrowerFirstName = table.Column<string>(type: "TEXT", nullable: true),
                    BorrowerLastName = table.Column<string>(type: "TEXT", nullable: true),
                    BorrowDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastReturnDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Equipments",
                columns: new[] { "Id", "BorrowDate", "BorrowerFirstName", "BorrowerLastName", "Category", "IsAvailable", "LastReturnDate", "Name" },
                values: new object[,]
                {
                    { 1, null, null, null, "Display", true, null, "Monitor" },
                    { 2, null, null, null, "Input", true, null, "Keyboard" },
                    { 3, null, null, null, "Input", true, null, "Mouse" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Equipments");
        }
    }
}
