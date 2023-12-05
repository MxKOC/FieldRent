using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FieldRent.Migrations
{
    /// <inheritdoc />
    public partial class NewColumnUpdate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Map_Field_FieldId",
                table: "Map");

            migrationBuilder.DropForeignKey(
                name: "FK_Map_Users_UserId",
                table: "Map");

            migrationBuilder.DropForeignKey(
                name: "FK_MapRequest_Map_MapsMapId",
                table: "MapRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Map",
                table: "Map");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Field",
                table: "Field");

            migrationBuilder.RenameTable(
                name: "Map",
                newName: "Maps");

            migrationBuilder.RenameTable(
                name: "Field",
                newName: "Fields");

            migrationBuilder.RenameIndex(
                name: "IX_Map_UserId",
                table: "Maps",
                newName: "IX_Maps_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Map_FieldId",
                table: "Maps",
                newName: "IX_Maps_FieldId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Maps",
                table: "Maps",
                column: "MapId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fields",
                table: "Fields",
                column: "FieldId");

            migrationBuilder.AddForeignKey(
                name: "FK_MapRequest_Maps_MapsMapId",
                table: "MapRequest",
                column: "MapsMapId",
                principalTable: "Maps",
                principalColumn: "MapId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Maps_Fields_FieldId",
                table: "Maps",
                column: "FieldId",
                principalTable: "Fields",
                principalColumn: "FieldId");

            migrationBuilder.AddForeignKey(
                name: "FK_Maps_Users_UserId",
                table: "Maps",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapRequest_Maps_MapsMapId",
                table: "MapRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_Maps_Fields_FieldId",
                table: "Maps");

            migrationBuilder.DropForeignKey(
                name: "FK_Maps_Users_UserId",
                table: "Maps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Maps",
                table: "Maps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fields",
                table: "Fields");

            migrationBuilder.RenameTable(
                name: "Maps",
                newName: "Map");

            migrationBuilder.RenameTable(
                name: "Fields",
                newName: "Field");

            migrationBuilder.RenameIndex(
                name: "IX_Maps_UserId",
                table: "Map",
                newName: "IX_Map_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Maps_FieldId",
                table: "Map",
                newName: "IX_Map_FieldId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Map",
                table: "Map",
                column: "MapId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Field",
                table: "Field",
                column: "FieldId");

            migrationBuilder.AddForeignKey(
                name: "FK_Map_Field_FieldId",
                table: "Map",
                column: "FieldId",
                principalTable: "Field",
                principalColumn: "FieldId");

            migrationBuilder.AddForeignKey(
                name: "FK_Map_Users_UserId",
                table: "Map",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MapRequest_Map_MapsMapId",
                table: "MapRequest",
                column: "MapsMapId",
                principalTable: "Map",
                principalColumn: "MapId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
