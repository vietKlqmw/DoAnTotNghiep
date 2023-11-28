using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.MaterialManagement
{
    [Table("ProdStockPart")]
    public class ProdStockPart : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxPartNoLength = 12;

        public const int MaxPartNoNormalizedLength = 12;

        public const int MaxPartNameLength = 300;

        public const int MaxPartNoNormalizedS4Length = 10;

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNoNormalizedLength)]
        public virtual string PartNoNormalized { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MaxPartNoNormalizedS4Length)]
        public virtual string PartNoNormalizedS4 { get; set; }

        public virtual long? PartListId { get; set; }

        public virtual long? MaterialId { get; set; }

        public virtual decimal? Qty { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        public virtual DateTime? LastCalDatetime { get; set; }
    }
}
