using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Webion.Stargaze.Pgsql.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "connect");

            migrationBuilder.EnsureSchema(
                name: "accounting");

            migrationBuilder.EnsureSchema(
                name: "core");

            migrationBuilder.EnsureSchema(
                name: "media");

            migrationBuilder.EnsureSchema(
                name: "projects");

            migrationBuilder.EnsureSchema(
                name: "identity");

            migrationBuilder.EnsureSchema(
                name: "time_tracking");

            migrationBuilder.CreateTable(
                name: "bank_account",
                schema: "accounting",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bank_account", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "client",
                schema: "connect",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    secret = table.Column<byte[]>(type: "bytea", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "company",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_company", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "file",
                schema: "media",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    path = table.Column<string>(type: "text", nullable: false),
                    provider = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_file", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "payment_category",
                schema: "accounting",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    description = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_payment_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                schema: "identity",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "time_package",
                schema: "time_tracking",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    hours = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_time_package", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                schema: "identity",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    security_stamp = table.Column<string>(type: "text", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "api_key",
                schema: "connect",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    client_id = table.Column<Guid>(type: "uuid", nullable: false),
                    secret = table.Column<byte[]>(type: "bytea", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_api_key", x => x.id);
                    table.ForeignKey(
                        name: "fk_api_key_clients_client_id",
                        column: x => x.client_id,
                        principalSchema: "connect",
                        principalTable: "client",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invoice",
                schema: "accounting",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    issued_by_id = table.Column<Guid>(type: "uuid", nullable: true),
                    issued_to_id = table.Column<Guid>(type: "uuid", nullable: true),
                    net_price = table.Column<decimal>(type: "numeric", nullable: false),
                    taxed_price = table.Column<decimal>(type: "numeric", nullable: false),
                    vat_percentage = table.Column<decimal>(type: "numeric", nullable: false),
                    paid = table.Column<bool>(type: "boolean", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    emitted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    expires_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoice", x => x.id);
                    table.ForeignKey(
                        name: "fk_invoice_companies_issued_by_id",
                        column: x => x.issued_by_id,
                        principalSchema: "core",
                        principalTable: "company",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_invoice_companies_issued_to_id",
                        column: x => x.issued_to_id,
                        principalSchema: "core",
                        principalTable: "company",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "project",
                schema: "projects",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    company_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    description = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project", x => x.id);
                    table.ForeignKey(
                        name: "fk_project_company_company_id",
                        column: x => x.company_id,
                        principalSchema: "core",
                        principalTable: "company",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "role_claim",
                schema: "identity",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_claim", x => x.id);
                    table.ForeignKey(
                        name: "fk_role_claim_asp_net_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "identity",
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refresh_token",
                schema: "connect",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    secret = table.Column<byte[]>(type: "bytea", maxLength: 256, nullable: false),
                    expires_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_refresh_token", x => x.id);
                    table.ForeignKey(
                        name: "fk_refresh_token_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "time_package_rate",
                schema: "time_tracking",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    time_package_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rate = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_time_package_rate", x => x.id);
                    table.ForeignKey(
                        name: "fk_time_package_rate_time_package_time_package_id",
                        column: x => x.time_package_id,
                        principalSchema: "time_tracking",
                        principalTable: "time_package",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_time_package_rate_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "user_claim",
                schema: "identity",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_claim", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_claim_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_login",
                schema: "identity",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    provider_key = table.Column<string>(type: "text", nullable: false),
                    provider_display_name = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_login", x => new { x.login_provider, x.provider_key });
                    table.ForeignKey(
                        name: "fk_user_login_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_role",
                schema: "identity",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_role", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_user_role_role_role_id",
                        column: x => x.role_id,
                        principalSchema: "identity",
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_role_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_token",
                schema: "identity",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    issued_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    expires_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_token", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "fk_user_token_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invoice_document",
                columns: table => new
                {
                    documents_id = table.Column<Guid>(type: "uuid", nullable: false),
                    invoice_dbo_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoice_document", x => new { x.documents_id, x.invoice_dbo_id });
                    table.ForeignKey(
                        name: "fk_invoice_document_file_dbo_documents_id",
                        column: x => x.documents_id,
                        principalSchema: "media",
                        principalTable: "file",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_invoice_document_invoice_invoice_dbo_id",
                        column: x => x.invoice_dbo_id,
                        principalSchema: "accounting",
                        principalTable: "invoice",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invoice_item",
                schema: "accounting",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    invoice_id = table.Column<Guid>(type: "uuid", nullable: false),
                    total_units = table.Column<decimal>(type: "numeric", nullable: false),
                    description = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    net_price = table.Column<decimal>(type: "numeric", nullable: false),
                    taxed_price = table.Column<decimal>(type: "numeric", nullable: false),
                    vat_percentage = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoice_item", x => x.id);
                    table.ForeignKey(
                        name: "fk_invoice_item_invoice_invoice_id",
                        column: x => x.invoice_id,
                        principalSchema: "accounting",
                        principalTable: "invoice",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "payment",
                schema: "accounting",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    invoice_id = table.Column<Guid>(type: "uuid", nullable: true),
                    bank_account_id = table.Column<Guid>(type: "uuid", nullable: true),
                    category_id = table.Column<Guid>(type: "uuid", nullable: true),
                    description = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    net_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    taxed_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    vat_percentage = table.Column<decimal>(type: "numeric", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    paid_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_payment", x => x.id);
                    table.ForeignKey(
                        name: "fk_payment_bank_account_bank_account_id",
                        column: x => x.bank_account_id,
                        principalSchema: "accounting",
                        principalTable: "bank_account",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_payment_invoice_invoice_id",
                        column: x => x.invoice_id,
                        principalSchema: "accounting",
                        principalTable: "invoice",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_payment_payment_category_category_id",
                        column: x => x.category_id,
                        principalSchema: "accounting",
                        principalTable: "payment_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "time_invoice",
                schema: "accounting",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    invoice_id = table.Column<Guid>(type: "uuid", nullable: false),
                    time_package_id = table.Column<Guid>(type: "uuid", nullable: false),
                    invoiced_time = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_time_invoice", x => x.id);
                    table.ForeignKey(
                        name: "fk_time_invoice_invoice_invoice_id",
                        column: x => x.invoice_id,
                        principalSchema: "accounting",
                        principalTable: "invoice",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_time_invoice_time_packages_time_package_id",
                        column: x => x.time_package_id,
                        principalSchema: "time_tracking",
                        principalTable: "time_package",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "task",
                schema: "projects",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    description = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_task", x => x.id);
                    table.ForeignKey(
                        name: "fk_task_project_project_id",
                        column: x => x.project_id,
                        principalSchema: "projects",
                        principalTable: "project",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "time_package_project",
                columns: table => new
                {
                    applies_to_id = table.Column<Guid>(type: "uuid", nullable: false),
                    time_packages_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_time_package_project", x => new { x.applies_to_id, x.time_packages_id });
                    table.ForeignKey(
                        name: "fk_time_package_project_project_applies_to_id",
                        column: x => x.applies_to_id,
                        principalSchema: "projects",
                        principalTable: "project",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_time_package_project_time_package_time_packages_id",
                        column: x => x.time_packages_id,
                        principalSchema: "time_tracking",
                        principalTable: "time_package",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "time_entry",
                schema: "time_tracking",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    task_id = table.Column<Guid>(type: "uuid", nullable: true),
                    description = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    start = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    locked = table.Column<bool>(type: "boolean", nullable: false),
                    billable = table.Column<bool>(type: "boolean", nullable: false),
                    billed = table.Column<bool>(type: "boolean", nullable: false),
                    paid = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_time_entry", x => x.id);
                    table.ForeignKey(
                        name: "fk_time_entry_task_task_id",
                        column: x => x.task_id,
                        principalSchema: "projects",
                        principalTable: "task",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_time_entry_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "user_task",
                columns: table => new
                {
                    assignees_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tasks_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_task", x => new { x.assignees_id, x.tasks_id });
                    table.ForeignKey(
                        name: "fk_user_task_task_tasks_id",
                        column: x => x.tasks_id,
                        principalSchema: "projects",
                        principalTable: "task",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_task_user_assignees_id",
                        column: x => x.assignees_id,
                        principalSchema: "identity",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "time_entry_invoice",
                schema: "accounting",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    time_invoice_id = table.Column<Guid>(type: "uuid", nullable: false),
                    time_entry_id = table.Column<Guid>(type: "uuid", nullable: false),
                    billed_time = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_time_entry_invoice", x => x.id);
                    table.ForeignKey(
                        name: "fk_time_entry_invoice_time_entries_time_entry_id",
                        column: x => x.time_entry_id,
                        principalSchema: "time_tracking",
                        principalTable: "time_entry",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_time_entry_invoice_time_invoice_dbo_time_invoice_id",
                        column: x => x.time_invoice_id,
                        principalSchema: "accounting",
                        principalTable: "time_invoice",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_api_key_client_id",
                schema: "connect",
                table: "api_key",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoice_issued_by_id",
                schema: "accounting",
                table: "invoice",
                column: "issued_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoice_issued_to_id",
                schema: "accounting",
                table: "invoice",
                column: "issued_to_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoice_document_invoice_dbo_id",
                table: "invoice_document",
                column: "invoice_dbo_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoice_item_invoice_id",
                schema: "accounting",
                table: "invoice_item",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "ix_payment_bank_account_id",
                schema: "accounting",
                table: "payment",
                column: "bank_account_id");

            migrationBuilder.CreateIndex(
                name: "ix_payment_category_id",
                schema: "accounting",
                table: "payment",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_payment_invoice_id",
                schema: "accounting",
                table: "payment",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "ix_project_company_id",
                schema: "projects",
                table: "project",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "ix_refresh_token_user_id",
                schema: "connect",
                table: "refresh_token",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "identity",
                table: "role",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_role_claim_role_id",
                schema: "identity",
                table: "role_claim",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_task_project_id",
                schema: "projects",
                table: "task",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "ix_time_entry_task_id",
                schema: "time_tracking",
                table: "time_entry",
                column: "task_id");

            migrationBuilder.CreateIndex(
                name: "ix_time_entry_user_id",
                schema: "time_tracking",
                table: "time_entry",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_time_entry_invoice_time_entry_id",
                schema: "accounting",
                table: "time_entry_invoice",
                column: "time_entry_id");

            migrationBuilder.CreateIndex(
                name: "ix_time_entry_invoice_time_invoice_id",
                schema: "accounting",
                table: "time_entry_invoice",
                column: "time_invoice_id");

            migrationBuilder.CreateIndex(
                name: "ix_time_invoice_invoice_id",
                schema: "accounting",
                table: "time_invoice",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "ix_time_invoice_time_package_id",
                schema: "accounting",
                table: "time_invoice",
                column: "time_package_id");

            migrationBuilder.CreateIndex(
                name: "ix_time_package_project_time_packages_id",
                table: "time_package_project",
                column: "time_packages_id");

            migrationBuilder.CreateIndex(
                name: "ix_time_package_rate_time_package_id",
                schema: "time_tracking",
                table: "time_package_rate",
                column: "time_package_id");

            migrationBuilder.CreateIndex(
                name: "ix_time_package_rate_user_id",
                schema: "time_tracking",
                table: "time_package_rate",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "identity",
                table: "user",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "identity",
                table: "user",
                column: "normalized_user_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_claim_user_id",
                schema: "identity",
                table: "user_claim",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_login_user_id",
                schema: "identity",
                table: "user_login",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_role_role_id",
                schema: "identity",
                table: "user_role",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_task_tasks_id",
                table: "user_task",
                column: "tasks_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "api_key",
                schema: "connect");

            migrationBuilder.DropTable(
                name: "invoice_document");

            migrationBuilder.DropTable(
                name: "invoice_item",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "payment",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "refresh_token",
                schema: "connect");

            migrationBuilder.DropTable(
                name: "role_claim",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "time_entry_invoice",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "time_package_project");

            migrationBuilder.DropTable(
                name: "time_package_rate",
                schema: "time_tracking");

            migrationBuilder.DropTable(
                name: "user_claim",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "user_login",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "user_role",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "user_task");

            migrationBuilder.DropTable(
                name: "user_token",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "client",
                schema: "connect");

            migrationBuilder.DropTable(
                name: "file",
                schema: "media");

            migrationBuilder.DropTable(
                name: "bank_account",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "payment_category",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "time_entry",
                schema: "time_tracking");

            migrationBuilder.DropTable(
                name: "time_invoice",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "role",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "task",
                schema: "projects");

            migrationBuilder.DropTable(
                name: "user",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "invoice",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "time_package",
                schema: "time_tracking");

            migrationBuilder.DropTable(
                name: "project",
                schema: "projects");

            migrationBuilder.DropTable(
                name: "company",
                schema: "core");
        }
    }
}
