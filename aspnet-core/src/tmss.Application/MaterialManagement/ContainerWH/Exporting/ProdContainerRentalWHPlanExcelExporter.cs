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
                        ("Request Date"),
                        ("Request Time"),
                        ("Invoice No"),
                        ("Bill Of Lading No"),
                        ("Supplier No"),
                        ("Seal No"),
                        ("Devanning Date"),
                        ("Devanning Time"),
                        ("Actual Devanning Date"),
                        ("Gate In Plan Time"),
                        ("Gate In Actual Date Time"),
                        ("Transport"),
                        ("Status")
                    );

                    AddObjects(
                        sheet, 1, listdata,
                        _ => _.ContainerNo,
                        _ => _.RequestDate,
                        _ => _.RequestTime,
                        _ => _.InvoiceNo,
                        _ => _.BillofladingNo,
                        _ => _.SupplierNo,
                        _ => _.SealNo,
                        _ => _.DevanningDate,
                        _ => _.DevanningTime,
                        _ => _.ActualDevanningDate,
                        _ => _.GateInPlanTime,
                        _ => _.GateInActualDateTime,
                        _ => _.Transport,
                        _ => _.Status
                    );

                    for (var i = 0; i < 14; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
