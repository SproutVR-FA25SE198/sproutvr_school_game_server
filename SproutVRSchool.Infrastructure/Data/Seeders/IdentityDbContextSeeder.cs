using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.Identities;

namespace SproutVRSchool.Infrastructure.Data.Seeders;

public class IdentityDbContextSeeder
{
    // ==========================
    // === Fields
    // ==========================

    private readonly IConfiguration _configuration;
    private readonly RoleManager<UserAccountRole> _roleManager;
    private readonly UserManager<UserAccount> _userManager;

    // ==========================
    // === Constructors
    // ==========================

    public IdentityDbContextSeeder(IConfiguration configuration, RoleManager<UserAccountRole> roleManager, UserManager<UserAccount> userManager)
    {
        _configuration = configuration;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    // ==========================
    // === Methods
    // ==========================

    public async Task SeedDevelopmentAsync()
    {

        await SeedRolesAsync();
        await SeedAdminUserAsync();
        await SeedTeacherUsersAsync();
    }

    public async Task SeedProductionAsync()
    {

        await SeedRolesAsync();
        await SeedAdminUserAsync();
    }

    /// <summary>
    /// Seeding Roles
    /// </summary>
    /// <param name="roleManager"></param>
    /// <returns></returns>
    private async Task SeedRolesAsync()
    {
        string[] roleNames = { AppCts.Db.ROLE_TEACHER, AppCts.Db.ROLE_SCHOOL_ADMIN };

        foreach (string roleName in roleNames)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var role = new UserAccountRole { Name = roleName };
                await _roleManager.CreateAsync(role);
            }
        }
    }

    /// <summary>
    /// Seed Admin User
    /// </summary>
    /// <returns></returns>
    private async Task SeedAdminUserAsync()
    {
        string? adminUserName = _configuration["SeedingSettings:DefaultAdminUser:UserName"];
        string? adminPassword = _configuration["SeedingSettings:DefaultAdminUser:Password"] ?? "000000";

        if (string.IsNullOrEmpty(adminUserName) || await _userManager.FindByEmailAsync(adminUserName) != null)
        {
            return;
        }

        var adminUser = new SchoolAdmin
        {
            Id = Guid.NewGuid(),
            UserName = adminUserName,
            Email = adminUserName,
            FirstName = "Râu",
            LastName = "Vũ Thị",
            Status = UserAccountStatus.Active,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            UpdatedAtUtc = DateTimeOffset.UtcNow

        };
        IdentityResult result = await _userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(adminUser, AppCts.Db.ROLE_SCHOOL_ADMIN);
        }
    }

    // ==========================
    // === Seed Teacher Users
    // ==========================
    private async Task SeedTeacherUsersAsync()
    {
        // --- Teacher 1 ---
        string teacher1Email = _configuration["SeedingSettings:DefaultTeacherUsers:0:UserName"]!;
        string teacher1Password = _configuration["SeedingSettings:DefaultTeacherUsers:0:Password"]!;
        var teacher1Id = new Guid("0199f4b1-8487-4352-8a2a-320a00e40e58");

        // Check if the first teacher already exists
        if (await _userManager.FindByEmailAsync(teacher1Email) == null)
        {
            var teacherUser1 = new Teacher
            {
                Id = teacher1Id,
                UserName = teacher1Email,
                Email = teacher1Email,
                FirstName = "Bảy",
                LastName = "Nguyễn Thị",
                Status = UserAccountStatus.Active,
                CreatedAtUtc = DateTimeOffset.UtcNow,
                UpdatedAtUtc = DateTimeOffset.UtcNow
            };

            IdentityResult result1 = await _userManager.CreateAsync(teacherUser1, teacher1Password);
            if (result1.Succeeded)
            {
                await _userManager.AddToRoleAsync(teacherUser1, "Teacher");
            }
        }

        // --- Teacher 2 ---
        string teacher2Email = _configuration["SeedingSettings:DefaultTeacherUsers:1:UserName"]!;
        string teacher2Password = _configuration["SeedingSettings:DefaultTeacherUsers:1:Password"]!;
        var teacher2Id = new Guid("0199f4b1-8487-4352-8a2a-320a00e40e59");

        // Check if the second teacher already exists
        if (await _userManager.FindByEmailAsync(teacher2Email) == null)
        {
            var teacherUser2 = new Teacher
            {
                Id = teacher2Id, // Explicit GUID
                UserName = teacher2Email,
                Email = teacher2Email,
                FirstName = "Kim",
                LastName = "Kardashian",
                Status = UserAccountStatus.Active,
                CreatedAtUtc = DateTimeOffset.UtcNow
            };

            IdentityResult result2 = await _userManager.CreateAsync(teacherUser2, teacher2Password);
            if (result2.Succeeded)
            {
                await _userManager.AddToRoleAsync(teacherUser2, AppCts.Db.ROLE_TEACHER);
            }
        }
    }
}
