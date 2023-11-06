using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.Master
{
    [Table("MasterProductGroup")]
    public class MasterProductGroup : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxCodeLength = 10;

        public const int MaxNameLength = 200;

        [StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }
    }
}
