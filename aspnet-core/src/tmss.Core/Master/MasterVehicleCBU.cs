using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.Master.VehicleCBU
{
    [Table("MasterVehicleCBU")]
    public class MasterVehicleCBU : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxVehicleTypeLength = 10;

        public const int MaxModelLength = 5000;

        public const int MaxMarketingCodeLength = 50;

        public const int MaxProductionCodeLength = 50;

        [StringLength(MaxVehicleTypeLength)]
        public virtual string VehicleType { get; set; }

        [StringLength(MaxModelLength)]
        public virtual string Model { get; set; }

        [StringLength(MaxMarketingCodeLength)]
        public virtual string MarketingCode { get; set; }

        [StringLength(MaxProductionCodeLength)]
        public virtual string ProductionCode { get; set; }
    }

}
