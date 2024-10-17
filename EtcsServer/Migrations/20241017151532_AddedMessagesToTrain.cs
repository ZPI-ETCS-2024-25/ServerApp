using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EtcsServer.Migrations
{
    /// <inheritdoc />
    public partial class AddedMessagesToTrain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trains_Messages_MessageId",
                table: "Trains");

            migrationBuilder.DropIndex(
                name: "IX_Trains_MessageId",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "Trains");

            migrationBuilder.CreateTable(
                name: "MessageTrain",
                columns: table => new
                {
                    MessagesMessageId = table.Column<int>(type: "int", nullable: false),
                    ReceiversTrainId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageTrain", x => new { x.MessagesMessageId, x.ReceiversTrainId });
                    table.ForeignKey(
                        name: "FK_MessageTrain_Messages_MessagesMessageId",
                        column: x => x.MessagesMessageId,
                        principalTable: "Messages",
                        principalColumn: "MessageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageTrain_Trains_ReceiversTrainId",
                        column: x => x.ReceiversTrainId,
                        principalTable: "Trains",
                        principalColumn: "TrainId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageTrain_ReceiversTrainId",
                table: "MessageTrain",
                column: "ReceiversTrainId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageTrain");

            migrationBuilder.AddColumn<int>(
                name: "MessageId",
                table: "Trains",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trains_MessageId",
                table: "Trains",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trains_Messages_MessageId",
                table: "Trains",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "MessageId");
        }
    }
}
