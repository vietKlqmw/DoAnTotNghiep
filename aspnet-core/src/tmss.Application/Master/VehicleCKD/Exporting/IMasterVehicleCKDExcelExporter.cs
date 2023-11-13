using Abp.Application.Services;
using System.Collections.Generic;
using tmss.Dto;

namespace tmss.Master.VehicleCKD.Exporting
{
    public interface IMasterVehicleCKDExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<MasterVehicleCKDDto> listdata);
    }
}
