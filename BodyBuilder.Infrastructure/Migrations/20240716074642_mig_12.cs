using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BodyBuilder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig_12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SubBodyPart_BodyPartId",
                table: "SubBodyPart");

            migrationBuilder.CreateIndex(
                name: "IX_SubBodyPart_BodyPartId",
                table: "SubBodyPart",
                column: "BodyPartId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SubBodyPart_BodyPartId",
                table: "SubBodyPart");

            migrationBuilder.CreateIndex(
                name: "IX_SubBodyPart_BodyPartId",
                table: "SubBodyPart",
                column: "BodyPartId",
                unique: true);
        }
    }
}
