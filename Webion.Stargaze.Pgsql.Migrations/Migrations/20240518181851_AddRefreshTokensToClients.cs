using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webion.Stargaze.Pgsql.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokensToClients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "client_id",
                schema: "connect",
                table: "refresh_token",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_refresh_token_client_id",
                schema: "connect",
                table: "refresh_token",
                column: "client_id");

            migrationBuilder.AddForeignKey(
                name: "fk_refresh_token_client_client_id",
                schema: "connect",
                table: "refresh_token",
                column: "client_id",
                principalSchema: "connect",
                principalTable: "client",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_refresh_token_client_client_id",
                schema: "connect",
                table: "refresh_token");

            migrationBuilder.DropIndex(
                name: "ix_refresh_token_client_id",
                schema: "connect",
                table: "refresh_token");

            migrationBuilder.DropColumn(
                name: "client_id",
                schema: "connect",
                table: "refresh_token");
        }
    }
}
