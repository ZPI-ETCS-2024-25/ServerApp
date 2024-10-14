using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EtcsServer.Migrations
{
    /// <inheritdoc />
    public partial class RemovedTrainWeight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeightKilos",
                table: "Trains");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WeightKilos",
                table: "Trains",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
