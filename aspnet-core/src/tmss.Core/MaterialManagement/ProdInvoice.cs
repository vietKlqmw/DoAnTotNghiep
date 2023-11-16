using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.MaterialManagement
{
    [Table("ProdInvoice")]
    public class ProdInvoice : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxInvoiceNoLength = 20;

        public const int MaxOrdertypeCodeLength = 4;

        public const int MaxGoodstypeCodeLength = 4;

        public const int MaxCurrencyLength = 20;

        public const int MaxSupplierNoLength = 10;

        public const int MaxStatusLength = 10;

        [StringLength(MaxInvoiceNoLength)]
        public virtual string InvoiceNo { get; set; } //mã hóa đơn

        public virtual long? BillId { get; set; }

        [StringLength(MaxOrdertypeCodeLength)]
        public virtual string OrderTypeCode { get; set; }

        [StringLength(MaxGoodstypeCodeLength)]
        public virtual string GoodsTypeCode { get; set; }

        public virtual long? InvoiceParentId { get; set; }

        public virtual DateTime? InvoiceDate { get; set; }

        public virtual decimal? Freight { get; set; } // Cước vẫn chuyển

        public virtual decimal? FreightTotal { get; set; }

        public virtual decimal? Insurance { get; set; } //Bảo hiểm

        public virtual decimal? InsuranceTotal { get; set; }

        public virtual decimal? Cif { get; set; } //Cost + Insurance + Freight

        public virtual decimal? ThcTotal { get; set; } //phí xếp dỡ bến

        public virtual decimal? NetWeight { get; set; } //khối lượng tịnh

        public virtual decimal? GrossWeight { get; set; } //khối lượng toàn phần

        [StringLength(MaxCurrencyLength)]
        public virtual string Currency { get; set; } //Tiền tệ

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        public virtual int? Quantity { get; set; } //số lượng

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        public virtual decimal? FreightTotalVn { get; set; }

        public virtual decimal? InsuranceTotalVn { get; set; }

        public virtual decimal? CifVn { get; set; }

        public virtual decimal? ThcTotalVn { get; set; }

        public virtual long? PeriodId { get; set; }
    }
}
