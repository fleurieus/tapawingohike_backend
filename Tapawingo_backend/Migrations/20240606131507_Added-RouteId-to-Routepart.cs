using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tapawingo_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddedRouteIdtoRoutepart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Organisations_OrganisationId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Editions_EditionId",
                table: "Routes");

            migrationBuilder.AlterColumn<int>(
                name: "EditionId",
                table: "Routes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RouteId",
                table: "Routeparts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "OrganisationId",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Organisations_OrganisationId",
                table: "Events",
                column: "OrganisationId",
                principalTable: "Organisations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Editions_EditionId",
                table: "Routes",
                column: "EditionId",
                principalTable: "Editions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Organisations_OrganisationId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Editions_EditionId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "RouteId",
                table: "Routeparts");

            migrationBuilder.AlterColumn<int>(
                name: "EditionId",
                table: "Routes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "OrganisationId",
                table: "Events",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Organisations_OrganisationId",
                table: "Events",
                column: "OrganisationId",
                principalTable: "Organisations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Editions_EditionId",
                table: "Routes",
                column: "EditionId",
                principalTable: "Editions",
                principalColumn: "Id");
        }
    }
}
