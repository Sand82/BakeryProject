using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BakeryData.Migrations
{
    public partial class AddNewPropertySubjectToMailinfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "MailInfos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subject",
                table: "MailInfos");
        }
    }
}
