using System.Collections.Generic;
using tmss.DataExporting.Excel.NPOI;
using tmss.Dto;
using tmss.Storage;

namespace tmss.MaterialManagement.ContainerWH.Exporting
{
    public class ProdContainerRentalWHPlanExcelExporter : NpoiExcelExporterBase, IProdContainerRentalWHPlanExcelExporter
    {
        public ProdContainerRentalWHPlanExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<ProdContainerRentalWHPlanDto> listdata)
        {
            return CreateExcelPackage(
                "ContainerAtWarehouse.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ContainerWarehouse");

                    AddHeader(
                        sheet,
                        ("Container No"),
                        ("Warehouse"),
                        ("Supplier No"),
                        ("Invoice No"),
                        ("Bill Of Lading No"),
                        ("Receive Date"),
                        ("Transport"),
                        ("Goods Received Note No")
                    );

                    AddObjects(
                        sheet, 1, listdata,
                        _ => _.ContainerNo,
                        _ => _.Warehouse,
                        _ => _.SupplierNo,
                        _ => _.InvoiceNo,
                        _ => _.BillofladingNo,
                        _ => _.ReceiveDate,
                        _ => _.Transport,
                        _ => _.GoodsReceivedNoteNo
                    );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
