using Abp.Application.Services;
using System.Collections.Generic;
using tmss.Dto;

namespace tmss.MaterialManagement.CustomsDeclare.Exporting
{
    public interface IProdCustomsDeclareExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<ProdCustomsDeclareDto> listdata);
    }
}
