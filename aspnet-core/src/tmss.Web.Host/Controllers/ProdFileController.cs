using Abp.AspNetZeroCore.Net;
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
        public async Task<ActionResult> ExportGoodsDeliveryNoteExcel([FromBody] GoodsReceivedNoteExportInput input)
        {
            string fileName = "GoodsDeliveryNote_" + input.ReceiveDate + ".xlsx";
            return File(await _prodFileAppService.ExportGoodsDeliveryNote(input), MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, fileName);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> ExportGoodsDeliveryNotePdf([FromBody] GoodsReceivedNoteExportInput input)
        {
            string fileName = "GoodsDeliveryNote_" + input.ReceiveDate + ".pdf";
            return File(await _prodFileAppService.ExportGoodsDeliveryNote(input), MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, fileName);
        }
    }
}
