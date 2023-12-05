using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FieldRent.Migrations
{
    /// <inheritdoc />
    public partial class NewColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapRequest_Maps_MapsMapId",
                table: "MapRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_Maps_Users_UserId",
                table: "Maps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Maps",
                table: "Maps");

            migrationBuilder.RenameTable(
                name: "Maps",
                newName: "Map");

            migrationBuilder.RenameIndex(
                name: "IX_Maps_UserId",
                table: "Map",
                newName: "IX_Map_UserId");

            migrationBuilder.AddColumn<int>(
                name: "FieldId",
                table: "Map",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Map",
                table: "Map",
                column: "MapId");

            migrationBuilder.CreateTable(
                name: "Field",
                columns: table => new
                {
                    FieldId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MapCoordinate = table.Column<string>(type: "TEXT", nullable: true),
                    MapCondition = table.Column<string>(type: "TEXT", nullable: true),
                    MapUrl = table.Column<string>(type: "TEXT", nullable: true),
                    MapIsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Field", x => x.FieldId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Map_FieldId",
                table: "Map",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "Field");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Map",
                table: "Map");

            migrationBuilder.DropIndex(
                name: "IX_Map_FieldId",
                table: "Map");

            migrationBuilder.DropColumn(
                name: "FieldId",
                table: "Map");

            migrationBuilder.RenameTable(
                name: "Map",
                newName: "Maps");

            migrationBuilder.RenameIndex(
                name: "IX_Map_UserId",
                table: "Maps",
                newName: "IX_Maps_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Maps",
                table: "Maps",
                column: "MapId");

            migrationBuilder.AddForeignKey(
                name: "FK_MapRequest_Maps_MapsMapId",
                table: "MapRequest",
                column: "MapsMapId",
                principalTable: "Maps",
                principalColumn: "MapId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Maps_Users_UserId",
                table: "Maps",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
