using Abp.Application.Services;
using System.Collections.Generic;
using tmss.Dto;

namespace tmss.MaterialManagement.ContainerIntransit.Exporting
{
    public interface IProdContainerIntransitExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<ProdContainerIntransitDto> listdata);
    }
}
