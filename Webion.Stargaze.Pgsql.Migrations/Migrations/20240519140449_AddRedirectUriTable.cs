using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webion.Stargaze.Pgsql.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddRedirectUriTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "redirect_uri",
                schema: "connect",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    client_id = table.Column<Guid>(type: "uuid", nullable: false),
                    uri = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    match = table.Column<string>(type: "text", nullable: false),
                    kind = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_redirect_uri", x => x.id);
                    table.ForeignKey(
                        name: "fk_redirect_uri_client_client_id",
                        column: x => x.client_id,
                        principalSchema: "connect",
                        principalTable: "client",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_redirect_uri_client_id",
                schema: "connect",
                table: "redirect_uri",
                column: "client_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "redirect_uri",
                schema: "connect");
        }
    }
}
