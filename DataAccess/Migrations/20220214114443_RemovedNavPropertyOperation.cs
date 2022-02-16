using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class RemovedNavPropertyOperation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operations_OperationCategories_CategoryId",
                table: "Operations");

            migrationBuilder.DropIndex(
                name: "IX_Operations_CategoryId",
                table: "Operations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Operations_CategoryId",
                table: "Operations",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_OperationCategories_CategoryId",
                table: "Operations",
                column: "CategoryId",
                principalTable: "OperationCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
