using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChicStreetwear.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class roolback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureName",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "Categories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PictureName",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
