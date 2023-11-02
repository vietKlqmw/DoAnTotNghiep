using Abp.Application.Services;
using System.Collections.Generic;
using tmss.Dto;

namespace tmss.Master.VehicleCBU.Exporting
{
    public interface IMasterVehicleCBUExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<MasterVehicleCBUDto> listdata);
    }
}
