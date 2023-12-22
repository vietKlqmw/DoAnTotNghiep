using Abp.Application.Services;
using System.Collections.Generic;
using tmss.Dto;

namespace tmss.MaterialManagement.OrderPart.Exporting
{
    public interface IProdOrderPartExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<ProdOrderPartDto> listdata);
    }
}
