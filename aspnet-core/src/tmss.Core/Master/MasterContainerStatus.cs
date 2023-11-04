using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.Master
{
    [Table("MasterContainerStatus")]
    public class MasterContainerStatus : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxCodeLength = 12;

        public const int MaxDescriptionLength = 50;

        public const int MaxDescriptionVnLength = 30;

        [StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MaxDescriptionLength)]
        public virtual string Description { get; set; }

        [StringLength(MaxDescriptionVnLength)]
        public virtual string DescriptionVn { get; set; }
    }
}
