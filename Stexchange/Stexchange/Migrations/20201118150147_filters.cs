using Microsoft.EntityFrameworkCore.Migrations;

namespace Stexchange.Migrations
{
    public partial class filters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Filters",
                columns: table => new
                {
                    value = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filters", x => x.value);
                });

            migrationBuilder.CreateTable(
                name: "FilterListings",
                columns: table => new
                {
                    listing_id = table.Column<long>(type: "bigint(20) unsigned", nullable: false),
                    filter_value = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilterListings", x => new { x.listing_id, x.filter_value });
                    table.ForeignKey(
                        name: "FK_FilterListings_Listings_listing_id",
                        column: x => x.listing_id,
                        principalTable: "Listings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilterListings_Filters_filter_value",
                        column: x => x.filter_value,
                        principalTable: "Filters",
                        principalColumn: "value",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilterListings_filter_value",
                table: "FilterListings",
                column: "filter_value");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Listings_listing_id",
                table: "Images",
                column: "listing_id",
                principalTable: "Listings",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Users_user_id",
                table: "Listings",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserVerifications_Users_user_id",
                table: "UserVerifications",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Listings_listing_id",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Users_user_id",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_UserVerifications_Users_user_id",
                table: "UserVerifications");

            migrationBuilder.DropTable(
                name: "FilterListings");

            migrationBuilder.DropTable(
                name: "Filters");

            migrationBuilder.AddColumn<long>(
                name: "TempId",
                table: "Users",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TempId1",
                table: "Users",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TempId",
                table: "Listings",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_TempId",
                table: "Users",
                column: "TempId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_TempId1",
                table: "Users",
                column: "TempId1");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Listings_TempId",
                table: "Listings",
                column: "TempId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Listings_listing_id",
                table: "Images",
                column: "listing_id",
                principalTable: "Listings",
                principalColumn: "TempId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Users_user_id",
                table: "Listings",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "TempId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserVerifications_Users_user_id",
                table: "UserVerifications",
                column: "user_id",
                principalTable: "Users",
                principalColumn: "TempId1",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
