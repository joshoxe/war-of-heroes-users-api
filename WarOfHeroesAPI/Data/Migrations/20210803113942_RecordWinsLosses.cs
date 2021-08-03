using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfHeroesUsersAPI.Migrations
{
    public partial class RecordWinsLosses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Coins",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Losses",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Wins",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coins",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Losses",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Wins",
                table: "Users");
        }
    }
}
