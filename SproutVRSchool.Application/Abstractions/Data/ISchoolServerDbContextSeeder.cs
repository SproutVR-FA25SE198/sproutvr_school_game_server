namespace SproutVRSchool.Application.Abstractions.Data;

public interface ISchoolServerDbContextSeeder
{
    Task SeedDevelopmentAsync();
    Task SeedProductionAsync();
}
