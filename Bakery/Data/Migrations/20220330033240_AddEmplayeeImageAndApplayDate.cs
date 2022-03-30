using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bakery.Data.Migrations
{
    public partial class AddEmplayeeImageAndApplayDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApplayDate",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Employees",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplayDate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Employees");
        }
    }
}
