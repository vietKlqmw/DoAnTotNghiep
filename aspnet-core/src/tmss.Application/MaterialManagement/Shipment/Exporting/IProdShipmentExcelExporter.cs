using Abp.Application.Services;
using System.Collections.Generic;
using tmss.Dto;

namespace tmss.MaterialManagement.Shipment.Exporting
{
    public interface IProdShipmentExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<ProdShipmentDto> listdata);
    }
}
