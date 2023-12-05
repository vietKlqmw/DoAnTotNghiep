using System.Collections.Generic;
using tmss.DataExporting.Excel.NPOI;
using tmss.Dto;
using tmss.Storage;

namespace tmss.MaterialManagement.ContainerList.Exporting
{
    public class ProdContainerListExcelExporter : NpoiExcelExporterBase, IProdContainerListExcelExporter
    {
        public ProdContainerListExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<ProdContainerListDto> listdata)
        {
            return CreateExcelPackage(
                "ContainerList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ContainerList");

                    AddHeader(
                        sheet,
                        "Request Status",
                        "Container No",
                        "Supplier No",
                        "Status",
                        "Bill Of Lading No",
                        "Bill Date",
                        "Seal No",
                        "Container Size",
                        "Shipping Date (ETD)",
                        "Port Date (ETA)",
                        "Recieve Date",
                        "Port Date Actual(ATA)",
                        "Port Transit Date",
                        "Invoice No",
                        "Transport",
                        "Devanning Date",
                        "Devanning Time",
                        "Remark",
                        "Warehouse Location",
                        "Gate In Date",
                        "Gate In Time",
                        "Transit Port Request Date",
                        "Transit Port Request Time",
                        "Freight",
                        "Insurance",
                        "C.I.F",
                        "TAX",
                        "Amount",
                        "Location Code",
                        "Location Date"
                    );

                    AddObjects(
                        sheet, 1, listdata,
                        _ => _.RequestStatus,
                        _ => _.ContainerNo,
                        _ => _.SupplierNo,
                        _ => _.Status,
                        _ => _.BillOfLadingNo,
                        _ => _.BillDate,
                        _ => _.SealNo,
                        _ => _.ContainerSize,
                        _ => _.ShippingDate,
                        _ => _.PortDate,
                        _ => _.ReceiveDate,
                        _ => _.PortDateActual,
                        _ => _.PortTransitDate,
                        _ => _.InvoiceNo,
                        _ => _.Transport,
                        _ => _.DevanningDate,
                        _ => _.DevanningTime,
                        _ => _.Remark,
                        _ => _.WhLocation,
                        _ => _.GateInDate,
                        _ => _.GateInTime,
                        _ => _.TransitPortReqDate,
                        _ => _.TransitPortReqTime,
                        _ => _.Freight,
                        _ => _.Insurance,
                        _ => _.Cif,
                        _ => _.Tax,
                        _ => _.Amount,
                        _ => _.LocationCode,
                        _ => _.LocationDate
                    );

                    for (var i = 0; i < 30; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
