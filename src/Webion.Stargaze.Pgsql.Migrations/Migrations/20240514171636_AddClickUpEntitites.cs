using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webion.Stargaze.Pgsql.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddClickUpEntitites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_folder_click_up_space_dbo_space_id",
                schema: "click_up",
                table: "folder");

            migrationBuilder.DropForeignKey(
                name: "fk_list_click_up_space_dbo_space_id",
                schema: "click_up",
                table: "list");

            migrationBuilder.RenameIndex(
                name: "ix_time_entry_task_id",
                schema: "time_tracking",
                table: "time_entry",
                newName: "ix_time_entry_task_id1");

            migrationBuilder.AddColumn<string>(
                name: "folder_id",
                schema: "click_up",
                table: "list",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "time_entry",
                schema: "click_up",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    task_id = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_time_entry", x => x.id);
                    table.ForeignKey(
                        name: "fk_time_entry_task_task_id",
                        column: x => x.task_id,
                        principalSchema: "click_up",
                        principalTable: "task",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_list_folder_id",
                schema: "click_up",
                table: "list",
                column: "folder_id");

            migrationBuilder.CreateIndex(
                name: "ix_time_entry_task_id",
                schema: "click_up",
                table: "time_entry",
                column: "task_id");

            migrationBuilder.AddForeignKey(
                name: "fk_folder_click_up_spaces_space_id",
                schema: "click_up",
                table: "folder",
                column: "space_id",
                principalSchema: "click_up",
                principalTable: "space",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_list_click_up_spaces_space_id",
                schema: "click_up",
                table: "list",
                column: "space_id",
                principalSchema: "click_up",
                principalTable: "space",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_list_folder_folder_id",
                schema: "click_up",
                table: "list",
                column: "folder_id",
                principalSchema: "click_up",
                principalTable: "folder",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_folder_click_up_spaces_space_id",
                schema: "click_up",
                table: "folder");

            migrationBuilder.DropForeignKey(
                name: "fk_list_click_up_spaces_space_id",
                schema: "click_up",
                table: "list");

            migrationBuilder.DropForeignKey(
                name: "fk_list_folder_folder_id",
                schema: "click_up",
                table: "list");

            migrationBuilder.DropTable(
                name: "time_entry",
                schema: "click_up");

            migrationBuilder.DropIndex(
                name: "ix_list_folder_id",
                schema: "click_up",
                table: "list");

            migrationBuilder.DropColumn(
                name: "folder_id",
                schema: "click_up",
                table: "list");

            migrationBuilder.RenameIndex(
                name: "ix_time_entry_task_id1",
                schema: "time_tracking",
                table: "time_entry",
                newName: "ix_time_entry_task_id");

            migrationBuilder.AddForeignKey(
                name: "fk_folder_click_up_space_dbo_space_id",
                schema: "click_up",
                table: "folder",
                column: "space_id",
                principalSchema: "click_up",
                principalTable: "space",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_list_click_up_space_dbo_space_id",
                schema: "click_up",
                table: "list",
                column: "space_id",
                principalSchema: "click_up",
                principalTable: "space",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
