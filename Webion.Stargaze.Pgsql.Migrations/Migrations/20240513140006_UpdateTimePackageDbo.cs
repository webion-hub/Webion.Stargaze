using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webion.Stargaze.Pgsql.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTimePackageDbo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "company_id",
                schema: "time_tracking",
                table: "time_package",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_time_package_company_id",
                schema: "time_tracking",
                table: "time_package",
                column: "company_id");

            migrationBuilder.AddForeignKey(
                name: "fk_time_package_company_company_id",
                schema: "time_tracking",
                table: "time_package",
                column: "company_id",
                principalSchema: "core",
                principalTable: "company",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_time_package_company_company_id",
                schema: "time_tracking",
                table: "time_package");

            migrationBuilder.DropIndex(
                name: "ix_time_package_company_id",
                schema: "time_tracking",
                table: "time_package");

            migrationBuilder.DropColumn(
                name: "company_id",
                schema: "time_tracking",
                table: "time_package");
        }
    }
}
