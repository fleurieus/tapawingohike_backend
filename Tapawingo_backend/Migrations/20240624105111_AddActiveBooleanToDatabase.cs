using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tapawingo_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddActiveBooleanToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Routes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Routes");
        }
    }
}
