using Abp.Application.Services;
using System.Collections.Generic;
using tmss.Dto;

namespace tmss.Master.StorageLocation.Exporting
{
    public interface IMasterStorageLocationExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<MasterStorageLocationDto> listdata);
    }
}
