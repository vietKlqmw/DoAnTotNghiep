using Abp.Application.Services.Dto;

namespace tmss.Master.MaterialGroup
{
    public class MasterMaterialGroupDto : EntityDto<long?>
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }
    }

    public class GetMasterMaterialGroupInput : PagedAndSortedResultRequestDto
    {
        public virtual string Code { get; set; }
    }
}
