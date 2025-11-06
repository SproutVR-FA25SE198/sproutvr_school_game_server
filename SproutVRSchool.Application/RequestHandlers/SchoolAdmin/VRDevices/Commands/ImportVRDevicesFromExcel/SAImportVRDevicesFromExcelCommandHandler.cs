using MediatR;
using Microsoft.Extensions.Logging;
using SproutVRSchool.Application.Abstractions.FileServices;
using SproutVRSchool.Application.Abstractions.FileServices.Dtos;
using SproutVRSchool.Application.Abstractions.Repositories;
using SproutVRSchool.Application.Exceptions.ContentSeedings;
using SproutVRSchool.Application.Specifications;
using SproutVRSchool.Domain.Entities.VRDevices;

namespace SproutVRSchool.Application.RequestHandlers.SchoolAdmin.VRDevices.Commands.ImportVRDevicesFromExcel;

public class SAImportVRDevicesFromExcelCommandHandler(
    IUnitOfWork uow,
    ILogger<SAImportVRDevicesFromExcelCommandHandler> logger,
    IExcelFileService excelFileService,
    IFileValidationService fileValidationService) : IRequestHandler<SAImportVRDevicesFromExcelCommand, SAImportVRDevicesFromExcelCommandResponseDto>
{
    public async Task<SAImportVRDevicesFromExcelCommandResponseDto> Handle(
        SAImportVRDevicesFromExcelCommand request,
        CancellationToken cancellationToken)
    {
        // 1. File Validation
        if (!fileValidationService.IsFileValid(request.ExcelFile))
        {
            throw new SvrFileNotSupportedException("The provided Excel file is invalid or empty.");
        }

        if (!fileValidationService.IsExcelFile(request.ExcelFile))
        {
            throw new SvrFileNotSupportedException("The provided file is not a supported Excel file.");
        }

        // 2. Process Excel File, get the readonly list
        IReadOnlyList<VRDeviceExcelRowDto> vrdevices = excelFileService.ReadVRDevicesExcel(request.ExcelFile);

        // 3. Seeding to the VR devices
        int totalNewSeeded = 0;
        foreach (VRDeviceExcelRowDto vrdevice in vrdevices)
        {
            VRDevice? existingVrDevice = await uow
                .Repository<VRDevice>()
                .GetEntityBySpec(new VRDevicesSpecification(vrdevice.DeviceName, vrdevice.SerialNumber));

            // if already seeded, skip it
            if (existingVrDevice != null)
            {
                continue;
            }

            // Create new vr devices and add to db
            var newVrDevice = VRDevice.Create(vrdevice.DeviceName, vrdevice.SerialNumber);

            uow.Repository<VRDevice>().Add(newVrDevice);

            totalNewSeeded++;
        }

        await uow.SaveChangesAsync(cancellationToken);

        // 4. Return response
        logger.LogInformation("Imported {Imported} VR Devices.", totalNewSeeded);
        return new SAImportVRDevicesFromExcelCommandResponseDto(
            totalNewSeeded,
    $"Successfully imported {totalNewSeeded} vr devices from the provided Excel file.");
    }
}


