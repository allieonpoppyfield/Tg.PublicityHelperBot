using Microsoft.EntityFrameworkCore.Migrations;

namespace Tg.PublicityHelperBot.Migrations
{
    public partial class ChannelId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ChannelId",
                table: "Channels",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChannelId",
                table: "Channels");
        }
    }
}
