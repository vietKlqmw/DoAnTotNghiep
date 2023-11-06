using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.Master
{
    [Table("MasterFactory")]
    public class MasterFactory : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxPlantCodeLength = 4;

        public const int MaxPlantNameLength = 30;

        public const int MaxBranchNoLength = 4;

        public const int MaxAddressLanguageEnLength = 200;

        public const int MaxAddressLanguageVnLength = 200;

        [StringLength(MaxPlantCodeLength)]
        public virtual string PlantCode { get; set; }

        [StringLength(MaxPlantNameLength)]
        public virtual string PlantName { get; set; }

        [StringLength(MaxBranchNoLength)]
        public virtual string BranchNo { get; set; }

        [StringLength(MaxAddressLanguageEnLength)]
        public virtual string AddressLanguageEn { get; set; }

        [StringLength(MaxAddressLanguageVnLength)]
        public virtual string AddressLanguageVn { get; set; }
    }
}
