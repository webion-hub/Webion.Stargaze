using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webion.Stargaze.Pgsql.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class RemoveConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_user_id",
                schema: "identity",
                table: "user_token");

            migrationBuilder.DropCheckConstraint(
                name: "CK_role_id",
                schema: "identity",
                table: "user_role");

            migrationBuilder.DropCheckConstraint(
                name: "CK_user_id",
                schema: "identity",
                table: "user_role");

            migrationBuilder.DropCheckConstraint(
                name: "CK_user_id",
                schema: "identity",
                table: "user_login");

            migrationBuilder.DropCheckConstraint(
                name: "CK_user_id",
                schema: "identity",
                table: "user_claim");

            migrationBuilder.DropCheckConstraint(
                name: "CK_user_id",
                schema: "time_tracking",
                table: "time_entry");

            migrationBuilder.DropCheckConstraint(
                name: "CK_id",
                schema: "projects",
                table: "task");

            migrationBuilder.DropCheckConstraint(
                name: "CK_role_id",
                schema: "identity",
                table: "role_claim");

            migrationBuilder.DropCheckConstraint(
                name: "CK_user_id",
                schema: "connect",
                table: "refresh_token");

            migrationBuilder.DropCheckConstraint(
                name: "CK_client_id",
                schema: "connect",
                table: "api_key");

            migrationBuilder.AddCheckConstraint(
                name: "CK_id",
                schema: "projects",
                table: "task",
                sql: "typeid_check_text(id, 'task')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_id",
                schema: "projects",
                table: "task");

            migrationBuilder.AddCheckConstraint(
                name: "CK_user_id",
                schema: "identity",
                table: "user_token",
                sql: "typeid_check_text(user_id, 'user')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_role_id",
                schema: "identity",
                table: "user_role",
                sql: "typeid_check_text(role_id, 'role')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_user_id",
                schema: "identity",
                table: "user_role",
                sql: "typeid_check_text(user_id, 'user')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_user_id",
                schema: "identity",
                table: "user_login",
                sql: "typeid_check_text(user_id, 'user')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_user_id",
                schema: "identity",
                table: "user_claim",
                sql: "typeid_check_text(user_id, 'user')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_user_id",
                schema: "time_tracking",
                table: "time_entry",
                sql: "typeid_check_text(user_id, 'user')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_id",
                schema: "projects",
                table: "task",
                sql: "typeid_check_text(id, 'project')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_role_id",
                schema: "identity",
                table: "role_claim",
                sql: "typeid_check_text(role_id, 'role')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_user_id",
                schema: "connect",
                table: "refresh_token",
                sql: "typeid_check_text(user_id, 'user')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_client_id",
                schema: "connect",
                table: "api_key",
                sql: "typeid_check_text(client_id, 'client')");
        }
    }
}
