using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.Master
{
    [Table("MasterEngine_T")]
    public class MasterEngine_T : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxMaterialCodeLength = 40;

        public const int MaxClassTypeLength = 3;

        public const int MaxClassNameLength = 18;

        public const int MaxTransmissionTypeLength = 5;

        public const int MaxEngineModelLength = 10;

        public const int MaxEngineTypeLength = 10;

        [StringLength(128)]
        public virtual string Guid { get; set; }

        [StringLength(MaxMaterialCodeLength)]
        public virtual string MaterialCode { get; set; }

        [StringLength(MaxClassTypeLength)]
        public virtual string ClassType { get; set; }

        [StringLength(MaxClassNameLength)]
        public virtual string ClassName { get; set; }

        [StringLength(MaxTransmissionTypeLength)]
        public virtual string TransmissionType { get; set; }

        [StringLength(MaxEngineModelLength)]
        public virtual string EngineModel { get; set; }

        [StringLength(MaxEngineTypeLength)]
        public virtual string EngineType { get; set; }

        [StringLength(5000)]
        public string ErrorDescription { get; set; }
    }
}
