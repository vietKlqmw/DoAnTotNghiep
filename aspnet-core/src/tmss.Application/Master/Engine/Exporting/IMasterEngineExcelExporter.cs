using Abp.Application.Services;
using System.Collections.Generic;
using tmss.Dto;

namespace tmss.Master.Engine.Exporting
{
    public interface IMasterEngineExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<MasterEngineDto> listdata);
    }
}
