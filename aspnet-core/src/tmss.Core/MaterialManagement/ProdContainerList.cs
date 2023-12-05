using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.MaterialManagement
{
    [Table("ProdContainerList")]
    public class ProdContainerList : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxContainerNoLength = 15;

        public const int MaxSupplierNoLength = 10;

        public const int MaxBillOfLadingNoLength = 20;

        public const int MaxSealNoLength = 20;

        public const int MaxInvoiceNoLength = 20;

        public const int MaxTransportLength = 10;

        public const int MaxRemarkLength = 1000;

        public const int MaxWhLocationLength = 10;

        public const int MaxStatusLength = 20;

        public const int MaxLocationCodeLength = 20;

        public const int MaxDevanningTimeLength = 255;

        public const int MaxTransitPortReqTimeLength = 255;

        public const int MaxGateInTimeLength = 255;

        public const int MaxRequestStatusLength = 20;

        public const int MaxReceiveTimeLength = 50;


        [StringLength(MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxBillOfLadingNoLength)]
        public virtual string BillOfLadingNo { get; set; }

        [StringLength(MaxSealNoLength)]
        public virtual string SealNo { get; set; }

        public virtual int? ContainerSize { get; set; }

        public virtual long? ShipmentId { get; set; }

        public virtual DateTime? ShippingDate { get; set; }

        public virtual DateTime? PortDate { get; set; }

        public virtual DateTime? PortDateActual { get; set; }

        public virtual DateTime? PortTransitDate { get; set; }

        public virtual DateTime? ReceiveDate { get; set; }

        public virtual long? RequestId { get; set; }

        [StringLength(MaxInvoiceNoLength)]
        public virtual string InvoiceNo { get; set; }

        [StringLength(MaxTransportLength)]
        public virtual string Transport { get; set; }

        public virtual DateTime? DevanningDate { get; set; }

        [StringLength(MaxDevanningTimeLength)]
        public virtual string DevanningTime { get; set; }

        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        [StringLength(MaxWhLocationLength)]
        public virtual string WhLocation { get; set; }

        public virtual DateTime? GateInDate { get; set; }

        [StringLength(MaxGateInTimeLength)]
        public virtual string GateInTime { get; set; }

        public virtual long? TransitPortReqId { get; set; }

        public virtual DateTime? TransitPortReqDate { get; set; }

        [StringLength(MaxTransitPortReqTimeLength)]
        public virtual string TransitPortReqTime { get; set; }

        public virtual decimal? Freight { get; set; }

        public virtual decimal? Insurance { get; set; }

        public virtual decimal? Cif { get; set; }

        public virtual decimal? Tax { get; set; }

        public virtual decimal? Amount { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(MaxLocationCodeLength)]
        public virtual string LocationCode { get; set; }

        public virtual DateTime? LocationDate { get; set; }

        public virtual long? ReceivingPeriodId { get; set; }

        public virtual long? RentalWhId { get; set; }

        [StringLength(MaxRequestStatusLength)]
        public virtual string RequestStatus { get; set; }

        public virtual DateTime? BillDate { get; set; }

        [StringLength(MaxReceiveTimeLength)]
        public virtual string ReceiveTime { get; set; }
    }
}
