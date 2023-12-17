using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.MaterialManagement
{
    [Table("ProdContainerIntransit_T")]
    public class ProdContainerIntransit_T : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxContainerNoLength = 20;

        public const int MaxSupplierNoLength = 10;

        public const int MaxPartNoLength = 15;

        public const int MaxCfcLength = 4;

        [StringLength(128)]
        public virtual string Guid { get; set; }

        [StringLength(MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        public virtual int? UsageQty { get; set; }

        [StringLength(5000)]
        public string ErrorDescription { get; set; }

        [StringLength(MaxCfcLength)]
        public virtual string CarfamilyCode { get; set; }
    }
}
