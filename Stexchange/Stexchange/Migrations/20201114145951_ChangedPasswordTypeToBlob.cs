using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Stexchange.Migrations
{
    public partial class ChangedPasswordTypeToBlob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Postal_Code",
                table: "Users",
                maxLength: 6,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "CHAR(6)",
                oldMaxLength: 6,
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Password",
                table: "Users",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                maxLength: 254,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(254)",
                oldMaxLength: 254,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "VARCHAR(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Postal_Code",
                table: "Users",
                type: "CHAR(6)",
                maxLength: 6,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 6,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "VARCHAR(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "VARCHAR(254)",
                maxLength: 254,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 254,
                oldNullable: true);
        }
    }
}
