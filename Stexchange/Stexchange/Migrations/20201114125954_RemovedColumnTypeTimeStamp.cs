using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Stexchange.Migrations
{
    public partial class RemovedColumnTypeTimeStamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Created_At",
                table: "Users",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TimeStamp",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Created_At",
                table: "Users",
                type: "TimeStamp",
                nullable: true,
                oldClrType: typeof(DateTime));
        }
    }
}
