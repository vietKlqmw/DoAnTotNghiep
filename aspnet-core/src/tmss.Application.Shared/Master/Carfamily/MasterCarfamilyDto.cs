using Abp.Application.Services.Dto;

namespace tmss.Master.Carfamily
{
    public class MasterCarfamilyDto : EntityDto<long?>
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

    }

    public class GetMasterCarfamilyInput : PagedAndSortedResultRequestDto
    {
        public virtual string Code { get; set; }

    }
}
