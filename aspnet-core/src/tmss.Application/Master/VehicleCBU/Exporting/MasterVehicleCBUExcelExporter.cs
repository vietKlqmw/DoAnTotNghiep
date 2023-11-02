using System.Collections.Generic;
using tmss.DataExporting.Excel.NPOI;
using tmss.Dto;
using tmss.Storage;

namespace tmss.Master.VehicleCBU.Exporting
{
    public class MasterVehicleCBUExcelExporter : NpoiExcelExporterBase, IMasterVehicleCBUExcelExporter
    {
        public MasterVehicleCBUExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }

        public FileDto ExportToFile(List<MasterVehicleCBUDto> listdata)
        {
            return CreateExcelPackage(
                "MasterVehicleCBU.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("VehicleCBU");

                    AddHeader(
                        sheet,
                        ("Vehicle Type"),
                        ("Model"),
                        ("Marketing Code"),
                        ("Production Code")
                    );

                    AddObjects(
                        sheet, 1, listdata,
                        _ => _.VehicleType,
                        _ => _.Model,
                        _ => _.MarketingCode,
                        _ => _.ProductionCode
                    );

                    for (var i = 0; i < 9; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
