using Abp.Application.Services.Dto;

namespace tmss.Master.EngineType
{
    public class MasterEngineTypeDto : EntityDto<long?>
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

    }

    public class GetMasterEngineTypeInput : PagedAndSortedResultRequestDto
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }
    }
}
