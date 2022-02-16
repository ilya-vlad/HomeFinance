using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OperationCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsIncome = table.Column<bool>(type: "bit", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operations_OperationCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "OperationCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "OperationCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Foodstuffs" },
                    { 2, "Rent" },
                    { 3, "FastFood" },
                    { 4, "Hobby" },
                    { 5, "Other" },
                    { 6, "Salary" },
                    { 7, "Present" }
                });

            migrationBuilder.InsertData(
                table: "Operations",
                columns: new[] { "Id", "Amount", "CategoryId", "Date", "IsIncome" },
                values: new object[,]
                {
                    { 1, 10m, 1, new DateTime(2022, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), false },
                    { 4, 40m, 1, new DateTime(2022, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), false },
                    { 9, 10m, 1, new DateTime(2022, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), false },
                    { 10, 300m, 2, new DateTime(2022, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), false },
                    { 2, 2m, 3, new DateTime(2022, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), false },
                    { 6, 5m, 3, new DateTime(2022, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), false },
                    { 7, 2m, 3, new DateTime(2022, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false },
                    { 3, 220m, 4, new DateTime(2022, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), false },
                    { 5, 140m, 5, new DateTime(2022, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), false },
                    { 8, 53m, 5, new DateTime(2022, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), false },
                    { 11, 1000m, 6, new DateTime(2022, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), true },
                    { 12, 100m, 7, new DateTime(2022, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Operations_CategoryId",
                table: "Operations",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "OperationCategories");
        }
    }
}
