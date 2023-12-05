using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FieldRent.Migrations
{
    /// <inheritdoc />
    public partial class dbupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestIsActive",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "RequestStart",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "RequestStop",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "MapUrl",
                table: "Maps");

            migrationBuilder.DropColumn(
                name: "FieldImage",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "FieldIsActive",
                table: "Fields");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RequestIsActive",
                table: "Requests",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestStart",
                table: "Requests",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestStop",
                table: "Requests",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "MapUrl",
                table: "Maps",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FieldImage",
                table: "Fields",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FieldIsActive",
                table: "Fields",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
