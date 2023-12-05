using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.MaterialManagement
{
    [Table("ProdContainerTransitPortPlan_T")]
    public class ProdContainerTransitPortPlan_T : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxContainerNoLength = 15;

        public const int MaxInvoiceNoLength = 20;

        public const int MaxBillOfLadingNoLength = 20;

        public const int MaxSupplierNoLength = 10;

        public const int MaxSealNoLength = 20;

        public const int MaxTransportLength = 50;

        public const int MaxStatusLength = 10;

        public const int MaxCustomsLength = 100;

        [StringLength(128)]
        public virtual string Guid { get; set; }

        [StringLength(MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }

        public virtual DateTime? RequestDate { get; set; }

        public virtual TimeSpan? RequestTime { get; set; }

        [StringLength(MaxInvoiceNoLength)]
        public virtual string InvoiceNo { get; set; }

        [StringLength(MaxBillOfLadingNoLength)]
        public virtual string BillOfLadingNo { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxSealNoLength)]
        public virtual string SealNo { get; set; }

        [StringLength(MaxTransportLength)]
        public virtual string Transport { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(MaxCustomsLength)]
        public virtual string Customs1 { get; set; }

        [StringLength(MaxCustomsLength)]
        public virtual string Customs2 { get; set; }

        [StringLength(5000)]
        public string ErrorDescription { get; set; }
    }
}
