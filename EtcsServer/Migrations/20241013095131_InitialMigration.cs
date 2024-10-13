using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EtcsServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageId);
                });

            migrationBuilder.CreateTable(
                name: "RailwaySignal",
                columns: table => new
                {
                    RailwaySignalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RailwaySignal", x => x.RailwaySignalId);
                });

            migrationBuilder.CreateTable(
                name: "TrackageElement",
                columns: table => new
                {
                    TrackageElementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeftSideElementId = table.Column<int>(type: "int", nullable: true),
                    RightSideElementId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackageElement", x => x.TrackageElementId);
                    table.ForeignKey(
                        name: "FK_TrackageElement_TrackageElement_LeftSideElementId",
                        column: x => x.LeftSideElementId,
                        principalTable: "TrackageElement",
                        principalColumn: "TrackageElementId");
                    table.ForeignKey(
                        name: "FK_TrackageElement_TrackageElement_RightSideElementId",
                        column: x => x.RightSideElementId,
                        principalTable: "TrackageElement",
                        principalColumn: "TrackageElementId");
                });

            migrationBuilder.CreateTable(
                name: "Trains",
                columns: table => new
                {
                    TrainId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LengthMeters = table.Column<int>(type: "int", nullable: false),
                    WeightKilos = table.Column<int>(type: "int", nullable: false),
                    MaxSpeedMps = table.Column<int>(type: "int", nullable: false),
                    BrakeWeight = table.Column<int>(type: "int", nullable: false),
                    MessageId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trains", x => x.TrainId);
                    table.ForeignKey(
                        name: "FK_Trains_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "MessageId");
                });

            migrationBuilder.CreateTable(
                name: "Switch",
                columns: table => new
                {
                    TrackageElementId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Switch", x => x.TrackageElementId);
                    table.ForeignKey(
                        name: "FK_Switch_TrackageElement_TrackageElementId",
                        column: x => x.TrackageElementId,
                        principalTable: "TrackageElement",
                        principalColumn: "TrackageElementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Track",
                columns: table => new
                {
                    TrackageElementId = table.Column<int>(type: "int", nullable: false),
                    Length = table.Column<int>(type: "int", nullable: false),
                    MaxUpSpeedMps = table.Column<int>(type: "int", nullable: false),
                    MaxDownSpeedMps = table.Column<int>(type: "int", nullable: false),
                    Gradient = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Track", x => x.TrackageElementId);
                    table.ForeignKey(
                        name: "FK_Track_TrackageElement_TrackageElementId",
                        column: x => x.TrackageElementId,
                        principalTable: "TrackageElement",
                        principalColumn: "TrackageElementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Crossings",
                columns: table => new
                {
                    CrossingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDamaged = table.Column<bool>(type: "bit", nullable: false),
                    TrackId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crossings", x => x.CrossingId);
                    table.ForeignKey(
                        name: "FK_Crossings_Track_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Track",
                        principalColumn: "TrackageElementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrackSignals",
                columns: table => new
                {
                    RailwaySignalTrackId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrackId = table.Column<int>(type: "int", nullable: false),
                    RailwaySignalId = table.Column<int>(type: "int", nullable: false),
                    DistanceFromTrackStart = table.Column<int>(type: "int", nullable: false),
                    IsFacedUp = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackSignals", x => x.RailwaySignalTrackId);
                    table.ForeignKey(
                        name: "FK_TrackSignals_RailwaySignal_RailwaySignalId",
                        column: x => x.RailwaySignalId,
                        principalTable: "RailwaySignal",
                        principalColumn: "RailwaySignalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrackSignals_Track_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Track",
                        principalColumn: "TrackageElementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrackSwitches",
                columns: table => new
                {
                    SwitchRouteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SwitchId = table.Column<int>(type: "int", nullable: false),
                    TrackFromId = table.Column<int>(type: "int", nullable: false),
                    TrackToId = table.Column<int>(type: "int", nullable: false),
                    MaxSpeedMps = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackSwitches", x => x.SwitchRouteId);
                    table.ForeignKey(
                        name: "FK_TrackSwitches_Switch_SwitchId",
                        column: x => x.SwitchId,
                        principalTable: "Switch",
                        principalColumn: "TrackageElementId");
                    table.ForeignKey(
                        name: "FK_TrackSwitches_Track_TrackFromId",
                        column: x => x.TrackFromId,
                        principalTable: "Track",
                        principalColumn: "TrackageElementId");
                    table.ForeignKey(
                        name: "FK_TrackSwitches_Track_TrackToId",
                        column: x => x.TrackToId,
                        principalTable: "Track",
                        principalColumn: "TrackageElementId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Crossings_TrackId",
                table: "Crossings",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackageElement_LeftSideElementId",
                table: "TrackageElement",
                column: "LeftSideElementId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackageElement_RightSideElementId",
                table: "TrackageElement",
                column: "RightSideElementId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackSignals_RailwaySignalId",
                table: "TrackSignals",
                column: "RailwaySignalId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackSignals_TrackId",
                table: "TrackSignals",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackSwitches_SwitchId",
                table: "TrackSwitches",
                column: "SwitchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackSwitches_TrackFromId",
                table: "TrackSwitches",
                column: "TrackFromId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackSwitches_TrackToId",
                table: "TrackSwitches",
                column: "TrackToId");

            migrationBuilder.CreateIndex(
                name: "IX_Trains_MessageId",
                table: "Trains",
                column: "MessageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Crossings");

            migrationBuilder.DropTable(
                name: "TrackSignals");

            migrationBuilder.DropTable(
                name: "TrackSwitches");

            migrationBuilder.DropTable(
                name: "Trains");

            migrationBuilder.DropTable(
                name: "RailwaySignal");

            migrationBuilder.DropTable(
                name: "Switch");

            migrationBuilder.DropTable(
                name: "Track");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "TrackageElement");
        }
    }
}
