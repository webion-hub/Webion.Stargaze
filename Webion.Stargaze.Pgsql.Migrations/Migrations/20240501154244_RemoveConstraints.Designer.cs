﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Webion.Stargaze.Pgsql;

#nullable disable

namespace Webion.Stargaze.Pgsql.Migrations.Migrations
{
    [DbContext(typeof(StargazeDbContext))]
    [Migration("20240501154244_RemoveConstraints")]
    partial class RemoveConstraints
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Webion.Stargaze.Pgsql.Entities.Connect.ApiKeyDbo", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasColumnName("id")
                        .HasDefaultValueSql("typeid_generate_text('api_key')");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("client_id");

                    b.Property<byte[]>("Secret")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("bytea")
                        .HasColumnName("secret");

                    b.HasKey("Id")
                        .HasName("pk_api_key");

                    b.HasIndex("ClientId")
                        .HasDatabaseName("ix_api_key_client_id");

                    b.ToTable("api_key", "connect", t =>
                        {
                            t.HasCheckConstraint("CK_id", "typeid_check_text(id, 'api_key')");
                        });
                });

            modelBuilder.Entity("Webion.Stargaze.Pgsql.Entities.Connect.ClientDbo", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasColumnName("id")
                        .HasDefaultValueSql("typeid_generate_text('client')");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("name");

                    b.Property<byte[]>("Secret")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("bytea")
                        .HasColumnName("secret");

                    b.HasKey("Id")
                        .HasName("pk_client");

                    b.ToTable("client", "connect", t =>
                        {
                            t.HasCheckConstraint("CK_id", "typeid_check_text(id, 'client')");
                        });
                });

            modelBuilder.Entity("Webion.Stargaze.Pgsql.Entities.Connect.RefreshTokenDbo", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasColumnName("id")
                        .HasDefaultValueSql("typeid_generate_text('refresh_token')");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expires_at");

                    b.Property<byte[]>("Secret")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("bytea")
                        .HasColumnName("secret");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_refresh_token");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_refresh_token_user_id");

                    b.ToTable("refresh_token", "connect", t =>
                        {
                            t.HasCheckConstraint("CK_id", "typeid_check_text(id, 'refresh_token')");
                        });
                });

            modelBuilder.Entity("Webion.Stargaze.Pgsql.Entities.Identity.RoleClaimDbo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text")
                        .HasColumnName("claim_type");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text")
                        .HasColumnName("claim_value");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("role_id");

                    b.HasKey("Id")
                        .HasName("pk_role_claim");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_role_claim_role_id");

                    b.ToTable("role_claim", "identity");
                });

            modelBuilder.Entity("Webion.Stargaze.Pgsql.Entities.Identity.RoleDbo", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasColumnName("id")
                        .HasDefaultValueSql("typeid_generate_text('role')");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("text")
                        .HasColumnName("concurrency_stamp");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("text")
                        .HasColumnName("normalized_name");

                    b.HasKey("Id")
                        .HasName("pk_role");

                    b.ToTable("role", "identity", t =>
                        {
                            t.HasCheckConstraint("CK_id", "typeid_check_text(id, 'role')");
                        });
                });

            modelBuilder.Entity("Webion.Stargaze.Pgsql.Entities.Identity.UserClaimDbo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text")
                        .HasColumnName("claim_type");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text")
                        .HasColumnName("claim_value");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_claim");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_claim_user_id");

                    b.ToTable("user_claim", "identity");
                });

            modelBuilder.Entity("Webion.Stargaze.Pgsql.Entities.Identity.UserDbo", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasColumnName("id")
                        .HasDefaultValueSql("typeid_generate_text('user')");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer")
                        .HasColumnName("access_failed_count");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("text")
                        .HasColumnName("concurrency_stamp");

                    b.Property<string>("Email")
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("email_confirmed");

                    b.Property<bool>("Enabled")
                        .HasColumnType("boolean")
                        .HasColumnName("enabled");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("lockout_enabled");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("lockout_end");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("text")
                        .HasColumnName("normalized_email");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("text")
                        .HasColumnName("normalized_user_name");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text")
                        .HasColumnName("phone_number");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("phone_number_confirmed");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text")
                        .HasColumnName("security_stamp");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("two_factor_enabled");

                    b.Property<string>("UserName")
                        .HasColumnType("text")
                        .HasColumnName("user_name");

                    b.HasKey("Id")
                        .HasName("pk_user");

                    b.ToTable("user", "identity", t =>
                        {
                            t.HasCheckConstraint("CK_id", "typeid_check_text(id, 'user')");
                        });
                });

            modelBuilder.Entity("Webion.Stargaze.Pgsql.Entities.Identity.UserLoginDbo", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text")
                        .HasColumnName("login_provider");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text")
                        .HasColumnName("provider_key");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text")
                        .HasColumnName("provider_display_name");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.HasKey("LoginProvider", "ProviderKey")
                        .HasName("pk_user_login");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_login_user_id");

                    b.ToTable("user_login", "identity");
                });

            modelBuilder.Entity("Webion.Stargaze.Pgsql.Entities.Identity.UserRoleDbo", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.Property<string>("RoleId")
                        .HasColumnType("text")
                        .HasColumnName("role_id");

                    b.HasKey("UserId", "RoleId")
                        .HasName("pk_user_role");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_user_role_role_id");

                    b.ToTable("user_role", "identity");
                });

            modelBuilder.Entity("Webion.Stargaze.Pgsql.Entities.Identity.UserTokenDbo", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text")
                        .HasColumnName("login_provider");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Value")
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("UserId", "LoginProvider", "Name")
                        .HasName("pk_user_token");

                    b.ToTable("user_token", "identity");
                });

            modelBuilder.Entity("Webion.Stargaze.Pgsql.Entities.Projects.ProjectDbo", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasColumnName("id")
                        .HasDefaultValueSql("typeid_generate_text('project')");

                    b.Property<string>("Description")
                        .HasMaxLength(4096)
                        .HasColumnType("character varying(4096)")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_project");

                    b.ToTable("project", "projects", t =>
                        {
                            t.HasCheckConstraint("CK_id", "typeid_check_text(id, 'project')");
                        });
                });

            modelBuilder.Entity("Webion.Stargaze.Pgsql.Entities.Projects.TaskDbo", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasColumnName("id")
                        .HasDefaultValueSql("typeid_generate_text('task')");

                    b.Property<string>("Description")
                        .HasMaxLength(4096)
                        .HasColumnType("character varying(4096)")
                        .HasColumnName("description");

                    b.Property<string>("ProjectId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("project_id");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_task");

                    b.HasIndex("ProjectId")
                        .HasDatabaseName("ix_task_project_id");

                    b.ToTable("task", "projects", t =>
                        {
                            t.HasCheckConstraint("CK_id", "typeid_check_text(id, 'task')");
                        });
                });

            modelBuilder.Entity("Webion.Stargaze.Pgsql.Entities.TimeTracking.TimeEntryDbo", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasColumnName("id")
                        .HasDefaultValueSql("typeid_generate_text('time_entry')");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("interval")
                        .HasColumnName("duration");

                    b.Property<DateTimeOffset>("End")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("end");

                    b.Property<DateTimeOffset>("Start")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("start");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_time_entry");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_time_entry_user_id");

                    b.ToTable("time_entry", "time_tracking", t =>
                        {
                            t.HasCheckConstraint("CK_id", "typeid_check_text(id, 'time_entry')");
                        });
                });

            modelBuilder.Entity("Webion.Stargaze.Pgsql.Entities.Connect.ApiKeyDbo", b =>
                {
                    b.HasOne("Webion.Stargaze.Pgsql.Entities.Connect.ClientDbo", "Client")
                        .WithMany("ApiKeys")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_api_key_client_dbo_client_id");

                    b.Navigation("Client");
                });

            modelBuilder.Entity("Webion.Stargaze.Pgsql.Entities.Connect.RefreshTokenDbo", b =>
                {
                    b.HasOne("Webion.Stargaze.Pgsql.Entities.Identity.UserDbo", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_refresh_token_user_dbo_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Webion.Stargaze.Pgsql.Entities.Identity.RoleClaimDbo", b =>
                {
                    b.HasOne("Webion.Stargaze.Pgsql.Entities.Identity.RoleDbo", "Role")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_role_claim_role_dbo_role_id");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Webion.Stargaze.Pgsql.Entities.Identity.UserClaimDbo", b =>
                {
                    b.HasOne("Webion.Stargaze.Pgsql.Entities.Identity.UserDbo", "User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_claim_user_dbo_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Webion.Stargaze.Pgsql.Entities.Identity.UserLoginDbo", b =>
                {
                    b.HasOne("Webion.Stargaze.Pgsql.Entities.Identity.UserDbo", "User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_login_user_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Webion.Stargaze.Pgsql.Entities.Identity.UserRoleDbo", b =>
                {
                    b.HasOne("Webion.Stargaze.Pgsql.Entities.Identity.RoleDbo", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_role_role_role_id");

                    b.HasOne("Webion.Stargaze.Pgsql.Entities.Identity.UserDbo", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_role_user_user_id");

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Webion.Stargaze.Pgsql.Entities.Identity.UserTokenDbo", b =>
                {
                    b.HasOne("Webion.Stargaze.Pgsql.Entities.Identity.UserDbo", "User")
                        .WithMany("Tokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_token_user_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Webion.Stargaze.Pgsql.Entities.Projects.TaskDbo", b =>
                {
                    b.HasOne("Webion.Stargaze.Pgsql.Entities.Projects.ProjectDbo", "Project")
                        .WithMany("Tasks")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("fk_task_project_project_id");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("Webion.Stargaze.Pgsql.Entities.TimeTracking.TimeEntryDbo", b =>
                {
                    b.HasOne("Webion.Stargaze.Pgsql.Entities.Identity.UserDbo", "User")
                        .WithMany("TimeEntries")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("fk_time_entry_user_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Webion.Stargaze.Pgsql.Entities.Connect.ClientDbo", b =>
                {
                    b.Navigation("ApiKeys");
                });

            modelBuilder.Entity("Webion.Stargaze.Pgsql.Entities.Identity.RoleDbo", b =>
                {
                    b.Navigation("Claims");
                });

            modelBuilder.Entity("Webion.Stargaze.Pgsql.Entities.Identity.UserDbo", b =>
                {
                    b.Navigation("Claims");

                    b.Navigation("Logins");

                    b.Navigation("RefreshTokens");

                    b.Navigation("TimeEntries");

                    b.Navigation("Tokens");
                });

            modelBuilder.Entity("Webion.Stargaze.Pgsql.Entities.Projects.ProjectDbo", b =>
                {
                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}
