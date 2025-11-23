using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SproutVRSchool.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class AddedSystemSettings : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "SystemSettings",
            schema: "app",
            columns: table => new
            {
                Key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                Value = table.Column<string>(type: "text", nullable: true),
                Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SystemSettings", x => x.Key);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "SystemSettings",
            schema: "app");
    }
}
