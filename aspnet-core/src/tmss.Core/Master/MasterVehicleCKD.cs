using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.Master
{
    [Table("MasterVehicleCKD")]
    public class MasterVehicleCKD : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxModelLength = 1;

        public const int MaxLotCodeLength = 2;

        public const int MaxCfcLength = 4;

        public const int MaxGradeLength = 2;

        public const int MaxGradeNameLength = 50;

        public const int MaxModelCodeLength = 50;

        public const int MaxVehicleIdLength = 2;

        public const int MaxCarSeriesLength = 18;

        public const int MaxTransmissionTypeLength = 2;

        public const int MaxEngineTypeLength = 10;

        public const int MaxFuelTypeLength = 2;

        [StringLength(MaxModelLength)]
        public virtual string Model { get; set; }

        [StringLength(MaxLotCodeLength)]
        public virtual string LotCode { get; set; }

        [StringLength(MaxCfcLength)]
        public virtual string Cfc { get; set; }

        [StringLength(MaxGradeLength)]
        public virtual string Grade { get; set; }

        [StringLength(MaxGradeNameLength)]
        public virtual string GradeName { get; set; }

        [StringLength(MaxModelCodeLength)]
        public virtual string ModelCode { get; set; }

        [StringLength(MaxVehicleIdLength)]
        public virtual string VehicleId { get; set; }

        [StringLength(MaxCarSeriesLength)]
        public virtual string CarSeries { get; set; }

        [StringLength(MaxTransmissionTypeLength)]
        public virtual string TransmissionType { get; set; }

        [StringLength(MaxEngineTypeLength)]
        public virtual string EngineType { get; set; }

        [StringLength(MaxFuelTypeLength)]
        public virtual string FuelType { get; set; }
    }
}
