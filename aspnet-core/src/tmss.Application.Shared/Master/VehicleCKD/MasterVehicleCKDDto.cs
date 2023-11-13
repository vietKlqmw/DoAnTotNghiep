using Abp.Application.Services.Dto;

namespace tmss.Master.VehicleCKD
{
    public class MasterVehicleCKDDto : EntityDto<long?>
    {
        public virtual string Model { get; set; }

        public virtual string LotCode { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string Grade { get; set; }

        public virtual string GradeName { get; set; }

        public virtual string ModelCode { get; set; }

        public virtual string VehicleId { get; set; }

        public virtual string CarSeries { get; set; }

        public virtual string TransmissionType { get; set; }

        public virtual string EngineType { get; set; }

        public virtual string FuelType { get; set; }
    }

    public class GetVehicleCKDInput : PagedAndSortedResultRequestDto
    {
        public virtual string Model { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string ModelCode { get; set; }
    }

    public class GetVehicleCKDExportInput
    {
        public virtual string Model { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string ModelCode { get; set; }
    }
}
