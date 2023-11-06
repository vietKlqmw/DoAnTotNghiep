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
                        ("Plant Code"),
                        ("Plant Name"),
                        ("Storage Location"),
                        ("Storage Location Name"),
                        ("Address Language En"),
                        ("Address Language Vn"),
                        ("Category")
                    );

                    AddObjects(
                        sheet, 1, listdata,
                        _ => _.PlantCode,
                        _ => _.PlantName,
                        _ => _.StorageLocation,
                        _ => _.StorageLocationName,
                        _ => _.AddressLanguageEn,
                        _ => _.AddressLanguageVn,
                        _ => _.Category
                    );

                    for (var i = 0; i < 9; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
