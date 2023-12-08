using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.MaterialManagement
{
    [Table("ProdInvoiceDetails")]
    public class ProdInvoiceDetails : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxInvoiceNoLength = 20;

        public const int MaxPartNoLength = 12;

        public const int MaxContainerNoLength = 15;

        public const int MaxSupplierNoLength = 50;

        public const int MaxPartNameLength = 300;

        public const int MaxCarfamilyCodeLength = 4;

        public const int MaxCurrencyLength = 20;

        [StringLength(MaxInvoiceNoLength)]
        public virtual string InvoiceNo { get; set; } //mã hóa đơn

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        public virtual decimal? Insurance { get; set; } //Bảo hiểm

        [StringLength(MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        public virtual decimal? Freight { get; set; } // Cước vẫn chuyển

        public virtual decimal? Thc { get; set; } //phí xếp dỡ bến

        public virtual decimal? Cif { get; set; }  //Cost + Insurance + Freight

        public virtual decimal? Tax { get; set; } //thuế

        public virtual decimal? TaxRate { get; set; } //% thuế

        public virtual decimal? Vat { get; set; } //giá trị gia tăng

        public virtual decimal? VatRate { get; set; } // % GTGT

        public virtual int? UsageQty { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MaxCarfamilyCodeLength)]
        public virtual string CarfamilyCode { get; set; }

        public virtual decimal? GrossWeight { get; set; } //khối lượng toàn phần

        [StringLength(MaxCurrencyLength)]
        public virtual string Currency { get; set; } //Tiền tệ

    }
}
