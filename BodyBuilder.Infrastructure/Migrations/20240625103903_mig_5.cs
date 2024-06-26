using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BodyBuilder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig_5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PicureUrl",
                table: "BodyParts",
                newName: "PictureUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PictureUrl",
                table: "BodyParts",
                newName: "PicureUrl");
        }
    }
}
