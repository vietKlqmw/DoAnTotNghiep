using System.Collections.Generic;
using tmss.DataExporting.Excel.NPOI;
using tmss.Dto;
using tmss.Storage;

namespace tmss.Master.VehicleCKD.Exporting
{
    public class MasterVehicleCKDExcelExporter : NpoiExcelExporterBase, IMasterVehicleCKDExcelExporter
    {
        public MasterVehicleCKDExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MasterVehicleCKDDto> listdata)
        {
            return CreateExcelPackage(
                "MasterVehicleCKD.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("VehicleCKD");

                    AddHeader(
                        sheet,
                        ("Model"),
                        ("Lot Code"),
                        ("Cfc"),
                        ("Grade"),
                        ("Grade Name"),
                        ("Model Code"),
                        ("Transmission Type"),
                        ("Engine Type"),
                        ("Fuel Type")
                    );

                    AddObjects(
                        sheet, 1, listdata,
                        _ => _.Model,
                        _ => _.LotCode,
                        _ => _.Cfc,
                        _ => _.Grade,
                        _ => _.GradeName,
                        _ => _.ModelCode,
                        _ => _.TransmissionType,
                        _ => _.EngineType,
                        _ => _.FuelType
                    );

                    for (var i = 0; i < 9; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
