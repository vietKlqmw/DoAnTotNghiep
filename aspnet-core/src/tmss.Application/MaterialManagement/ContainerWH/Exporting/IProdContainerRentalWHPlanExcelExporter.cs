using Abp.Application.Services;
using System.Collections.Generic;
using tmss.Dto;

namespace tmss.MaterialManagement.ContainerWH.Exporting
{
    public interface IProdContainerRentalWHPlanExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<ProdContainerRentalWHPlanDto> listdata);
    }
}
