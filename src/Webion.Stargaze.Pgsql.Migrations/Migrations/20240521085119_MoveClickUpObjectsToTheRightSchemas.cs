using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webion.Stargaze.Pgsql.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class MoveClickUpObjectsToTheRightSchemas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "click_up_project_space",
                newName: "click_up_project_space",
                newSchema: "click_up");

            migrationBuilder.RenameTable(
                name: "click_up_project_list",
                newName: "click_up_project_list",
                newSchema: "click_up");

            migrationBuilder.RenameTable(
                name: "click_up_project_folder",
                newName: "click_up_project_folder",
                newSchema: "click_up");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "click_up_project_space",
                schema: "click_up",
                newName: "click_up_project_space");

            migrationBuilder.RenameTable(
                name: "click_up_project_list",
                schema: "click_up",
                newName: "click_up_project_list");

            migrationBuilder.RenameTable(
                name: "click_up_project_folder",
                schema: "click_up",
                newName: "click_up_project_folder");
        }
    }
}
