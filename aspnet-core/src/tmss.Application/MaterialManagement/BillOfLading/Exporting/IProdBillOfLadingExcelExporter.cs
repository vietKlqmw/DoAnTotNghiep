using Abp.Application.Services;
using System.Collections.Generic;
using tmss.Dto;

namespace tmss.MaterialManagement.BillOfLading.Exporting
{
    public interface IProdBillOfLadingExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<ProdBillOfLadingDto> listdata);
    }
}
