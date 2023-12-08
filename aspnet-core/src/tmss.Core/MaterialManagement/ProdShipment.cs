using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.MaterialManagement
{
    [Table("ProdShipment")]
    public class ProdShipment : FullAuditedEntity<long>, IEntity<long> // lô hàng
    {
        public const int MaxShipmentNoLength = 10;

        public const int MaxSupplierNoLength = 10;

        public const int MaxBuyerLength = 4;

        public const int MaxFromPortLength = 50;

        public const int MaxToPortLength = 50;

        public const int MaxOceanVesselNameLength = 30;

        public const int MaxStatusLength = 50;

        public const int MaxForwarderLength = 10;


        [StringLength(MaxShipmentNoLength)]
        public virtual string ShipmentNo { get; set; } //số lô

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; } //mã nhà cung cấp

        [StringLength(MaxBuyerLength)]
        public virtual string Buyer { get; set; } //người mua

        [StringLength(MaxFromPortLength)]
        public virtual string FromPort { get; set; } //từ cảng

        [StringLength(MaxToPortLength)]
        public virtual string ToPort { get; set; } //đến cảng

        public virtual DateTime? ShipmentDate { get; set; } //ngày giao hàng

        public virtual DateTime? Etd { get; set; } // time khởi hành dự kiến

        public virtual DateTime? Eta { get; set; } // time đến dự kiến

        public virtual DateTime? Ata { get; set; } //thời gian vận chuyển

        [StringLength(MaxOceanVesselNameLength)]
        public virtual string OceanVesselName { get; set; } //tên tàu biển

        public virtual DateTime? Atd { get; set; } //thời gian thực tế đến nơi

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; } //trạng thái

        [StringLength(MaxForwarderLength)]
        public virtual string Forwarder { get; set; }
    }

}
