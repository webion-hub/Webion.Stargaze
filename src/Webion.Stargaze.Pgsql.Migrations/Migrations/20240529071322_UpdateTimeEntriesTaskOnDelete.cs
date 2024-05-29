using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webion.Stargaze.Pgsql.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTimeEntriesTaskOnDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_time_entry_task_task_id",
                schema: "time_tracking",
                table: "time_entry");

            migrationBuilder.AddForeignKey(
                name: "fk_time_entry_task_task_id",
                schema: "time_tracking",
                table: "time_entry",
                column: "task_id",
                principalSchema: "projects",
                principalTable: "task",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_time_entry_task_task_id",
                schema: "time_tracking",
                table: "time_entry");

            migrationBuilder.AddForeignKey(
                name: "fk_time_entry_task_task_id",
                schema: "time_tracking",
                table: "time_entry",
                column: "task_id",
                principalSchema: "projects",
                principalTable: "task",
                principalColumn: "id");
        }
    }
}
