using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BodyBuilder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubProgrammeMovements_Movements_MovementId",
                table: "SubProgrammeMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_SubProgrammeMovements_SubProgrammes_SubProgrammeId",
                table: "SubProgrammeMovements");

            migrationBuilder.DropIndex(
                name: "IX_SubProgrammeMovements_MovementId",
                table: "SubProgrammeMovements");

            migrationBuilder.DropIndex(
                name: "IX_SubProgrammeMovements_SubProgrammeId",
                table: "SubProgrammeMovements");

            migrationBuilder.DropColumn(
                name: "Reps",
                table: "SubProgrammes");

            migrationBuilder.DropColumn(
                name: "Sets",
                table: "SubProgrammes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Reps",
                table: "SubProgrammes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sets",
                table: "SubProgrammes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SubProgrammeMovements_MovementId",
                table: "SubProgrammeMovements",
                column: "MovementId");

            migrationBuilder.CreateIndex(
                name: "IX_SubProgrammeMovements_SubProgrammeId",
                table: "SubProgrammeMovements",
                column: "SubProgrammeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubProgrammeMovements_Movements_MovementId",
                table: "SubProgrammeMovements",
                column: "MovementId",
                principalTable: "Movements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubProgrammeMovements_SubProgrammes_SubProgrammeId",
                table: "SubProgrammeMovements",
                column: "SubProgrammeId",
                principalTable: "SubProgrammes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
