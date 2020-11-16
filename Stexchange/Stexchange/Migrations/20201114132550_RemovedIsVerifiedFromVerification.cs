using Microsoft.EntityFrameworkCore.Migrations;

namespace Stexchange.Migrations
{
    public partial class RemovedIsVerifiedFromVerification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "UserVerifications");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "UserVerifications",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
