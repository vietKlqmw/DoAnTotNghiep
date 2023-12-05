using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
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

        public const int MaxStatusLength = 10;

        public const int MaxPartnameVnLength = 300;

        [StringLength(MaxInvoiceNoLength)]
        public virtual string InvoiceNo { get; set; } //mã hóa đơn

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        public virtual decimal? Insurance { get; set; }

        [StringLength(MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }

        public virtual long? InvoiceId { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        public virtual decimal? Freight { get; set; }

        public virtual decimal? Thc { get; set; }

        public virtual decimal? Cif { get; set; }

        public virtual decimal? Tax { get; set; } //thuế

        public virtual decimal? TaxRate { get; set; } //% thuế

        public virtual decimal? Vat { get; set; } //giá trị gia tăng

        public virtual decimal? VatRate { get; set; } // % GTGT

        public virtual int? UsageQty { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MaxCarfamilyCodeLength)]
        public virtual string CarfamilyCode { get; set; }

        public virtual decimal? PartNetWeight { get; set; } //khối lượng tịnh của part

        public virtual DateTime? PackagingDate { get; set; } //ngày đóng gói

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        public virtual decimal? FreightVn { get; set; }

        public virtual decimal? InsuranceVn { get; set; }

        public virtual decimal? ThcVn { get; set; }

        public virtual decimal? CifVn { get; set; }

        public virtual decimal? TaxVn { get; set; }

        public virtual decimal? VatVn { get; set; }

        [StringLength(MaxPartnameVnLength)]
        public virtual string PartnameVn { get; set; }

    }
}
