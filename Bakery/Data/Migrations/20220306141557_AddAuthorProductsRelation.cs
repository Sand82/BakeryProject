using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bakery.Data.Migrations
{
    public partial class AddAuthorProductsRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_AuthorId",
                table: "Products",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Authors_AuthorId",
                table: "Products",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Authors_AuthorId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_AuthorId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Products");
        }
    }
}
