using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.Master
{
    [Table("MasterPartList")]
    public class MasterPartList : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxPartNoLength = 50;

        public const int MaxPartNameLength = 10;

        public const int MaxSupplierNoLength = 50;

        public const int MaxCfcLength = 4;

        public const int MaxRemarkLength = 5000;


        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        public virtual long? SupplierId { get; set; }

        public virtual long? MaterialId { get; set; }

        [StringLength(MaxCfcLength)]
        public virtual string CarfamilyCode { get; set; }

        public virtual DateTime? StartProductionMonth { get; set; }

        public virtual DateTime? EndProductionMonth { get; set; }

        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }
    }
}
