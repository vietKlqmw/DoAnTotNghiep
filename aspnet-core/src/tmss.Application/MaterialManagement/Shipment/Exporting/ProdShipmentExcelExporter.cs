using System.Collections.Generic;
using tmss.DataExporting.Excel.NPOI;
using tmss.Dto;
using tmss.Storage;

namespace tmss.MaterialManagement.Shipment.Exporting
{
    public class ProdShipmentExcelExporter : NpoiExcelExporterBase, IProdShipmentExcelExporter
    {
        public ProdShipmentExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<ProdShipmentDto> listdata)
        {
            return CreateExcelPackage(
                "Shipment.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Shipment");

                    AddHeader(
                        sheet,
                        ("Shipment No"),
                        ("Shipping Company Code"),
                        ("Supplier No"),
                        ("Buyer"),
                        ("From Port"),
                        ("To Port"),
                        ("Shipment Date"),
                        ("ETD"),
                        ("ETA"),
                        ("ATA"),
                        ("Ocean Vessel Name"),
                        ("ATD"),
                        ("Status")
                    );

                    AddObjects(
                        sheet, 1, listdata,
                        _ => _.ShipmentNo,
                        _ => _.ShippingcompanyCode,
                        _ => _.SupplierNo,
                        _ => _.Buyer,
                        _ => _.FromPort,
                        _ => _.ToPort,
                        _ => _.ShipmentDate,
                        _ => _.Etd,
                        _ => _.Eta,
                        _ => _.Ata,
                        _ => _.OceanVesselName,
                        _ => _.Atd,
                        _ => _.Status
                    );

                    for (var i = 0; i < 12; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
