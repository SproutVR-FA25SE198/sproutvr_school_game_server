using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SproutVRSchool.Application.Abstractions.Data;
using SproutVRSchool.Application.Abstractions.FileServices.Dtos;
using SproutVRSchool.Application.Exceptions.ContentSeedings;
using SproutVRSchool.Domain;
using SproutVRSchool.Domain.Entities.Identities;

namespace SproutVRSchool.Infrastructure.Data.Seeders;


public class IdentityDbContextSeeder(
    IConfiguration configuration,
    ILogger<IdentityDbContextSeeder> logger,
    RoleManager<UserAccountRole> roleManager,
    UserManager<UserAccount> userManager) : IIdentityDbContextSeeder
{
    // ==========================
    // === Methods
    // ==========================

    /// <summary>
    /// Seeding for Development
    /// </summary>
    /// <returns></returns>
    public async Task SeedDevelopmentAsync()
    {

        await SeedRolesAsync();
        await SeedSchoolAdminUserAsync();
        await SeedTeacherUsersAsync();
    }

    /// <summary>
    /// Seeding for Production Environment
    /// </summary>
    /// <returns></returns>
    public async Task SeedProductionAsync()
    {

        await SeedRolesAsync();
        await SeedSchoolAdminUserAsync();
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
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var role = new UserAccountRole { Name = roleName };
                await roleManager.CreateAsync(role);
            }
        }
    }

    // ==========================
    // === Seed School Admin User
    // ==========================

    /// <summary>
    /// Seed Admin User
    /// </summary>
    /// <returns></returns>
    private async Task SeedSchoolAdminUserAsync()
    {
        string? adminUserName = configuration["SeedingSettings:DefaultAdminUser:UserName"];
        string? adminPassword = configuration["SeedingSettings:DefaultAdminUser:Password"] ?? "000000";
        string? adminOrganizationId = configuration["SeedingSettings:DefaultAdminUser:OrganizationId"];

        if (string.IsNullOrEmpty(adminUserName) || await userManager.FindByEmailAsync(adminUserName) != null)
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
            OrganizationId = Guid.Parse(adminOrganizationId!),
            Status = UserAccountStatus.Active,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            UpdatedAtUtc = DateTimeOffset.UtcNow

        };
        IdentityResult result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, AppCts.Db.ROLE_SCHOOL_ADMIN);
        }
    }

    // ==========================
    // === Seed Teacher Users
    // ==========================

    /// <summary>
    /// Seeding teachers by excel file row
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    /// <exception cref="SvrInstallFailedException"></exception>
    public async Task<bool> SeedTeacherFromExcelFileAsync(TeacherAccountExcelRowDto account)
    {
        // 1. If the teacher (username or email) already exists, then skipping seeding 
        bool isExistingUser = await userManager.FindByEmailAsync(account.Email) != null ||
                             await userManager.FindByNameAsync(account.UserName) != null;

        // 2. if existing, then return
        if (isExistingUser)
        {
            logger.LogWarning("Already seeding the UserName {Username} with Email {Email}", account.UserName, account.Email);
            return false;
        }

        // 3. Create new teacher entity
        var teacher = Teacher.Create(username: account.UserName,
                                     email: account.Email,
                                     dateOfBirth: account.DateOfBirth,
                                     firstName: account.FirstName,
                                     lastName: account.LastName);

        IdentityResult createdResult = await userManager.CreateAsync(teacher, account.DefaultPassword);
        if (!createdResult.Succeeded)
        {
            return false;
        }

        // 4. Assign role Teacher to the created account
        await userManager.AddToRoleAsync(teacher, AppCts.Db.ROLE_TEACHER);
        logger.LogInformation("Seeded new Teacher account: {Email}", account.Email);
        return true;
    }

    /// <summary>
    /// Seeding teachers for Development only 
    /// </summary>
    /// <returns></returns>
    private async Task SeedTeacherUsersAsync()
    {
        // --- Teacher 1 ---
        string teacher1Email = configuration["SeedingSettings:DefaultTeacherUsers:0:UserName"]!;
        string teacher1Password = configuration["SeedingSettings:DefaultTeacherUsers:0:Password"]!;
        var teacher1Id = new Guid("0199f4b1-8487-4352-8a2a-320a00e40e58");

        // Check if the first teacher already exists
        if (await userManager.FindByEmailAsync(teacher1Email) == null)
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

            IdentityResult result1 = await userManager.CreateAsync(teacherUser1, teacher1Password);
            if (result1.Succeeded)
            {
                await userManager.AddToRoleAsync(teacherUser1, AppCts.Db.ROLE_TEACHER);
            }
        }

        // --- Teacher 2 ---
        string teacher2Email = configuration["SeedingSettings:DefaultTeacherUsers:1:UserName"]!;
        string teacher2Password = configuration["SeedingSettings:DefaultTeacherUsers:1:Password"]!;
        var teacher2Id = new Guid("0199f4b1-8487-4352-8a2a-320a00e40e59");

        // Check if the second teacher already exists
        if (await userManager.FindByEmailAsync(teacher2Email) == null)
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

            IdentityResult result2 = await userManager.CreateAsync(teacherUser2, teacher2Password);
            if (result2.Succeeded)
            {
                await userManager.AddToRoleAsync(teacherUser2, AppCts.Db.ROLE_TEACHER);
            }
        }
    }
}
