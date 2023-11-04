using Abp.Application.Services.Dto;

namespace tmss.Master.ContainerStatus
{
    public class MasterContainerStatusDto : EntityDto<long?>
    {
        public virtual string Code { get; set; }

        public virtual string Description { get; set; }

        public virtual string DescriptionVn { get; set; }
    }

    public class GetMasterContainerStatusInput : PagedAndSortedResultRequestDto
    {
        public virtual string Code { get; set; }

        public virtual string Description { get; set; }
    }
}
