using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webion.Stargaze.Pgsql.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddClickUpSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "click_up");

            migrationBuilder.CreateTable(
                name: "space",
                schema: "click_up",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_space", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "folder",
                schema: "click_up",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    space_id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_folder", x => x.id);
                    table.ForeignKey(
                        name: "fk_folder_click_up_space_dbo_space_id",
                        column: x => x.space_id,
                        principalSchema: "click_up",
                        principalTable: "space",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "list",
                schema: "click_up",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    space_id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_list", x => x.id);
                    table.ForeignKey(
                        name: "fk_list_click_up_space_dbo_space_id",
                        column: x => x.space_id,
                        principalSchema: "click_up",
                        principalTable: "space",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "click_up_project_list",
                columns: table => new
                {
                    click_up_lists_id = table.Column<string>(type: "text", nullable: false),
                    projects_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_click_up_project_list", x => new { x.click_up_lists_id, x.projects_id });
                    table.ForeignKey(
                        name: "fk_click_up_project_list_list_click_up_lists_id",
                        column: x => x.click_up_lists_id,
                        principalSchema: "click_up",
                        principalTable: "list",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_click_up_project_list_projects_projects_id",
                        column: x => x.projects_id,
                        principalSchema: "projects",
                        principalTable: "project",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task",
                schema: "click_up",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    list_id = table.Column<string>(type: "text", nullable: false),
                    task_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_task", x => x.id);
                    table.ForeignKey(
                        name: "fk_task_list_list_id",
                        column: x => x.list_id,
                        principalSchema: "click_up",
                        principalTable: "list",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_task_tasks_task_id",
                        column: x => x.task_id,
                        principalSchema: "projects",
                        principalTable: "task",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "ix_click_up_project_list_projects_id",
                table: "click_up_project_list",
                column: "projects_id");

            migrationBuilder.CreateIndex(
                name: "ix_folder_space_id",
                schema: "click_up",
                table: "folder",
                column: "space_id");

            migrationBuilder.CreateIndex(
                name: "ix_list_space_id",
                schema: "click_up",
                table: "list",
                column: "space_id");

            migrationBuilder.CreateIndex(
                name: "ix_task_list_id",
                schema: "click_up",
                table: "task",
                column: "list_id");

            migrationBuilder.CreateIndex(
                name: "ix_task_task_id",
                schema: "click_up",
                table: "task",
                column: "task_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "click_up_project_list");

            migrationBuilder.DropTable(
                name: "folder",
                schema: "click_up");

            migrationBuilder.DropTable(
                name: "task",
                schema: "click_up");

            migrationBuilder.DropTable(
                name: "list",
                schema: "click_up");

            migrationBuilder.DropTable(
                name: "space",
                schema: "click_up");
        }
    }
}
