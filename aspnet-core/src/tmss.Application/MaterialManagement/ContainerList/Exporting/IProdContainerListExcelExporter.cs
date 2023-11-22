using Abp.Application.Services;
using System.Collections.Generic;
using tmss.Dto;

namespace tmss.MaterialManagement.ContainerList.Exporting
{
    public interface IProdContainerListExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<ProdContainerListDto> listdata);
    }
}
