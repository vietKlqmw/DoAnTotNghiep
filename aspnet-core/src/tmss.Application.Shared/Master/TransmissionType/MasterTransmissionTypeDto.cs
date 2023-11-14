using Abp.Application.Services.Dto;

namespace tmss.Master.TransmissionType
{
    public class MasterTransmissionTypeDto : EntityDto<long?>
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

    }

    public class GetMasterTransmissionTypeInput : PagedAndSortedResultRequestDto
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }
    }
}
