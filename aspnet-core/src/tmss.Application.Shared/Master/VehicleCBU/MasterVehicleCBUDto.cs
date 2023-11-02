using Abp.Application.Services.Dto;

namespace tmss.Master.VehicleCBU
{
    public class MasterVehicleCBUDto : EntityDto<long?>
    {
        public virtual string VehicleType { get; set; }

        public virtual string Model { get; set; }

        public virtual string MarketingCode { get; set; }

        public virtual string ProductionCode { get; set; }
    }

    public class GetMasterVehicleCBUInput : PagedAndSortedResultRequestDto
    {
        public virtual string VehicleType { get; set; }

        public virtual string Model { get; set; }
    }
}
