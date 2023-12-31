﻿using Dapper;
using GemBox.Spreadsheet;
using Microsoft.Data.SqlClient;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using tmss.MaterialManagement.Invoice;
using tmss.MaterialManagement.StockReceiving;

namespace tmss.ManagementOther
{
    public class ProdFileAppService : tmssAppServiceBase, IProdFileAppService
    {
        private readonly ProdOthersAppService _prodOthersAppService;

        public ProdFileAppService(ProdOthersAppService prodOthersAppService)
        {
            _prodOthersAppService = prodOthersAppService;
        }

        public async Task<byte[]> ExportGoodsReceivedNote(GoodsReceivedNoteExportInput input)
        {
            byte[] fileByteReturn = new byte[] { };
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string p_template = "wwwroot/Excel_Template";
            string path = Path.Combine(Directory.GetCurrentDirectory(), p_template, "GoodsReceivedNote_Template.xlsx");
            var xlWorkBook = ExcelFile.Load(path);
            var xlWorkSheet = xlWorkBook.Worksheets[0];
            string connectionString = Common.Commons.getConnectionString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string _sql = "Exec INV_PROD_EXPORT_GOODS_RECEIVED_NOTE @p_ContId";
                var reader = await conn.QueryMultipleAsync(_sql, new
                {
                    p_ContId = input.ContId
                });
                var listdata = reader.ReadAsync<ProdInvoiceDto>().Result.ToList();
                var listother = reader.ReadAsync<ProdInvoiceDto>().Result.FirstOrDefault();
                conn.Close();

                xlWorkSheet.Cells[1, 3].Value = "Ngày " + input.ReceiveDate.Substring(6, 2) + " tháng " + input.ReceiveDate.Substring(4, 2) + " năm " + input.ReceiveDate.Substring(0, 4);
                xlWorkSheet.Cells[2, 4].Value = input.GoodsReceivedNoteNo.ToUpper();
                xlWorkSheet.Cells[4, 2].Value = listother.Forwarder;
                xlWorkSheet.Cells[5, 2].Value = listother.InvoiceNo;
                xlWorkSheet.Cells[6, 2].Value = input.Warehouse;
                xlWorkSheet.Cells[6, 4].Value = input.Address;

                var currentRow = xlWorkSheet.Rows[12];
                int addrow = 1;
                if (input.ListActualQty != null)
                {
                    if (input.ListActualQty.Length > 9) addrow = input.ListActualQty.Length - 8;
                }
                currentRow.InsertCopy(addrow, xlWorkSheet.Rows[12]);

                int startrow = 11;
                decimal? sum = 0;

                for (int i = 0;i < listdata.Count;i++)
                {
                    xlWorkSheet.Cells[startrow + i, 0].Value = i + "-" + listdata[i].ContainerNo;
                    xlWorkSheet.Cells[startrow + i, 1].Value = listdata[i].PartName;
                    xlWorkSheet.Cells[startrow + i, 3].Value = listdata[i].PartNo;
                    xlWorkSheet.Cells[startrow + i, 4].Value = listdata[i].BaseUnitOfMeasure;
                    xlWorkSheet.Cells[startrow + i, 5].Value = listdata[i].UsageQty;
                    xlWorkSheet.Cells[startrow + i, 6].Value = int.Parse(input.ListActualQty[i]);
                    xlWorkSheet.Cells[startrow + i, 7].Value = listdata[i].StandardPrice;
                    xlWorkSheet.Cells[startrow + i, 8].Value = int.Parse(input.ListActualQty[i]) * listdata[i].StandardPrice;
                }
            }
            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);

            byte[] fileByte = new byte[] { };
            if (input.IsExcel)
            {
                fileByte = await File.ReadAllBytesAsync(tempFile);
            }
            else
            {
                string _savePdf = _prodOthersAppService.ConvertExcelToPdf(tempFile, Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "GoodsReceivedNote_" + input.ReceiveDate) + ".pdf";
                fileByte = await File.ReadAllBytesAsync(_savePdf);
                File.Delete(_savePdf);
            }

            File.Delete(tempFile);

