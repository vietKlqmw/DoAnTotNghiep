using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.Master.CustomsStatus
{
    [Table("MasterCustomsStatus")]
    public class MasterCustomsStatus : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxCodeLength = 4;

        public const int MaxDescriptionLength = 50;

        [StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MaxDescriptionLength)]
        public virtual string Description { get; set; }
    }
}
