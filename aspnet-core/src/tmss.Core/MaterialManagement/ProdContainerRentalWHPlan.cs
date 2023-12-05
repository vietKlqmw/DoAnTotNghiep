using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.MaterialManagement
{
    [Table("ProdContainerRentalWHPlan")]
    public class ProdContainerRentalWHPlan : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxContainerNoLength = 15;

        public const int MaxInvoiceNoLength = 20;

        public const int MaxBillofladingNoLength = 20;

        public const int MaxSupplierNoLength = 10;

        public const int MaxSealNoLength = 20;

        public const int MaxTransportLength = 50;

        public const int MaxStatusLength = 10;


        [StringLength(MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }

        public virtual DateTime? RequestDate { get; set; }

        public virtual TimeSpan? RequestTime { get; set; }

        [StringLength(MaxInvoiceNoLength)]
        public virtual string InvoiceNo { get; set; }

        [StringLength(MaxBillofladingNoLength)]
        public virtual string BillofladingNo { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxSealNoLength)]
        public virtual string SealNo { get; set; }

        public virtual DateTime? DevanningDate { get; set; }

        public virtual TimeSpan? DevanningTime { get; set; }

        public virtual DateTime? ActualDevanningDate { get; set; }

        public virtual DateTime? GateInPlanTime { get; set; }

        public virtual DateTime? GateInActualDateTime { get; set; }

        [StringLength(MaxTransportLength)]
        public virtual string Transport { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }
    }
}
