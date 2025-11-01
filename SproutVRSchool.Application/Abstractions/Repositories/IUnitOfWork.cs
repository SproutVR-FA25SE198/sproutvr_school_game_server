using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SproutVRSchool.Domain.Entities;
using SproutVRSchool.Domain.Entities.ActivityTypes;
using SproutVRSchool.Domain.Entities.Identities;
using SproutVRSchool.Domain.Entities.Lessons;
using SproutVRSchool.Domain.Entities.MapObjects;
using SproutVRSchool.Domain.Entities.Maps;
using SproutVRSchool.Domain.Entities.MasterSubjects;
using SproutVRSchool.Domain.Entities.ObjectActivityTypes;
using SproutVRSchool.Domain.Entities.ObjectLocations;
using SproutVRSchool.Domain.Entities.Subjects;
using SproutVRSchool.Domain.Entities.TaskLocations;
using SproutVRSchool.Domain.Entities.VRDevices;
using SproutVRSchool.Domain.Entities.VRDeviceSessionSummaries;
using SproutVRSchool.Domain.Entities.VRDeviceTaskProgresses;
using SproutVRSchool.Domain.Entities.VRLearningSessions;
using SproutVRSchool.Domain.Entities.VRLessons;
using SproutVRSchool.Domain.Entities.VRTasks;

namespace SproutVRSchool.Application.Abstractions.Repositories;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<T> Repository<T>() where T : BaseEntity;
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}
