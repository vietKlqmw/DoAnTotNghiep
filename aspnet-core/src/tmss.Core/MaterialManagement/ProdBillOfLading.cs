using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.MaterialManagement
{
    [Table("ProdBillOfLading")]
    public class ProdBillOfLading : FullAuditedEntity<long>, IEntity<long> // vận đơn -> gg search
    {
        public const int MaxBillofladingNoLength = 20;

        public const int MaxStatusCodeLength = 50;

        [StringLength(MaxBillofladingNoLength)]
        public virtual string BillofladingNo { get; set; }

        public virtual long? ShipmentId { get; set; }

        [Column(TypeName = "date")]
        public virtual DateTime? BillDate { get; set; }

        [StringLength(MaxStatusCodeLength)]
        public virtual string StatusCode { get; set; }
    }
}
