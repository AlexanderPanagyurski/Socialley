using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Socialley.Data.Migrations
{
    public partial class AddBaseDeletableModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "PostTag",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "PostTag",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "PostTag",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PostTag",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "PostTag",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostTag_IsDeleted",
                table: "PostTag",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PostTag_IsDeleted",
                table: "PostTag");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "PostTag");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "PostTag");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PostTag");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PostTag");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "PostTag");
        }
    }
}
