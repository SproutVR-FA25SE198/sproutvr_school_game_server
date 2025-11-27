using SproutVRSchool.Application.Abstractions.FileServices.Dtos;

namespace SproutVRSchool.Application.Abstractions.Data;

public interface IIdentityDbContextSeeder
{
    Task SeedDevelopmentAsync();
    Task SeedProductionAsync();
    Task<bool> SeedTeacherFromExcelFileAsync(TeacherAccountExcelRowDto account);
}