            fileByteReturn = fileByte;
            return fileByteReturn;
        }


        public async Task<byte[]> ExportGoodsDeliveryNote(GoodsDeliveryNoteExportInput input)
        {
            byte[] fileByteReturn = new byte[] { };
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string p_template = "wwwroot/Excel_Template";
            string path = Path.Combine(Directory.GetCurrentDirectory(), p_template, "GoodsDeliveryNote_Template.xlsx");
            var xlWorkBook = ExcelFile.Load(path);
            var xlWorkSheet = xlWorkBook.Worksheets[0];
            string connectionString = Common.Commons.getConnectionString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string _sql = "Exec INV_PROD_EXPORT_GOODS_DELIVERY_NOTE @p_StockId";
                var reader = await conn.QueryMultipleAsync(_sql, new
                {
                    p_StockId = input.StockId
                });
                var listdata = reader.ReadAsync<ProdStockReceivingDto>().Result.ToList();
                var listother = reader.ReadAsync<ProdStockReceivingDto>().Result.FirstOrDefault();
                conn.Close();

                xlWorkSheet.Cells[1, 3].Value = "Ngày " + input.DeliveryDate.Substring(6, 2) + " tháng " + input.DeliveryDate.Substring(4, 2) + " năm " + input.DeliveryDate.Substring(0, 4);
                xlWorkSheet.Cells[2, 4].Value = input.GoodsDeliveryNoteNo;
                xlWorkSheet.Cells[5, 2].Value = listother.InvoiceNoOut;
                xlWorkSheet.Cells[6, 2].Value = input.Warehouse;
                xlWorkSheet.Cells[6, 4].Value = input.Address;

                var currentRow = xlWorkSheet.Rows[12];
                int addrow = 1;
                if (input.ListActualDeliveryQty != null)
                {
                    if (input.ListActualDeliveryQty.Length > 9) addrow = input.ListActualDeliveryQty.Length - 8;
                }
                currentRow.InsertCopy(addrow, xlWorkSheet.Rows[12]);

                int startrow = 11;
                decimal? sum = 0;

                for (int i = 0; i < listdata.Count; i++)
                {
                    xlWorkSheet.Cells[startrow + i, 0].Value = i + 1;
                    xlWorkSheet.Cells[startrow + i, 1].Value = listdata[i].PartName;
                    xlWorkSheet.Cells[startrow + i, 3].Value = listdata[i].PartNo;
                    xlWorkSheet.Cells[startrow + i, 4].Value = listdata[i].BaseUnitOfMeasure;
                    xlWorkSheet.Cells[startrow + i, 5].Value = int.Parse(input.ListDeliveryQty[i]);
                    xlWorkSheet.Cells[startrow + i, 6].Value = int.Parse(input.ListActualDeliveryQty[i]);
                    xlWorkSheet.Cells[startrow + i, 7].Value = listdata[i].StandardPrice;
                    xlWorkSheet.Cells[startrow + i, 8].Value = int.Parse(input.ListActualDeliveryQty[i]) * listdata[i].StandardPrice + listdata[i].MovingPrice;
                }
            }
            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);

            byte[] fileByte = new byte[] { };
            if (input.IsExcel)
            {
                fileByte = await File.ReadAllBytesAsync(tempFile);
            }
            else
            {
                string _savePdf = _prodOthersAppService.ConvertExcelToPdf(tempFile, Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "GoodsDeliveryNote_" + input.DeliveryDate) + ".pdf";
                fileByte = await File.ReadAllBytesAsync(_savePdf);
                File.Delete(_savePdf);
            }

            File.Delete(tempFile);

            fileByteReturn = fileByte;
            return fileByteReturn;
        }


        public async Task<byte[]> ExportGoodsReceivedNoteHistory(GoodsReceivedNoteHistoryExportInput input)
        {
            byte[] fileByteReturn = new byte[] { };
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string p_template = "wwwroot/Excel_Template";
            string path = Path.Combine(Directory.GetCurrentDirectory(), p_template, "GoodsReceivedNote_Template.xlsx");
            var xlWorkBook = ExcelFile.Load(path);
            var xlWorkSheet = xlWorkBook.Worksheets[0];
            string connectionString = Common.Commons.getConnectionString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string _sql = "Exec INV_PROD_HISTORY_RECEIVE @p_GRN";
                var reader = await conn.QueryMultipleAsync(_sql, new
                {
                    p_GRN = input.GoodsReceivedNoteNo
                });
                var listdata = reader.ReadAsync<ProdHistoryDto>().Result.ToList();
                var listother = reader.ReadAsync<ProdHistoryDto>().Result.FirstOrDefault();
                conn.Close();

                xlWorkSheet.Cells[1, 3].Value = "Ngày " + input.ReceiveDate.Substring(6, 2) + " tháng " + input.ReceiveDate.Substring(4, 2) + " năm " + input.ReceiveDate.Substring(0, 4);
                xlWorkSheet.Cells[2, 4].Value = input.GoodsReceivedNoteNo;
                xlWorkSheet.Cells[4, 2].Value = listother.Forwarder;
                xlWorkSheet.Cells[5, 2].Value = listother.Invoice;
                xlWorkSheet.Cells[6, 2].Value = listdata[0].Warehouse;
                xlWorkSheet.Cells[6, 4].Value = listdata[0].AddressLanguageVn;

                var currentRow = xlWorkSheet.Rows[12];
                int addrow = 1;
                if (listdata.Count > 9) addrow = listdata.Count - 8;

                currentRow.InsertCopy(addrow, xlWorkSheet.Rows[12]);

                int startrow = 11;

                for (int i = 0; i < listdata.Count; i++)
                {
                    xlWorkSheet.Cells[startrow + i, 0].Value = i + "-" + listdata[i].ContainerNo;
                    xlWorkSheet.Cells[startrow + i, 1].Value = listdata[i].PartName;
                    xlWorkSheet.Cells[startrow + i, 3].Value = listdata[i].PartNo;
                    xlWorkSheet.Cells[startrow + i, 4].Value = listdata[i].BaseUnitOfMeasure;
                    xlWorkSheet.Cells[startrow + i, 5].Value = listdata[i].UsageQty;
                    xlWorkSheet.Cells[startrow + i, 6].Value = listdata[i].RealQty;
                    xlWorkSheet.Cells[startrow + i, 7].Value = listdata[i].AmountUnit;
                    xlWorkSheet.Cells[startrow + i, 8].Value = listdata[i].Cost;
                }
            }
            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);

            byte[] fileByte = new byte[] { };
            if (input.IsExcel)
            {
                fileByte = await File.ReadAllBytesAsync(tempFile);
            }
            else
            {
                string _savePdf = _prodOthersAppService.ConvertExcelToPdf(tempFile, Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "GoodsReceivedNote_" + input.ReceiveDate) + ".pdf";
                fileByte = await File.ReadAllBytesAsync(_savePdf);
                File.Delete(_savePdf);
            }

            File.Delete(tempFile);

            fileByteReturn = fileByte;
            return fileByteReturn;
        }


        public async Task<byte[]> ExportGoodsDeliveryNoteHistory(InvoiceStockHistoryExportInput input)
        {
            byte[] fileByteReturn = new byte[] { };
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string p_template = "wwwroot/Excel_Template";
            string path = Path.Combine(Directory.GetCurrentDirectory(), p_template, "GoodsDeliveryNote_Template.xlsx");
            var xlWorkBook = ExcelFile.Load(path);
            var xlWorkSheet = xlWorkBook.Worksheets[0];
            string connectionString = Common.Commons.getConnectionString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string _sql = "Exec INV_PROD_HISTORY_DELIVERY @p_Invoice";
                var reader = await conn.QueryMultipleAsync(_sql, new
                {
                    p_Invoice = input.Invoice
                });
                var listdata = reader.ReadAsync<ProdInvoiceStockHistoryDto>().Result.ToList();
                var listother = reader.ReadAsync<ProdInvoiceStockHistoryDto>().Result.FirstOrDefault();
                conn.Close();

                xlWorkSheet.Cells[1, 3].Value = "Ngày " + input.DeliveryDate.Substring(6, 2) + " tháng " + input.DeliveryDate.Substring(4, 2) + " năm " + input.DeliveryDate.Substring(0, 4);
                xlWorkSheet.Cells[2, 4].Value = input.GoodsDeliveryNoteNo;
                xlWorkSheet.Cells[5, 2].Value = listother.InvoiceNoOut;
                xlWorkSheet.Cells[6, 2].Value = input.Warehouse;
                xlWorkSheet.Cells[6, 4].Value = input.Address;

                var currentRow = xlWorkSheet.Rows[12];
                int addrow = 1;

                if (listdata.Count > 9) addrow = listdata.Count - 8;

                currentRow.InsertCopy(addrow, xlWorkSheet.Rows[12]);

                int startrow = 11;

                for (int i = 0; i < listdata.Count; i++)
                {
                    xlWorkSheet.Cells[startrow + i, 0].Value = i + 1;
                    xlWorkSheet.Cells[startrow + i, 1].Value = listdata[i].ListPartName;
                    xlWorkSheet.Cells[startrow + i, 3].Value = listdata[i].ListPartNo;
                    xlWorkSheet.Cells[startrow + i, 4].Value = listdata[i].BaseUnitOfMeasure;
                    xlWorkSheet.Cells[startrow + i, 5].Value = listdata[i].TotalOrderQty;
                    xlWorkSheet.Cells[startrow + i, 6].Value = listdata[i].TotalDeliveryQty;
                    xlWorkSheet.Cells[startrow + i, 7].Value = listdata[i].StandardPrice;
                    xlWorkSheet.Cells[startrow + i, 8].Value = listdata[i].TotalAmount;
                }
            }
            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);

            byte[] fileByte = new byte[] { };
            if (input.IsExcel)
            {
                fileByte = await File.ReadAllBytesAsync(tempFile);
            }
            else
            {
                string _savePdf = _prodOthersAppService.ConvertExcelToPdf(tempFile, Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "GoodsDeliveryNote_" + input.DeliveryDate) + ".pdf";
                fileByte = await File.ReadAllBytesAsync(_savePdf);
                File.Delete(_savePdf);
            }

            File.Delete(tempFile);

            fileByteReturn = fileByte;
            return fileByteReturn;
        }
    }
}
