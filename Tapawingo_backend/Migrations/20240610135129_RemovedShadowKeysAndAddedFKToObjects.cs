using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tapawingo_backend.Migrations
{
    /// <inheritdoc />
    public partial class RemovedShadowKeysAndAddedFKToObjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Destinations_Routeparts_RoutepartId",
                table: "Destinations");

            migrationBuilder.DropForeignKey(
                name: "FK_Editions_Events_EventId",
                table: "Editions");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Organisations_OrganisationId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Routeparts_RoutepartId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Locationlogs_Teams_TeamId",
                table: "Locationlogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Routeparts_Routes_TWRouteId",
                table: "Routeparts");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Editions_EditionId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Editions_EditionId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Routeparts_TWRouteId",
                table: "Routeparts");

            migrationBuilder.DropColumn(
                name: "TWRouteId",
                table: "Routeparts");

            migrationBuilder.AlterColumn<int>(
                name: "EditionId",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
                name: "TeamId",
                table: "Locationlogs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoutepartId",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OrganisationId",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Editions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoutepartId",
                table: "Destinations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Routeparts_RouteId",
                table: "Routeparts",
                column: "RouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Destinations_Routeparts_RoutepartId",
                table: "Destinations",
                column: "RoutepartId",
                principalTable: "Routeparts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Editions_Events_EventId",
                table: "Editions",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Organisations_OrganisationId",
                table: "Events",
                column: "OrganisationId",
                principalTable: "Organisations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Routeparts_RoutepartId",
                table: "Files",
                column: "RoutepartId",
                principalTable: "Routeparts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Locationlogs_Teams_TeamId",
                table: "Locationlogs",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Routeparts_Routes_RouteId",
                table: "Routeparts",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Editions_EditionId",
                table: "Routes",
                column: "EditionId",
                principalTable: "Editions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Editions_EditionId",
                table: "Teams",
                column: "EditionId",
                principalTable: "Editions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Destinations_Routeparts_RoutepartId",
                table: "Destinations");

            migrationBuilder.DropForeignKey(
                name: "FK_Editions_Events_EventId",
                table: "Editions");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Organisations_OrganisationId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Routeparts_RoutepartId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Locationlogs_Teams_TeamId",
                table: "Locationlogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Routeparts_Routes_RouteId",
                table: "Routeparts");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Editions_EditionId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Editions_EditionId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Routeparts_RouteId",
                table: "Routeparts");

            migrationBuilder.DropColumn(
                name: "RouteId",
                table: "Routeparts");

            migrationBuilder.AlterColumn<int>(
                name: "EditionId",
                table: "Teams",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "EditionId",
                table: "Routes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TWRouteId",
                table: "Routeparts",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "Locationlogs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RoutepartId",
                table: "Files",
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

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Editions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RoutepartId",
                table: "Destinations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Routeparts_TWRouteId",
                table: "Routeparts",
                column: "TWRouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Destinations_Routeparts_RoutepartId",
                table: "Destinations",
                column: "RoutepartId",
                principalTable: "Routeparts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Editions_Events_EventId",
                table: "Editions",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Organisations_OrganisationId",
                table: "Events",
                column: "OrganisationId",
                principalTable: "Organisations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Routeparts_RoutepartId",
                table: "Files",
                column: "RoutepartId",
                principalTable: "Routeparts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Locationlogs_Teams_TeamId",
                table: "Locationlogs",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Routeparts_Routes_TWRouteId",
                table: "Routeparts",
                column: "TWRouteId",
                principalTable: "Routes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Editions_EditionId",
                table: "Routes",
                column: "EditionId",
                principalTable: "Editions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Editions_EditionId",
                table: "Teams",
                column: "EditionId",
                principalTable: "Editions",
                principalColumn: "Id");
        }
    }
}
