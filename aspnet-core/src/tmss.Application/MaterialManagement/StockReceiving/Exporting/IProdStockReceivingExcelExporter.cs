using Abp.Application.Services;
using System.Collections.Generic;
using tmss.Dto;

namespace tmss.MaterialManagement.StockReceiving.Exporting
{
    public interface IProdStockReceivingExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<ProdStockReceivingDto> listdata);
    }
}
