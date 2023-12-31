﻿using Abp.AspNetZeroCore.Net;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using tmss.ManagementOther;

namespace tmss.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    public class ProdFileController : tmssControllerBase
    {
        private readonly IProdFileAppService _prodFileAppService;

        public ProdFileController(IProdFileAppService prodFileAppService)
        {
            _prodFileAppService = prodFileAppService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> ExportGoodsReceivedNoteExcel([FromBody] GoodsReceivedNoteExportInput input)
        {
            string fileName = "GoodsReceivedNote_" + input.ReceiveDate + ".xlsx";
            return File(await _prodFileAppService.ExportGoodsReceivedNote(input), MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, fileName);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> ExportGoodsReceivedNotePdf([FromBody] GoodsReceivedNoteExportInput input)
        {
            string fileName = "GoodsReceivedNote_" + input.ReceiveDate + ".pdf";
            return File(await _prodFileAppService.ExportGoodsReceivedNote(input), MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, fileName);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> ExportGoodsDeliveryNoteExcel([FromBody] GoodsDeliveryNoteExportInput input)
        {
            string fileName = "GoodsDeliveryNote_" + input.DeliveryDate + ".xlsx";
            return File(await _prodFileAppService.ExportGoodsDeliveryNote(input), MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, fileName);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> ExportGoodsDeliveryNotePdf([FromBody] GoodsDeliveryNoteExportInput input)
        {
            string fileName = "GoodsDeliveryNote_" + input.DeliveryDate + ".pdf";
            return File(await _prodFileAppService.ExportGoodsDeliveryNote(input), MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, fileName);
        }


        [HttpPost("[action]")]
        public async Task<ActionResult> ExportGoodsReceivedNoteHistoryExcel([FromBody] GoodsReceivedNoteHistoryExportInput input)
        {
            string fileName = "GoodsReceivedNote_" + input.ReceiveDate + ".xlsx";
            return File(await _prodFileAppService.ExportGoodsReceivedNoteHistory(input), MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, fileName);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> ExportGoodsReceivedNoteHistoryPdf([FromBody] GoodsReceivedNoteHistoryExportInput input)
        {
            string fileName = "GoodsReceivedNote_" + input.ReceiveDate + ".pdf";
            return File(await _prodFileAppService.ExportGoodsReceivedNoteHistory(input), MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, fileName);
        }


        [HttpPost("[action]")]
        public async Task<ActionResult> ExportGoodsDeliveryNoteHistoryExcel([FromBody] InvoiceStockHistoryExportInput input)
        {
            string fileName = "GoodsDeliveryNote_" + input.DeliveryDate + ".xlsx";
            return File(await _prodFileAppService.ExportGoodsDeliveryNoteHistory(input), MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, fileName);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> ExportGoodsDeliveryNoteHistoryPdf([FromBody] InvoiceStockHistoryExportInput input)
        {
            string fileName = "GoodsDeliveryNote_" + input.DeliveryDate + ".pdf";
            return File(await _prodFileAppService.ExportGoodsDeliveryNoteHistory(input), MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, fileName);
        }
    }
}
