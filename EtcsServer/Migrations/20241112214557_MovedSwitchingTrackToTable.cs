using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EtcsServer.Migrations
{
    /// <inheritdoc />
    public partial class MovedSwitchingTrackToTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CrossingTracks",
                columns: table => new
                {
                    CrossingTrackId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CrossingId = table.Column<int>(type: "int", nullable: false),
                    TrackId = table.Column<int>(type: "int", nullable: false),
                    DistanceFromTrackStart = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrossingTracks", x => x.CrossingTrackId);
                    table.ForeignKey(
                        name: "FK_CrossingTracks_Track_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Track",
                        principalColumn: "TrackageElementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SwitchingTrack",
                columns: table => new
                {
                    TrackageElementId = table.Column<int>(type: "int", nullable: false),
                    Length = table.Column<double>(type: "float", nullable: false),
                    MaxSpeed = table.Column<double>(type: "float", nullable: false),
                    Gradient = table.Column<double>(type: "float", nullable: false),
                    TrackPosition = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SwitchingTrack", x => x.TrackageElementId);
                    table.ForeignKey(
                        name: "FK_SwitchingTrack_TrackageElement_TrackageElementId",
                        column: x => x.TrackageElementId,
                        principalTable: "TrackageElement",
                        principalColumn: "TrackageElementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CrossingTracks_TrackId",
                table: "CrossingTracks",
                column: "TrackId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CrossingTracks");

            migrationBuilder.DropTable(
                name: "SwitchingTrack");
        }
    }
}
