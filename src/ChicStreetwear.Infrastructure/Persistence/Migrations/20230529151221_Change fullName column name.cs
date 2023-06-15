using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChicStreetwear.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangefullNamecolumnname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeliveredAddress_FullName",
                table: "Orders",
                newName: "FullName");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Orders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Orders",
                newName: "DeliveredAddress_FullName");

            migrationBuilder.AlterColumn<string>(
                name: "DeliveredAddress_FullName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
