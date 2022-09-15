using Microsoft.EntityFrameworkCore.Migrations;

namespace Back.Migrations
{
    public partial class AddingFriendshipRepo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendship_People_FriendId",
                table: "Friendship");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendship_People_UserId",
                table: "Friendship");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friendship",
                table: "Friendship");

            migrationBuilder.RenameTable(
                name: "Friendship",
                newName: "Friendships");

            migrationBuilder.RenameIndex(
                name: "IX_Friendship_UserId",
                table: "Friendships",
                newName: "IX_Friendships_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Friendship_FriendId",
                table: "Friendships",
                newName: "IX_Friendships_FriendId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friendships",
                table: "Friendships",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_People_FriendId",
                table: "Friendships",
                column: "FriendId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_People_UserId",
                table: "Friendships",
                column: "UserId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_People_FriendId",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_People_UserId",
                table: "Friendships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friendships",
                table: "Friendships");

            migrationBuilder.RenameTable(
                name: "Friendships",
                newName: "Friendship");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_UserId",
                table: "Friendship",
                newName: "IX_Friendship_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_FriendId",
                table: "Friendship",
                newName: "IX_Friendship_FriendId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friendship",
                table: "Friendship",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendship_People_FriendId",
                table: "Friendship",
                column: "FriendId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendship_People_UserId",
                table: "Friendship",
                column: "UserId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
