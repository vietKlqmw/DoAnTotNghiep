using Abp.Application.Services;
using System.Collections.Generic;
using tmss.Dto;

namespace tmss.MaterialManagement.ContainerInvoice.Exporting
{
    public interface IProdContainerInvoiceExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<ProdContainerInvoiceDto> listdata);
    }
}
