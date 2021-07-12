using Microsoft.EntityFrameworkCore.Migrations;

namespace PasswordFinalProject.Migrations
{
    public partial class addfielduser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "fn_user",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "fn_user");
        }
    }
}
