using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EtcsServer.Migrations
{
    /// <inheritdoc />
    public partial class AddedSwitchLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "SwitchLength",
                table: "TrackSwitches",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SwitchLength",
                table: "TrackSwitches");
        }
    }
}
