using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EtcsServer.Migrations
{
    /// <inheritdoc />
    public partial class BothTrackTypesForSwitch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackSwitches_Track_TrackFromId",
                table: "TrackSwitches");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackSwitches_Track_TrackToId",
                table: "TrackSwitches");

            migrationBuilder.DropColumn(
                name: "MaxSpeed",
                table: "TrackSwitches");

            migrationBuilder.DropColumn(
                name: "SwitchLength",
                table: "TrackSwitches");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackSwitches_TrackageElement_TrackFromId",
                table: "TrackSwitches",
                column: "TrackFromId",
                principalTable: "TrackageElement",
                principalColumn: "TrackageElementId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackSwitches_TrackageElement_TrackToId",
                table: "TrackSwitches",
                column: "TrackToId",
                principalTable: "TrackageElement",
                principalColumn: "TrackageElementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackSwitches_TrackageElement_TrackFromId",
                table: "TrackSwitches");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackSwitches_TrackageElement_TrackToId",
                table: "TrackSwitches");

            migrationBuilder.AddColumn<double>(
                name: "MaxSpeed",
                table: "TrackSwitches",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SwitchLength",
                table: "TrackSwitches",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddForeignKey(
                name: "FK_TrackSwitches_Track_TrackFromId",
                table: "TrackSwitches",
                column: "TrackFromId",
                principalTable: "Track",
                principalColumn: "TrackageElementId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackSwitches_Track_TrackToId",
                table: "TrackSwitches",
                column: "TrackToId",
                principalTable: "Track",
                principalColumn: "TrackageElementId");
        }
    }
}
