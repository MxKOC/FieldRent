using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FieldRent.Migrations
{
    /// <inheritdoc />
    public partial class NewColumnUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MapCondition",
                table: "Field");

            migrationBuilder.RenameColumn(
                name: "MapUrl",
                table: "Field",
                newName: "MapImage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MapImage",
                table: "Field",
                newName: "MapUrl");

            migrationBuilder.AddColumn<string>(
                name: "MapCondition",
                table: "Field",
                type: "TEXT",
                nullable: true);
        }
    }
}
