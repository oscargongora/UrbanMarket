using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChicStreetwear.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddfullNamecolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Users_Addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeliveredAddress_FullName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Users_Addresses");

            migrationBuilder.DropColumn(
                name: "DeliveredAddress_FullName",
                table: "Orders");
        }
    }
}
