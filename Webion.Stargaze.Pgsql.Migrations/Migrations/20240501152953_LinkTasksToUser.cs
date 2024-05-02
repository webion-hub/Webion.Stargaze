using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webion.Stargaze.Pgsql.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class LinkTasksToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_time_entry_user_id",
                schema: "time_tracking",
                table: "time_entry",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_time_entry_user_user_id",
                schema: "time_tracking",
                table: "time_entry",
                column: "user_id",
                principalSchema: "identity",
                principalTable: "user",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_time_entry_user_user_id",
                schema: "time_tracking",
                table: "time_entry");

            migrationBuilder.DropIndex(
                name: "ix_time_entry_user_id",
                schema: "time_tracking",
                table: "time_entry");
        }
    }
}
