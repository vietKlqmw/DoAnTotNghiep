using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.MaterialManagement
{
    [Table("ProdOrderPart")]
    public class ProdOrderPart : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxPartNoLength = 15;

        public const int MaxPartNameLength = 500;

        public const int MaxSupplierNoLength = 10;

        public const int MaxCfcLength = 4;

        public const int MaxRemarkLength = 5000;

        public const int MaxStatusLength = 50;

        public const int MaxContainerNoLength = 20;


        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxCfcLength)]
        public virtual string CarfamilyCode { get; set; }

        public virtual long? SupplierId { get; set; }

        public virtual long? MaterialId { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        public virtual long? ShipmentId { get; set; }

        [StringLength(MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }

        public virtual int? Qty { get; set; }

        public virtual decimal? AmountUnit { get; set; }

        public virtual decimal? TotalAmount { get; set; }

        public virtual DateTime? OrderDate { get; set; }
    }
}
