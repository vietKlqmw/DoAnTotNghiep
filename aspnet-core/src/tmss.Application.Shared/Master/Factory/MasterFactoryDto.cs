using Abp.Application.Services.Dto;

namespace tmss.Master.Factory
{
    public class MasterFactoryDto : EntityDto<long?>
    {
        public virtual string PlantCode { get; set; }

        public virtual string PlantName { get; set; }

        public virtual string BranchNo { get; set; }

        public virtual string AddressLanguageEn { get; set; }

        public virtual string AddressLanguageVn { get; set; }
    }

    public class GetMasterFactoryInput : PagedAndSortedResultRequestDto
    {
        public virtual string PlantName { get; set; }

        public virtual string BranchNo { get; set; }

        public virtual string AddressLanguageEn { get; set; }
    }
}
