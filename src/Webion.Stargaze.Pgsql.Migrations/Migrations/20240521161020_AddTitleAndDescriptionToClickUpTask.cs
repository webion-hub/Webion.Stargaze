using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webion.Stargaze.Pgsql.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddTitleAndDescriptionToClickUpTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                schema: "click_up",
                table: "task",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "title",
                schema: "click_up",
                table: "task",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                schema: "click_up",
                table: "task");

            migrationBuilder.DropColumn(
                name: "title",
                schema: "click_up",
                table: "task");
        }
    }
}
