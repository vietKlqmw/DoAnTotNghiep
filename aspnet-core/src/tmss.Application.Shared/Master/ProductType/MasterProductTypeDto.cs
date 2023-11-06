using Abp.Application.Services.Dto;

namespace tmss.Master.ProductType
{
    public class MasterProductTypeDto : EntityDto<long?>
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

    }

    public class GetMasterProductTypeInput : PagedAndSortedResultRequestDto
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }
    }
}
