using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webion.Stargaze.Pgsql.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class WriteTypeIdsInCamelCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "id",
                schema: "time_tracking",
                table: "time_entry",
                type: "text",
                nullable: false,
                defaultValueSql: "typeid_generate_text('timeEntry')",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValueSql: "typeid_generate_text('timeentry')");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                schema: "connect",
                table: "refresh_token",
                type: "text",
                nullable: false,
                defaultValueSql: "typeid_generate_text('refreshToken')",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValueSql: "typeid_generate_text('refreshtoken')");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                schema: "connect",
                table: "api_key",
                type: "text",
                nullable: false,
                defaultValueSql: "typeid_generate_text('apiKey')",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValueSql: "typeid_generate_text('apikey')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "id",
                schema: "time_tracking",
                table: "time_entry",
                type: "text",
                nullable: false,
                defaultValueSql: "typeid_generate_text('timeentry')",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValueSql: "typeid_generate_text('timeEntry')");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                schema: "connect",
                table: "refresh_token",
                type: "text",
                nullable: false,
                defaultValueSql: "typeid_generate_text('refreshtoken')",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValueSql: "typeid_generate_text('refreshToken')");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                schema: "connect",
                table: "api_key",
                type: "text",
                nullable: false,
                defaultValueSql: "typeid_generate_text('apikey')",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValueSql: "typeid_generate_text('apiKey')");
        }
    }
}
