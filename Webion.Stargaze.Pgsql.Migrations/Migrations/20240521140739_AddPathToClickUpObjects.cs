using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webion.Stargaze.Pgsql.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddPathToClickUpObjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "path",
                schema: "click_up",
                table: "space",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "path",
                schema: "click_up",
                table: "list",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "path",
                schema: "click_up",
                table: "folder",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "path",
                schema: "click_up",
                table: "space");

            migrationBuilder.DropColumn(
                name: "path",
                schema: "click_up",
                table: "list");

            migrationBuilder.DropColumn(
                name: "path",
                schema: "click_up",
                table: "folder");
        }
    }
}
