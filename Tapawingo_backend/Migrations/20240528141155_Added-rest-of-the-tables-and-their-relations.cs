using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Tapawingo_backend.Migrations
{
    /// <inheritdoc />
    public partial class Addedrestofthetablesandtheirrelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Editions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Editions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Editions_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    EditionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Routes_Editions_EditionId",
                        column: x => x.EditionId,
                        principalTable: "Editions",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Code = table.Column<string>(type: "longtext", nullable: false),
                    ContactName = table.Column<string>(type: "longtext", nullable: false),
                    ContactEmail = table.Column<string>(type: "longtext", nullable: false),
                    ContactPhone = table.Column<string>(type: "longtext", nullable: false),
                    Online = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EditionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Editions_EditionId",
                        column: x => x.EditionId,
                        principalTable: "Editions",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Routeparts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    RouteType = table.Column<string>(type: "longtext", nullable: false),
                    RoutepartZoom = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RoutepartFullscreen = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Final = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TWRouteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routeparts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Routeparts_Routes_TWRouteId",
                        column: x => x.TWRouteId,
                        principalTable: "Routes",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Locationlogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Latitude = table.Column<double>(type: "double", nullable: false),
                    Longitude = table.Column<double>(type: "double", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locationlogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locationlogs_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Destinations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Latitude = table.Column<double>(type: "double", nullable: false),
                    Longitude = table.Column<double>(type: "double", nullable: false),
                    Radius = table.Column<int>(type: "int", nullable: false),
                    DestinationType = table.Column<string>(type: "longtext", nullable: false),
                    ConfirmByUser = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HideForUser = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RoutepartId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Destinations_Routeparts_RoutepartId",
                        column: x => x.RoutepartId,
                        principalTable: "Routeparts",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    File = table.Column<string>(type: "longtext", nullable: false),
                    Category = table.Column<string>(type: "longtext", nullable: false),
                    RoutepartId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_Routeparts_RoutepartId",
                        column: x => x.RoutepartId,
                        principalTable: "Routeparts",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TeamRouteparts",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    RoutepartId = table.Column<int>(type: "int", nullable: false),
                    IsFinished = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CompletedTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamRouteparts", x => new { x.TeamId, x.RoutepartId });
                    table.ForeignKey(
                        name: "FK_TeamRouteparts_Routeparts_RoutepartId",
                        column: x => x.RoutepartId,
                        principalTable: "Routeparts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamRouteparts_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_RoutepartId",
                table: "Destinations",
                column: "RoutepartId");

            migrationBuilder.CreateIndex(
                name: "IX_Editions_EventId",
                table: "Editions",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_RoutepartId",
                table: "Files",
                column: "RoutepartId");

            migrationBuilder.CreateIndex(
                name: "IX_Locationlogs_TeamId",
                table: "Locationlogs",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Routeparts_TWRouteId",
                table: "Routeparts",
                column: "TWRouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_EditionId",
                table: "Routes",
                column: "EditionId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamRouteparts_RoutepartId",
                table: "TeamRouteparts",
                column: "RoutepartId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_EditionId",
                table: "Teams",
                column: "EditionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Destinations");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Locationlogs");

            migrationBuilder.DropTable(
                name: "TeamRouteparts");

            migrationBuilder.DropTable(
                name: "Routeparts");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "Editions");
        }
    }
}
