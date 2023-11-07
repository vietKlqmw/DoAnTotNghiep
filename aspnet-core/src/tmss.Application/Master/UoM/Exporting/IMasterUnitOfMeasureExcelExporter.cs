using Abp.Application.Services;
using System.Collections.Generic;
using tmss.Dto;

namespace tmss.Master.UoM.Exporting
{
    public interface IMasterUnitOfMeasureExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<MasterUnitOfMeasureDto> listdata);
    }
}
