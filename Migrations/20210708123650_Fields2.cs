using Microsoft.EntityFrameworkCore.Migrations;

namespace Tg.PublicityHelperBot.Migrations
{
    public partial class Fields2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channel_TgUsers_OwnerId",
                table: "Channel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Channel",
                table: "Channel");

            migrationBuilder.RenameTable(
                name: "Channel",
                newName: "Channels");

            migrationBuilder.RenameIndex(
                name: "IX_Channel_OwnerId",
                table: "Channels",
                newName: "IX_Channels_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Channels",
                table: "Channels",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_TgUsers_OwnerId",
                table: "Channels",
                column: "OwnerId",
                principalTable: "TgUsers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channels_TgUsers_OwnerId",
                table: "Channels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Channels",
                table: "Channels");

            migrationBuilder.RenameTable(
                name: "Channels",
                newName: "Channel");

            migrationBuilder.RenameIndex(
                name: "IX_Channels_OwnerId",
                table: "Channel",
                newName: "IX_Channel_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Channel",
                table: "Channel",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_TgUsers_OwnerId",
                table: "Channel",
                column: "OwnerId",
                principalTable: "TgUsers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
