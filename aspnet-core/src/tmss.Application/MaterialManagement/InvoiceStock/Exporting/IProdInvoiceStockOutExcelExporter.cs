using Abp.Application.Services;
using System.Collections.Generic;
using tmss.Dto;

namespace tmss.MaterialManagement.InvoiceStock.Exporting
{
    public interface IProdInvoiceStockOutExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<ProdInvoiceStockOutDto> listdata);
    }
}
