using Abp.Application.Services.Dto;

namespace tmss.Master.StorageLocation
{
    public class MasterStorageLocationDto : EntityDto<long?>
    {
        public virtual string PlantCode { get; set; }

        public virtual string PlantName { get; set; }

        public virtual string StorageLocation { get; set; }

        public virtual string StorageLocationName { get; set; }

        public virtual string AddressLanguageEn { get; set; }

        public virtual string AddressLanguageVn { get; set; }

        public virtual string Category { get; set; }
    }

    public class GetMasterStorageLocationInput : PagedAndSortedResultRequestDto
    {
        public virtual string PlantName { get; set; }

        public virtual string StorageLocationName { get; set; }

        public virtual string AddressLanguageEn { get; set; }

        public virtual string Category { get; set; }
    }

    public class GetMasterStorageLocationExportInput
    {
        public virtual string PlantName { get; set; }

        public virtual string StorageLocationName { get; set; }

        public virtual string AddressLanguageEn { get; set; }

        public virtual string Category { get; set; }
    }
}
