using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.Master
{
    [Table("MasterStorageLocation")]
    public class MasterStorageLocation : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxPlantCodeLength = 4;

        public const int MaxPlantNameLength = 30;

        public const int MaxStorageLocationLength = 4;

        public const int MaxStorageLocationNameLength = 200;

        public const int MaxAddressLanguageEnLength = 200;

        public const int MaxAddressLanguageVnLength = 200;

        public const int MaxCategoryLength = 50;

        [StringLength(MaxPlantCodeLength)]
        public virtual string PlantCode { get; set; }

        [StringLength(MaxPlantNameLength)]
        public virtual string PlantName { get; set; }

        [StringLength(MaxStorageLocationLength)]
        public virtual string StorageLocation { get; set; }

        [StringLength(MaxStorageLocationNameLength)]
        public virtual string StorageLocationName { get; set; }

        [StringLength(MaxAddressLanguageEnLength)]
        public virtual string AddressLanguageEn { get; set; }

        [StringLength(MaxAddressLanguageVnLength)]
        public virtual string AddressLanguageVn { get; set; }

        [StringLength(MaxCategoryLength)]
        public virtual string Category { get; set; }
    }
}
