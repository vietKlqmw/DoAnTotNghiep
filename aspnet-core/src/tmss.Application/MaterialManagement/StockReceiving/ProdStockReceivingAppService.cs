using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tmss.Dto;
using tmss.ManagementOther;
using tmss.MaterialManagement.StockReceiving.Exporting;

namespace tmss.MaterialManagement.StockReceiving
{
    public class ProdStockReceivingAppService : tmssAppServiceBase, IProdStockReceivingAppService
    {
        private readonly IDapperRepository<ProdStockReceiving, long> _dapperRepo;
        private readonly IProdStockReceivingExcelExporter _excelExporter;

        public ProdStockReceivingAppService(
            IDapperRepository<ProdStockReceiving, long> dapperRepo,
            IProdStockReceivingExcelExporter excelExporter
            )
        {
            _dapperRepo = dapperRepo;
            _excelExporter = excelExporter;
        }

        public async Task<PagedResultDto<ProdStockReceivingDto>> GetProdStockReceivingSearch(GetProdStockReceivingInput input)
        {
            string _sql = "Exec INV_PROD_STOCK_RECEIVING_SEARCH @p_PartNo, @p_RequestDateFrom, @p_RequestDateTo, @p_SupplierNo, @p_Model, @p_Warehouse, @p_StockStatus";

            IEnumerable<ProdStockReceivingDto> result = await _dapperRepo.QueryAsync<ProdStockReceivingDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_RequestDateFrom = input.RequestDateFrom,
                p_RequestDateTo = input.RequestDateTo,
                p_SupplierNo = input.SupplierNo,
                p_Model = input.Model,
                p_Warehouse = input.Warehouse,
                p_StockStatus = input.StockStatus
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            if (listResult.Count > 0)
            {
                listResult[0].GrandQty = listResult.Sum(e => e.Qty);
                listResult[0].GrandActualQty = listResult.Sum(e => e.ActualQty);
                listResult[0].GrandOrderQty = listResult.Sum(e => e.OrderQty);
                listResult[0].GrandRemainQty = listResult.Sum(e => e.RemainQty);
                listResult[0].GrandOrderedQty = listResult.Sum(e => e.OrderedQty);
            }

            return new PagedResultDto<ProdStockReceivingDto>(totalCount, pagedAndFiltered);
        }

        public async Task<FileDto> GetProdStockReceivingToExcel(GetProdStockReceivingExportInput input)
        {
            string _sql = "Exec INV_PROD_STOCK_RECEIVING_SEARCH @p_PartNo, @p_RequestDateFrom, @p_RequestDateTo, @p_SupplierNo, @p_Model, @p_Warehouse, @p_StockStatus";

            IEnumerable<ProdStockReceivingDto> result = await _dapperRepo.QueryAsync<ProdStockReceivingDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_RequestDateFrom = input.RequestDateFrom,
                p_RequestDateTo = input.RequestDateTo,
                p_SupplierNo = input.SupplierNo,
                p_Model = input.Model,
                p_Warehouse = input.Warehouse,
                p_StockStatus = input.StockStatus
            });

            var exportToExcel = result.ToList();

            return _excelExporter.ExportToFile(exportToExcel);
        }

        public async Task ConfirmPurchaseOrder(GetPurchaseOrderInputDto input)
        {
            string _sql = "Exec INV_PROD_CONFIRM_PURCHASE_ORDER @p_InvoiceOut, @p_RequestDate, @p_ListOrder, @p_UserId";

            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_InvoiceOut = input.InvoiceNoOut,
                p_RequestDate = input.RequestDate,
                p_ListOrder = input.ListOrder,
                p_UserId = AbpSession.UserId
            });
        }


        public async Task UpdateOrderQtyStock(int? StockId, int? OrderQty)
        {
            string _sql = "Exec INV_PROD_UPDATE_ORDER_QTY_STOCK @p_StockId, @p_OrderQty, @p_UserId";

            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_StockId = StockId,
                p_OrderQty = OrderQty,
                p_UserId = AbpSession.UserId
            });
        }


        public async Task AddGdn(GoodsDeliveryNoteExportInput input)
        {
            string _sql = "Exec INV_PROD_STOCK_ADD_GOODS_DELIVERY_NOTE @p_GdnNo, @p_InvoiceOutDate, @p_ListStockId, @p_UserId";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_GdnNo = input.GoodsDeliveryNoteNo,
                p_InvoiceOutDate = input.InvoiceDate,
                p_ListStockId = input.ListStockId,
                p_UserId = AbpSession.UserId
            });
        }
    }
}
