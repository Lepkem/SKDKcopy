using Microsoft.EntityFrameworkCore.Migrations;

namespace Stexchange.Migrations
{
    public partial class chat_alternate_key : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Chats_ad_id_responder_id",
                table: "Chats",
                columns: new[] { "ad_id", "responder_id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Chats_ad_id_responder_id",
                table: "Chats");
        }
    }
}
