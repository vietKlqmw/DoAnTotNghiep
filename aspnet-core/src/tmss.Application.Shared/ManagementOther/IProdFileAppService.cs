﻿using Abp.Application.Services;
using System.Threading.Tasks;

namespace tmss.ManagementOther
{
    public interface IProdFileAppService : IApplicationService
    {
        Task<byte[]> ExportGoodsReceivedNote(GoodsReceivedNoteExportInput input);

        Task<byte[]> ExportGoodsDeliveryNote(GoodsDeliveryNoteExportInput input);

        Task<byte[]> ExportGoodsReceivedNoteHistory(GoodsReceivedNoteHistoryExportInput input);

        Task<byte[]> ExportGoodsDeliveryNoteHistory(InvoiceStockHistoryExportInput input);
    }
}
