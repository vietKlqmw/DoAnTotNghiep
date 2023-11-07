using Abp.Application.Services.Dto;

namespace tmss.Master.UoM
{
    public class MasterUnitOfMeasureDto : EntityDto<long?>
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }
    }

    public class GetMasterUnitOfMeasureInput : PagedAndSortedResultRequestDto
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }
    }

    public class GetMasterUnitOfMeasureExportInput
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }
    }
}
