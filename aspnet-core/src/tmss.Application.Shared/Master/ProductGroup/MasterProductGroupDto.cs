using Abp.Application.Services.Dto;

namespace tmss.Master.ProductGroup
{
    public class MasterProductGroupDto : EntityDto<long?>
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

    }

    public class GetMasterProductGroupInput : PagedAndSortedResultRequestDto
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }
    }
}
