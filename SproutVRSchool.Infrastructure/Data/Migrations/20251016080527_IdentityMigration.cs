using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SproutVRSchool.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class IdentityMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "app");

        migrationBuilder.EnsureSchema(
            name: "auth");

        migrationBuilder.AlterDatabase()
            .Annotation("Npgsql:PostgresExtension:citext", ",,");

        migrationBuilder.CreateTable(
            name: "ActivityTypes",
            schema: "app",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                ActivityCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ActivityTypes", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AspNetRoles",
            schema: "auth",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetRoles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "MasterSubjects",
            schema: "app",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                Description = table.Column<string>(type: "text", maxLength: 1000, nullable: false),
                ImageUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MasterSubjects", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "UserAccounts",
            schema: "auth",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                UserName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                PasswordHash = table.Column<string>(type: "text", nullable: true),
                SecurityStamp = table.Column<string>(type: "text", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                PhoneNumber = table.Column<string>(type: "text", nullable: true),
                PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserAccounts", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "VRDevices",
            schema: "app",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                SerialNumber = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_VRDevices", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AspNetRoleClaims",
            schema: "auth",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                ClaimType = table.Column<string>(type: "text", nullable: true),
                ClaimValue = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                    column: x => x.RoleId,
                    principalSchema: "auth",
                    principalTable: "AspNetRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Subjects",
            schema: "app",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                MasterSubjectId = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                Description = table.Column<string>(type: "text", maxLength: 1000, nullable: false),
                ImageUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Subjects", x => x.Id);
                table.ForeignKey(
                    name: "FK_Subjects_MasterSubjects_MasterSubjectId",
                    column: x => x.MasterSubjectId,
                    principalSchema: "app",
                    principalTable: "MasterSubjects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserClaims",
            schema: "auth",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                ClaimType = table.Column<string>(type: "text", nullable: true),
                ClaimValue = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_AspNetUserClaims_UserAccounts_UserId",
                    column: x => x.UserId,
                    principalSchema: "auth",
                    principalTable: "UserAccounts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserLogins",
            schema: "auth",
            columns: table => new
            {
                LoginProvider = table.Column<string>(type: "text", nullable: false),
                ProviderKey = table.Column<string>(type: "text", nullable: false),
                ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                UserId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                table.ForeignKey(
                    name: "FK_AspNetUserLogins_UserAccounts_UserId",
                    column: x => x.UserId,
                    principalSchema: "auth",
                    principalTable: "UserAccounts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserRoles",
            schema: "auth",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                RoleId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                table.ForeignKey(
                    name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                    column: x => x.RoleId,
                    principalSchema: "auth",
                    principalTable: "AspNetRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_AspNetUserRoles_UserAccounts_UserId",
                    column: x => x.UserId,
                    principalSchema: "auth",
                    principalTable: "UserAccounts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserTokens",
            schema: "auth",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                LoginProvider = table.Column<string>(type: "text", nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                Value = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                table.ForeignKey(
                    name: "FK_AspNetUserTokens_UserAccounts_UserId",
                    column: x => x.UserId,
                    principalSchema: "auth",
                    principalTable: "UserAccounts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "SchoolAdmins",
            schema: "auth",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                OrganizationId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SchoolAdmins", x => x.Id);
                table.ForeignKey(
                    name: "FK_SchoolAdmins_UserAccounts_Id",
                    column: x => x.Id,
                    principalSchema: "auth",
                    principalTable: "UserAccounts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Teachers",
            schema: "auth",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Teachers", x => x.Id);
                table.ForeignKey(
                    name: "FK_Teachers_UserAccounts_Id",
                    column: x => x.Id,
                    principalSchema: "auth",
                    principalTable: "UserAccounts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Maps",
            schema: "app",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                SubjectId = table.Column<Guid>(type: "uuid", nullable: false),
                MapCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                Name = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                Description = table.Column<string>(type: "text", maxLength: 1000, nullable: false),
                ImageUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Maps", x => x.Id);
                table.ForeignKey(
                    name: "FK_Maps_Subjects_SubjectId",
                    column: x => x.SubjectId,
                    principalSchema: "app",
                    principalTable: "Subjects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Lessons",
            schema: "app",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                SubjectId = table.Column<Guid>(type: "uuid", nullable: false),
                TeacherId = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                Description = table.Column<string>(type: "text", maxLength: 1000, nullable: false),
                ResourceUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Lessons", x => x.Id);
                table.ForeignKey(
                    name: "FK_Lessons_Subjects_SubjectId",
                    column: x => x.SubjectId,
                    principalSchema: "app",
                    principalTable: "Subjects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Lessons_Teachers_TeacherId",
                    column: x => x.TeacherId,
                    principalSchema: "auth",
                    principalTable: "Teachers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "MapObjects",
            schema: "app",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                MapId = table.Column<Guid>(type: "uuid", nullable: false),
                ObjectCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                Name = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                ImageUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MapObjects", x => x.Id);
                table.ForeignKey(
                    name: "FK_MapObjects_Maps_MapId",
                    column: x => x.MapId,
                    principalSchema: "app",
                    principalTable: "Maps",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "TaskLocations",
            schema: "app",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                MapId = table.Column<Guid>(type: "uuid", nullable: false),
                LocationCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                Name = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                ImageUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TaskLocations", x => x.Id);
                table.ForeignKey(
                    name: "FK_TaskLocations_Maps_MapId",
                    column: x => x.MapId,
                    principalSchema: "app",
                    principalTable: "Maps",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "VRLessons",
            schema: "app",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                LessonId = table.Column<Guid>(type: "uuid", nullable: false),
                MapId = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                Description = table.Column<string>(type: "text", maxLength: 1000, nullable: false),
                MaxDuration = table.Column<TimeSpan>(type: "interval", nullable: false),
                PresetJsonRelativeFilePath = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                ImageUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_VRLessons", x => x.Id);
                table.ForeignKey(
                    name: "FK_VRLessons_Lessons_LessonId",
                    column: x => x.LessonId,
                    principalSchema: "app",
                    principalTable: "Lessons",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_VRLessons_Maps_MapId",
                    column: x => x.MapId,
                    principalSchema: "app",
                    principalTable: "Maps",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "ObjectActivityTypes",
            schema: "app",
            columns: table => new
            {
                MapObjectId = table.Column<Guid>(type: "uuid", nullable: false),
                ActivityTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ObjectActivityTypes", x => new { x.MapObjectId, x.ActivityTypeId });
                table.ForeignKey(
                    name: "FK_ObjectActivityTypes_ActivityTypes_ActivityTypeId",
                    column: x => x.ActivityTypeId,
                    principalSchema: "app",
                    principalTable: "ActivityTypes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_ObjectActivityTypes_MapObjects_MapObjectId",
                    column: x => x.MapObjectId,
                    principalSchema: "app",
                    principalTable: "MapObjects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "ObjectLocations",
            schema: "app",
            columns: table => new
            {
                ObjectId = table.Column<Guid>(type: "uuid", nullable: false),
                TaskLocationId = table.Column<Guid>(type: "uuid", nullable: false),
                CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ObjectLocations", x => new { x.TaskLocationId, x.ObjectId });
                table.ForeignKey(
                    name: "FK_ObjectLocations_MapObjects_ObjectId",
                    column: x => x.ObjectId,
                    principalSchema: "app",
                    principalTable: "MapObjects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_ObjectLocations_TaskLocations_TaskLocationId",
                    column: x => x.TaskLocationId,
                    principalSchema: "app",
                    principalTable: "TaskLocations",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "VRTasks",
            schema: "app",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                TaskLocationId = table.Column<Guid>(type: "uuid", nullable: false),
                MapObjectId = table.Column<Guid>(type: "uuid", nullable: false),
                ActivityTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                TaskNumber = table.Column<string>(type: "citext", nullable: false),
                Description = table.Column<string>(type: "text", maxLength: 1000, nullable: false),
                CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_VRTasks", x => x.Id);
                table.ForeignKey(
                    name: "FK_VRTasks_ActivityTypes_ActivityTypeId",
                    column: x => x.ActivityTypeId,
                    principalSchema: "app",
                    principalTable: "ActivityTypes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_VRTasks_MapObjects_MapObjectId",
                    column: x => x.MapObjectId,
                    principalSchema: "app",
                    principalTable: "MapObjects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_VRTasks_TaskLocations_TaskLocationId",
                    column: x => x.TaskLocationId,
                    principalSchema: "app",
                    principalTable: "TaskLocations",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "VRLearningSessions",
            schema: "app",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                VRLessonId = table.Column<Guid>(type: "uuid", nullable: false),
                TeacherId = table.Column<Guid>(type: "uuid", nullable: false),
                StartTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                EndTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                Duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_VRLearningSessions", x => x.Id);
                table.ForeignKey(
                    name: "FK_VRLearningSessions_Teachers_TeacherId",
                    column: x => x.TeacherId,
                    principalSchema: "auth",
                    principalTable: "Teachers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_VRLearningSessions_VRLessons_VRLessonId",
                    column: x => x.VRLessonId,
                    principalSchema: "app",
                    principalTable: "VRLessons",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "VRDeviceSessionSummaries",
            schema: "app",
            columns: table => new
            {
                VRDeviceId = table.Column<Guid>(type: "uuid", nullable: false),
                VRLearningSessionId = table.Column<Guid>(type: "uuid", nullable: false),
                StudentName = table.Column<string>(type: "citext", maxLength: 100, nullable: false),
                NoTasksCompleted = table.Column<int>(type: "integer", nullable: false),
                CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_VRDeviceSessionSummaries", x => new { x.VRLearningSessionId, x.VRDeviceId });
                table.ForeignKey(
                    name: "FK_VRDeviceSessionSummaries_VRDevices_VRDeviceId",
                    column: x => x.VRDeviceId,
                    principalSchema: "app",
                    principalTable: "VRDevices",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_VRDeviceSessionSummaries_VRLearningSessions_VRLearningSessi~",
                    column: x => x.VRLearningSessionId,
                    principalSchema: "app",
                    principalTable: "VRLearningSessions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "VRDeviceTaskProgresses",
            schema: "app",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                VRDeviceId = table.Column<Guid>(type: "uuid", nullable: false),
                VRTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                VRLearningSessionId = table.Column<Guid>(type: "uuid", nullable: false),
                IsCompleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                IsCorrect = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                CompletionTimeAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_VRDeviceTaskProgresses", x => x.Id);
                table.ForeignKey(
                    name: "FK_VRDeviceTaskProgresses_VRDevices_VRDeviceId",
                    column: x => x.VRDeviceId,
                    principalSchema: "app",
                    principalTable: "VRDevices",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_VRDeviceTaskProgresses_VRLearningSessions_VRLearningSession~",
                    column: x => x.VRLearningSessionId,
                    principalSchema: "app",
                    principalTable: "VRLearningSessions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_VRDeviceTaskProgresses_VRTasks_VRTaskId",
                    column: x => x.VRTaskId,
                    principalSchema: "app",
                    principalTable: "VRTasks",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_AspNetRoleClaims_RoleId",
            schema: "auth",
            table: "AspNetRoleClaims",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "RoleNameIndex",
            schema: "auth",
            table: "AspNetRoles",
            column: "NormalizedName",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserClaims_UserId",
            schema: "auth",
            table: "AspNetUserClaims",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserLogins_UserId",
            schema: "auth",
            table: "AspNetUserLogins",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserRoles_RoleId",
            schema: "auth",
            table: "AspNetUserRoles",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "IX_Lessons_Name",
            schema: "app",
            table: "Lessons",
            column: "Name");

        migrationBuilder.CreateIndex(
            name: "IX_Lessons_SubjectId",
            schema: "app",
            table: "Lessons",
            column: "SubjectId");

        migrationBuilder.CreateIndex(
            name: "IX_Lessons_TeacherId",
            schema: "app",
            table: "Lessons",
            column: "TeacherId");

        migrationBuilder.CreateIndex(
            name: "IX_MapObjects_MapId",
            schema: "app",
            table: "MapObjects",
            column: "MapId");

        migrationBuilder.CreateIndex(
            name: "IX_Maps_SubjectId",
            schema: "app",
            table: "Maps",
            column: "SubjectId");

        migrationBuilder.CreateIndex(
            name: "IX_ObjectActivityTypes_ActivityTypeId",
            schema: "app",
            table: "ObjectActivityTypes",
            column: "ActivityTypeId");

        migrationBuilder.CreateIndex(
            name: "IX_ObjectLocations_ObjectId",
            schema: "app",
            table: "ObjectLocations",
            column: "ObjectId");

        migrationBuilder.CreateIndex(
            name: "IX_SchoolAdmins_OrganizationId",
            schema: "auth",
            table: "SchoolAdmins",
            column: "OrganizationId");

        migrationBuilder.CreateIndex(
            name: "IX_Subjects_MasterSubjectId",
            schema: "app",
            table: "Subjects",
            column: "MasterSubjectId");

        migrationBuilder.CreateIndex(
            name: "IX_Subjects_Name",
            schema: "app",
            table: "Subjects",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_TaskLocations_MapId",
            schema: "app",
            table: "TaskLocations",
            column: "MapId");

        migrationBuilder.CreateIndex(
            name: "EmailIndex",
            schema: "auth",
            table: "UserAccounts",
            column: "NormalizedEmail");

        migrationBuilder.CreateIndex(
            name: "IX_UserAccounts_Email",
            schema: "auth",
            table: "UserAccounts",
            column: "Email");

        migrationBuilder.CreateIndex(
            name: "UserNameIndex",
            schema: "auth",
            table: "UserAccounts",
            column: "NormalizedUserName",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_VRDevices_SerialNumber",
            schema: "app",
            table: "VRDevices",
            column: "SerialNumber",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_VRDeviceSessionSummaries_VRDeviceId",
            schema: "app",
            table: "VRDeviceSessionSummaries",
            column: "VRDeviceId");

        migrationBuilder.CreateIndex(
            name: "IX_VRDeviceTaskProgresses_VRDeviceId",
            schema: "app",
            table: "VRDeviceTaskProgresses",
            column: "VRDeviceId");

        migrationBuilder.CreateIndex(
            name: "IX_VRDeviceTaskProgresses_VRLearningSessionId",
            schema: "app",
            table: "VRDeviceTaskProgresses",
            column: "VRLearningSessionId");

        migrationBuilder.CreateIndex(
            name: "IX_VRDeviceTaskProgresses_VRTaskId",
            schema: "app",
            table: "VRDeviceTaskProgresses",
            column: "VRTaskId");

        migrationBuilder.CreateIndex(
            name: "IX_VRLearningSessions_TeacherId",
            schema: "app",
            table: "VRLearningSessions",
            column: "TeacherId");

        migrationBuilder.CreateIndex(
            name: "IX_VRLearningSessions_VRLessonId",
            schema: "app",
            table: "VRLearningSessions",
            column: "VRLessonId");

        migrationBuilder.CreateIndex(
            name: "IX_VRLessons_LessonId",
            schema: "app",
            table: "VRLessons",
            column: "LessonId");

        migrationBuilder.CreateIndex(
            name: "IX_VRLessons_MapId",
            schema: "app",
            table: "VRLessons",
            column: "MapId");

        migrationBuilder.CreateIndex(
            name: "IX_VRTasks_ActivityTypeId",
            schema: "app",
            table: "VRTasks",
            column: "ActivityTypeId");

        migrationBuilder.CreateIndex(
            name: "IX_VRTasks_MapObjectId",
            schema: "app",
            table: "VRTasks",
            column: "MapObjectId");

        migrationBuilder.CreateIndex(
            name: "IX_VRTasks_TaskLocationId",
            schema: "app",
            table: "VRTasks",
            column: "TaskLocationId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AspNetRoleClaims",
            schema: "auth");

        migrationBuilder.DropTable(
            name: "AspNetUserClaims",
            schema: "auth");

        migrationBuilder.DropTable(
            name: "AspNetUserLogins",
            schema: "auth");

        migrationBuilder.DropTable(
            name: "AspNetUserRoles",
            schema: "auth");

        migrationBuilder.DropTable(
            name: "AspNetUserTokens",
            schema: "auth");

        migrationBuilder.DropTable(
            name: "ObjectActivityTypes",
            schema: "app");

        migrationBuilder.DropTable(
            name: "ObjectLocations",
            schema: "app");

        migrationBuilder.DropTable(
            name: "SchoolAdmins",
            schema: "auth");

        migrationBuilder.DropTable(
            name: "VRDeviceSessionSummaries",
            schema: "app");

        migrationBuilder.DropTable(
            name: "VRDeviceTaskProgresses",
            schema: "app");

        migrationBuilder.DropTable(
            name: "AspNetRoles",
            schema: "auth");

        migrationBuilder.DropTable(
            name: "VRDevices",
            schema: "app");

        migrationBuilder.DropTable(
            name: "VRLearningSessions",
            schema: "app");

        migrationBuilder.DropTable(
            name: "VRTasks",
            schema: "app");

        migrationBuilder.DropTable(
            name: "VRLessons",
            schema: "app");

        migrationBuilder.DropTable(
            name: "ActivityTypes",
            schema: "app");

        migrationBuilder.DropTable(
            name: "MapObjects",
            schema: "app");

        migrationBuilder.DropTable(
            name: "TaskLocations",
            schema: "app");

        migrationBuilder.DropTable(
            name: "Lessons",
            schema: "app");

        migrationBuilder.DropTable(
            name: "Maps",
            schema: "app");

        migrationBuilder.DropTable(
            name: "Teachers",
            schema: "auth");

        migrationBuilder.DropTable(
            name: "Subjects",
            schema: "app");

        migrationBuilder.DropTable(
            name: "UserAccounts",
            schema: "auth");

        migrationBuilder.DropTable(
            name: "MasterSubjects",
            schema: "app");
    }
}
