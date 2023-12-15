using Abp.Application.Services.Dto;
using System;

namespace tmss.MaterialManagement.StockReceiving
{
    public class ProdStockReceivingDto : EntityDto<long?>
    {
        public virtual string PartNo { get; set; }
        public virtual string PartName { get; set; }
        public virtual long? PartListId { get; set; }
        public virtual long? MaterialId { get; set; }
        public virtual int? Qty { get; set; }
        public virtual DateTime? TransactionDatetime { get; set; }
        public virtual long? InvoiceDetailsId { get; set; }
        public virtual DateTime? WorkingDate { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string Model { get; set; }
        public virtual string ContainerNo { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual int? ActualQty { get; set; }
        public virtual int? OrderQty { get; set; }
        public virtual string InvoiceNoOut { get; set; }
        public virtual string RequestStatus { get; set; }
        public virtual DateTime? RequestDate { get; set; }
        public virtual DateTime? DeliveryDate { get; set; }
        public virtual string Warehouse { get; set; }
        public virtual int? OrderedQty { get; set; }
        public virtual int? RemainQty { get; set; }
        public virtual decimal? StandardPrice { get; set; }//Giá chuẩn
        public virtual decimal? MovingPrice { get; set; }//Giá vận chuyển
        public virtual decimal? AmountOrder { get; set; }

        public virtual int? GrandQty { get; set; }
        public virtual int? GrandActualQty { get; set; }
        public virtual int? GrandOrderQty { get; set; }
        public virtual int? GrandOrderedQty { get; set; }
        public virtual int? GrandRemainQty { get; set; }
    }

    public class GetProdStockReceivingInput : PagedAndSortedResultRequestDto
    {
        public virtual string PartNo { get; set; }
        public virtual DateTime? RequestDateFrom { get; set; }
        public virtual DateTime? RequestDateTo { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string Model { get; set; }
        public virtual string Warehouse { get; set; }
        public virtual string StockStatus { get; set; }
    }

    public class GetProdStockReceivingExportInput
    {
        public virtual string PartNo { get; set; }
        public virtual DateTime? RequestDateFrom { get; set; }
        public virtual DateTime? RequestDateTo { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string Model { get; set; }
        public virtual string Warehouse { get; set; }
        public virtual string StockStatus { get; set; }
    }

    public class GetPurchaseOrderInputDto
    {
        public virtual string InvoiceNoOut { get; set; }
        public virtual DateTime? RequestDate { get; set; }
        public virtual string ListOrder { get; set; }
    }
}
