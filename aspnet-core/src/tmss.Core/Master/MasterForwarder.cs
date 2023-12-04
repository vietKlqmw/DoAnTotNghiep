using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.Master
{
    [Table("MasterForwarder")]
    public class MasterForwarder : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxCodeLength = 10;

        public const int MaxNameLength = 100;

        [StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        public virtual long? SupplierId { get; set; }
    }
}
