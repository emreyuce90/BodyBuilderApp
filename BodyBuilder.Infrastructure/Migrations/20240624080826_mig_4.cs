using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BodyBuilder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PicureUrl",
                table: "BodyParts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PicureUrl",
                table: "BodyParts");
        }
    }
}
