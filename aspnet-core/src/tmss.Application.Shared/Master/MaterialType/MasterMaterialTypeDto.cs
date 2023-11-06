using Abp.Application.Services.Dto;

namespace tmss.Master.MaterialType
{
    public class MasterMaterialTypeDto : EntityDto<long?>
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

    }

    public class GetMasterMaterialTypeInput : PagedAndSortedResultRequestDto
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }
    }
}
