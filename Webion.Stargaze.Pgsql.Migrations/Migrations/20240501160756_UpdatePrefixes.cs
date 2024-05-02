using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webion.Stargaze.Pgsql.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePrefixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_id",
                schema: "time_tracking",
                table: "time_entry");

            migrationBuilder.DropCheckConstraint(
                name: "CK_id",
                schema: "connect",
                table: "refresh_token");

            migrationBuilder.DropCheckConstraint(
                name: "CK_id",
                schema: "connect",
                table: "api_key");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                schema: "time_tracking",
                table: "time_entry",
                type: "text",
                nullable: false,
                defaultValueSql: "typeid_generate_text('timeentry')",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValueSql: "typeid_generate_text('time_entry')");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                schema: "connect",
                table: "refresh_token",
                type: "text",
                nullable: false,
                defaultValueSql: "typeid_generate_text('refreshtoken')",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValueSql: "typeid_generate_text('refresh_token')");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                schema: "connect",
                table: "api_key",
                type: "text",
                nullable: false,
                defaultValueSql: "typeid_generate_text('apikey')",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValueSql: "typeid_generate_text('api_key')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_id",
                schema: "time_tracking",
                table: "time_entry",
                sql: "typeid_check_text(id, 'timeentry')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_id",
                schema: "connect",
                table: "refresh_token",
                sql: "typeid_check_text(id, 'refreshtoken')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_id",
                schema: "connect",
                table: "api_key",
                sql: "typeid_check_text(id, 'apikey')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_id",
                schema: "time_tracking",
                table: "time_entry");

            migrationBuilder.DropCheckConstraint(
                name: "CK_id",
                schema: "connect",
                table: "refresh_token");

            migrationBuilder.DropCheckConstraint(
                name: "CK_id",
                schema: "connect",
                table: "api_key");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                schema: "time_tracking",
                table: "time_entry",
                type: "text",
                nullable: false,
                defaultValueSql: "typeid_generate_text('time_entry')",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValueSql: "typeid_generate_text('timeentry')");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                schema: "connect",
                table: "refresh_token",
                type: "text",
                nullable: false,
                defaultValueSql: "typeid_generate_text('refresh_token')",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValueSql: "typeid_generate_text('refreshtoken')");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                schema: "connect",
                table: "api_key",
                type: "text",
                nullable: false,
                defaultValueSql: "typeid_generate_text('api_key')",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValueSql: "typeid_generate_text('apikey')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_id",
                schema: "time_tracking",
                table: "time_entry",
                sql: "typeid_check_text(id, 'time_entry')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_id",
                schema: "connect",
                table: "refresh_token",
                sql: "typeid_check_text(id, 'refresh_token')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_id",
                schema: "connect",
                table: "api_key",
                sql: "typeid_check_text(id, 'api_key')");
        }
    }
}
