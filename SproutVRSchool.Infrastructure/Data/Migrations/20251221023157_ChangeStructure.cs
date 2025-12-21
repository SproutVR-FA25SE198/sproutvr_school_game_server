using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SproutVRSchool.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class ChangeStructure : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "PlayedAtUtc",
            schema: "app",
            table: "VRLessons",
            type: "timestamp with time zone",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "PlayedAtUtc",
            schema: "app",
            table: "VRLessons");
    }
}
