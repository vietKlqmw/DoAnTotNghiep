using Abp.Application.Services.Dto;

namespace tmss.Master.CarSeries
{
    public class MasterCarSeriesDto : EntityDto<long?>
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

    }

    public class GetMasterCarSeriesInput : PagedAndSortedResultRequestDto
    {
        public virtual string Code { get; set; }

    }
}
