using Abp.Application.Services.Dto;
using System;

namespace tmss.MaterialManagement.InvoiceStock
{
    public class ProdInvoiceStockOutDto : EntityDto<long?>
    {
        public virtual string InvoiceNoOut { get; set; }
        public virtual DateTime? InvoiceDate { get; set; }
        public virtual string Status { get; set; }
        public virtual string ListPartNo { get; set; }
        public virtual string ListPartName { get; set; }
        public virtual string ListCfc { get; set; }
        public virtual string ListStockId { get; set; }
        public virtual int? TotalOrderQty { get; set; }
        public virtual decimal? TotalAmount { get; set; }
        public virtual string Warehouse { get; set; }

        public virtual int? GrandTotalOrderQty { get; set; }
        public virtual decimal? GrandTotalAmount { get; set; }
    }

    public class GetProdInvoiceStockOutInput : PagedAndSortedResultRequestDto
    {
        public virtual string InvoiceNoOut { get; set; }
        public virtual DateTime? InvoiceDateFrom { get; set; }
        public virtual DateTime? InvoiceDateTo { get; set; }
        public virtual string Status { get; set; }
        public virtual string Warehouse { get; set; }
    }

    public class GetProdInvoiceStockOutExportInput
    {
        public virtual string InvoiceNoOut { get; set; }
        public virtual DateTime? InvoiceDateFrom { get; set; }
        public virtual DateTime? InvoiceDateTo { get; set; }
        public virtual string Status { get; set; }
        public virtual string Warehouse { get; set; }
    }
}
