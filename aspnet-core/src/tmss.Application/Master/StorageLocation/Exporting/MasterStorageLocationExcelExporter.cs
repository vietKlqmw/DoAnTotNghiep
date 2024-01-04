using System.Collections.Generic;
using tmss.DataExporting.Excel.NPOI;
using tmss.Dto;
using tmss.Storage;

namespace tmss.Master.StorageLocation.Exporting
{
    public class MasterStorageLocationExcelExporter : NpoiExcelExporterBase, IMasterStorageLocationExcelExporter
    {
        public MasterStorageLocationExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }

        public FileDto ExportToFile(List<MasterStorageLocationDto> listdata)
        {
            return CreateExcelPackage(
                "MasterStorageLocation.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("StorageLocation");

                    AddHeader(
                        sheet,
                        ("Warehouse"),
                        ("Address"),
                        ("Category"),
                        ("Max Stock"),
                        ("Inventory"),
                        ("Status")
                    );

                    AddObjects(
                        sheet, 1, listdata,
                        _ => _.StorageLocation,
                        _ => _.AddressLanguageVn,
                        _ => _.Category,
                        _ => _.MaxStock,
                        _ => _.Inventory,
                        _ => _.Status
                    );

                    for (var i = 0; i < 6; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
