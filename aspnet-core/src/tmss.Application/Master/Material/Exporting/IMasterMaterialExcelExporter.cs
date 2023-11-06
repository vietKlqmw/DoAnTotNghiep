using Abp.Application.Services;
using System.Collections.Generic;
using tmss.Dto;

namespace tmss.Master.Material.Exporting
{
    public interface IMasterMaterialExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<MasterMaterialDto> listdata);
    }
}
