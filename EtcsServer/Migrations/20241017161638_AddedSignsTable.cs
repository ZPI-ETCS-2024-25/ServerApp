using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EtcsServer.Migrations
{
    /// <inheritdoc />
    public partial class AddedSignsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Signs",
                columns: table => new
                {
                    RailroadSignId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrackId = table.Column<int>(type: "int", nullable: false),
                    DistanceFromTrackStart = table.Column<int>(type: "int", nullable: false),
                    IsFacedUp = table.Column<bool>(type: "bit", nullable: false),
                    MaxSpeed = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Signs", x => x.RailroadSignId);
                    table.ForeignKey(
                        name: "FK_Signs_Track_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Track",
                        principalColumn: "TrackageElementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Signs_TrackId",
                table: "Signs",
                column: "TrackId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Signs");
        }
    }
}
