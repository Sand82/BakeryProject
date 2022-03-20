using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bakery.Data.Migrations
{
    public partial class IsDeleteProductProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "Votes");

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "Votes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
