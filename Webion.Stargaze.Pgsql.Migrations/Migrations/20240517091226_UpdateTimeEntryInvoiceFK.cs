using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webion.Stargaze.Pgsql.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTimeEntryInvoiceFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_time_entry_invoice_time_invoice_dbo_time_invoice_id",
                schema: "accounting",
                table: "time_entry_invoice");

            migrationBuilder.AddForeignKey(
                name: "fk_time_entry_invoice_time_invoices_time_invoice_id",
                schema: "accounting",
                table: "time_entry_invoice",
                column: "time_invoice_id",
                principalSchema: "accounting",
                principalTable: "time_invoice",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_time_entry_invoice_time_invoices_time_invoice_id",
                schema: "accounting",
                table: "time_entry_invoice");

            migrationBuilder.AddForeignKey(
                name: "fk_time_entry_invoice_time_invoice_dbo_time_invoice_id",
                schema: "accounting",
                table: "time_entry_invoice",
                column: "time_invoice_id",
                principalSchema: "accounting",
                principalTable: "time_invoice",
                principalColumn: "id");
        }
    }
}
