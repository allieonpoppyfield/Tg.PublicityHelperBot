using Microsoft.EntityFrameworkCore.Migrations;

namespace Tg.PublicityHelperBot.Migrations
{
    public partial class IdReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ChatID",
                table: "TgUsers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatID",
                table: "TgUsers");
        }
    }
}
