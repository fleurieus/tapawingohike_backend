using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tapawingo_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddedAttributesToSafeFileOnTableFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Files",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "Files",
                type: "longblob",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UploadDate",
                table: "Files",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Data",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "UploadDate",
                table: "Files");
        }
    }
}
