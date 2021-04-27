using Microsoft.EntityFrameworkCore.Migrations;

namespace WarOfHeroesUsersAPI.Migrations
{
    public partial class TestMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserHeroDeck_Users_UserId",
                table: "UserHeroDeck");

            migrationBuilder.DropForeignKey(
                name: "FK_UserHeroInventory_Users_UserId",
                table: "UserHeroInventory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserHeroInventory",
                table: "UserHeroInventory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserHeroDeck",
                table: "UserHeroDeck");

            migrationBuilder.RenameTable(
                name: "UserHeroInventory",
                newName: "UserHeroInventories");

            migrationBuilder.RenameTable(
                name: "UserHeroDeck",
                newName: "UserHeroDecks");

            migrationBuilder.RenameIndex(
                name: "IX_UserHeroInventory_UserId",
                table: "UserHeroInventories",
                newName: "IX_UserHeroInventories_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserHeroDeck_UserId",
                table: "UserHeroDecks",
                newName: "IX_UserHeroDecks_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserHeroInventories",
                table: "UserHeroInventories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserHeroDecks",
                table: "UserHeroDecks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserHeroDecks_Users_UserId",
                table: "UserHeroDecks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserHeroInventories_Users_UserId",
                table: "UserHeroInventories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserHeroDecks_Users_UserId",
                table: "UserHeroDecks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserHeroInventories_Users_UserId",
                table: "UserHeroInventories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserHeroInventories",
                table: "UserHeroInventories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserHeroDecks",
                table: "UserHeroDecks");

            migrationBuilder.RenameTable(
                name: "UserHeroInventories",
                newName: "UserHeroInventory");

            migrationBuilder.RenameTable(
                name: "UserHeroDecks",
                newName: "UserHeroDeck");

            migrationBuilder.RenameIndex(
                name: "IX_UserHeroInventories_UserId",
                table: "UserHeroInventory",
                newName: "IX_UserHeroInventory_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserHeroDecks_UserId",
                table: "UserHeroDeck",
                newName: "IX_UserHeroDeck_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserHeroInventory",
                table: "UserHeroInventory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserHeroDeck",
                table: "UserHeroDeck",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserHeroDeck_Users_UserId",
                table: "UserHeroDeck",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserHeroInventory_Users_UserId",
                table: "UserHeroInventory",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
