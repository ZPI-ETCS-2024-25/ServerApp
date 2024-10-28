using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EtcsServer.Migrations
{
    /// <inheritdoc />
    public partial class RemovedCrossingsIsDamaged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDamaged",
                table: "Crossings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDamaged",
                table: "Crossings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
