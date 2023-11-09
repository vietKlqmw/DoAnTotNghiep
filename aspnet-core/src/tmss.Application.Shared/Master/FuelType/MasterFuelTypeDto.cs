using Abp.Application.Services.Dto;

namespace tmss.Master.FuelType
{
    public class MasterFuelTypeDto : EntityDto<long?>
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }
    }

    public class GetMasterFuelTypeInput : PagedAndSortedResultRequestDto
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }
    }
}
