using Abp.Application.Services.Dto;
using System;

namespace tmss.MaterialManagement.ContainerInvoice
{
    public class ProdContainerInvoiceDto : EntityDto<long?>
    {
        public virtual string ContainerNo { get; set; }
        public virtual long? InvoiceId { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string SealNo { get; set; } //mã niêm chì -->gg search
        public virtual int? ContainerSize { get; set; } //kích thước container
        public virtual DateTime? PlanDevanningDate { get; set; } //ngày dỡ container theo kế hoạch
        public virtual DateTime? ActualDevanningDate { get; set; } //ngày dỡ container thực tế
        public virtual decimal? Thc { get; set; }
        public virtual string Status { get; set; }
        public virtual decimal? ThcVn { get; set; }
        public virtual decimal? Freight { get; set; } // Cước vẫn chuyển
        public virtual decimal? Insurance { get; set; } //Bảo hiểm
        public virtual decimal? Tax { get; set; } //thuế
        public virtual decimal? Amount { get; set; } //số lượng
        public virtual decimal? TaxVnd { get; set; }
        public virtual decimal? VatVnd { get; set; }
        public virtual string BillofladingNo { get; set; }
        public virtual DateTime? BillDate { get; set; }
        public virtual string InvoiceNo { get; set; }
    }

    public class GetProdContainerInvoiceInput : PagedAndSortedResultRequestDto
    {
        public virtual string BillNo { get; set; }

        public virtual string ContainerNo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string SealNo { get; set; }

        public virtual string Status { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual DateTime? BillDateFrom { get; set; }

        public virtual DateTime? BillDateTo { get; set; }
    }

    public class GetProdContainerInvoiceExportInput
    {
        public virtual string BillNo { get; set; }

        public virtual string ContainerNo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string SealNo { get; set; }

        public virtual string Status { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual DateTime? BillDateFrom { get; set; }

        public virtual DateTime? BillDateTo { get; set; }
    }
}
