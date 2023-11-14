using Abp.Application.Services.Dto;

namespace tmss.Master.EngineModel
{
    public class MasterEngineModelDto : EntityDto<long?>
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

    }

    public class GetMasterEngineModelInput : PagedAndSortedResultRequestDto
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }
    }
}
