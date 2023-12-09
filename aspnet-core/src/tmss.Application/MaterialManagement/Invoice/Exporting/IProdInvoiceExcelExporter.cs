using Abp.Application.Services;
using System.Collections.Generic;
using tmss.Dto;

namespace tmss.MaterialManagement.Invoice.Exporting
{
    public interface IProdInvoiceExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<ProdInvoiceDto> listdata);
        FileDto ExportToFileDetails(List<ProdInvoiceDto> listdatadetails);
    }
}
