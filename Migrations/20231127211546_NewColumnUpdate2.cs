using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FieldRent.Migrations
{
    /// <inheritdoc />
    public partial class NewColumnUpdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MapIsActive",
                table: "Field",
                newName: "FieldIsActive");

            migrationBuilder.RenameColumn(
                name: "MapImage",
                table: "Field",
                newName: "FieldImage");

            migrationBuilder.RenameColumn(
                name: "MapCoordinate",
                table: "Field",
                newName: "FieldCoordinate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FieldIsActive",
                table: "Field",
                newName: "MapIsActive");

            migrationBuilder.RenameColumn(
                name: "FieldImage",
                table: "Field",
                newName: "MapImage");

            migrationBuilder.RenameColumn(
                name: "FieldCoordinate",
                table: "Field",
                newName: "MapCoordinate");
        }
    }
}
