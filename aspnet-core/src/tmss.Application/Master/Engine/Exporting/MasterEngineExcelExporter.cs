using System.Collections.Generic;
using tmss.DataExporting.Excel.NPOI;
using tmss.Dto;
using tmss.Storage;

namespace tmss.Master.Engine.Exporting
{
    public class MasterEngineExcelExporter : NpoiExcelExporterBase, IMasterEngineExcelExporter
    {
        public MasterEngineExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MasterEngineDto> listdata)
        {
            return CreateExcelPackage(
                "MasterEngineDto.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("EngineMaster");

                    AddHeader(
                        sheet,
                        ("Material Code"),
                        ("Class Type"),
                        ("Class Name"),
                        ("Transmission Type"),
                        ("Engine Model"),
                        ("Engine Type")
                    );

                    AddObjects(
                        sheet, 1, listdata,
                        _ => _.MaterialCode,
                        _ => _.ClassType,
                        _ => _.ClassName,
                        _ => _.TransmissionType,
                        _ => _.EngineModel,
                        _ => _.EngineType
                    );

                    for (var i = 0; i < 6; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
