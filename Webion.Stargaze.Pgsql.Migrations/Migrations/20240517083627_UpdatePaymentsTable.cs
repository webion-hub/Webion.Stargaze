using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webion.Stargaze.Pgsql.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePaymentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "net_amount",
                schema: "accounting",
                table: "payment");

            migrationBuilder.DropColumn(
                name: "taxed_amount",
                schema: "accounting",
                table: "payment");

            migrationBuilder.RenameColumn(
                name: "vat_percentage",
                schema: "accounting",
                table: "payment",
                newName: "amount");

            migrationBuilder.AddColumn<string>(
                name: "from",
                schema: "accounting",
                table: "payment",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "status",
                schema: "accounting",
                table: "payment",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "to",
                schema: "accounting",
                table: "payment",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "from",
                schema: "accounting",
                table: "payment");

            migrationBuilder.DropColumn(
                name: "status",
                schema: "accounting",
                table: "payment");

            migrationBuilder.DropColumn(
                name: "to",
                schema: "accounting",
                table: "payment");

            migrationBuilder.RenameColumn(
                name: "amount",
                schema: "accounting",
                table: "payment",
                newName: "vat_percentage");

            migrationBuilder.AddColumn<decimal>(
                name: "net_amount",
                schema: "accounting",
                table: "payment",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "taxed_amount",
                schema: "accounting",
                table: "payment",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
