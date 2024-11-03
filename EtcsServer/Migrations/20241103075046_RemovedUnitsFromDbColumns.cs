using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EtcsServer.Migrations
{
    /// <inheritdoc />
    public partial class RemovedUnitsFromDbColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaxSpeedMps",
                table: "Trains",
                newName: "MaxSpeed");

            migrationBuilder.RenameColumn(
                name: "MaxSpeedMps",
                table: "TrackSwitches",
                newName: "MaxSpeed");

            migrationBuilder.RenameColumn(
                name: "MaxUpSpeedMps",
                table: "Track",
                newName: "MaxUpSpeed");

            migrationBuilder.RenameColumn(
                name: "MaxDownSpeedMps",
                table: "Track",
                newName: "MaxDownSpeed");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaxSpeed",
                table: "Trains",
                newName: "MaxSpeedMps");

            migrationBuilder.RenameColumn(
                name: "MaxSpeed",
                table: "TrackSwitches",
                newName: "MaxSpeedMps");

            migrationBuilder.RenameColumn(
                name: "MaxUpSpeed",
                table: "Track",
                newName: "MaxUpSpeedMps");

            migrationBuilder.RenameColumn(
                name: "MaxDownSpeed",
                table: "Track",
                newName: "MaxDownSpeedMps");
        }
    }
}
