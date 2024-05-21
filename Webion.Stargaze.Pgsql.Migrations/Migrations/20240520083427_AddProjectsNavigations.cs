using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webion.Stargaze.Pgsql.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectsNavigations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "click_up_project_folder",
                columns: table => new
                {
                    click_up_folders_id = table.Column<string>(type: "text", nullable: false),
                    projects_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_click_up_project_folder", x => new { x.click_up_folders_id, x.projects_id });
                    table.ForeignKey(
                        name: "fk_click_up_project_folder_folder_click_up_folders_id",
                        column: x => x.click_up_folders_id,
                        principalSchema: "click_up",
                        principalTable: "folder",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_click_up_project_folder_projects_projects_id",
                        column: x => x.projects_id,
                        principalSchema: "projects",
                        principalTable: "project",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "click_up_project_space",
                columns: table => new
                {
                    click_up_spaces_id = table.Column<string>(type: "text", nullable: false),
                    projects_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_click_up_project_space", x => new { x.click_up_spaces_id, x.projects_id });
                    table.ForeignKey(
                        name: "fk_click_up_project_space_projects_projects_id",
                        column: x => x.projects_id,
                        principalSchema: "projects",
                        principalTable: "project",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_click_up_project_space_space_click_up_spaces_id",
                        column: x => x.click_up_spaces_id,
                        principalSchema: "click_up",
                        principalTable: "space",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_click_up_project_folder_projects_id",
                table: "click_up_project_folder",
                column: "projects_id");

            migrationBuilder.CreateIndex(
                name: "ix_click_up_project_space_projects_id",
                table: "click_up_project_space",
                column: "projects_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "click_up_project_folder");

            migrationBuilder.DropTable(
                name: "click_up_project_space");
        }
    }
}
