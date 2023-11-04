using Abp.Application.Services.Dto;

namespace tmss.Master.CustomsStatus
{
    public class MasterCustomsStatusDto : EntityDto<long?>
    {
        public virtual string Code { get; set; }

        public virtual string Description { get; set; }
    }

    public class GetMasterCustomsStatusInput : PagedAndSortedResultRequestDto
    {
        public virtual string Code { get; set; }
    }
}
