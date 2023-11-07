using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.Master.Material
{
    [Table("MasterMaterial")]
    public class MasterMaterial : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxMaterialTypeLength = 4;

        public const int MaxMaterialCodeLength = 40;

        public const int MaxDescriptionLength = 40;

        public const int MaxMaterialGroupLength = 9;

        public const int MaxBaseUnitOfMeasureLength = 3;

        public const int MaxPlantLength = 4;

        public const int MaxStorageLocationLength = 4;

        public const int MaxProductionGroupLength = 3;

        public const int MaxProductionPurposeLength = 2;

        public const int MaxProductionTypeLength = 10;

        public const int MaxReservedStockLength = 2;

        public const int MaxLotCodeLength = 10;

        public const int MaxProductionStorageLocationLength = 4;

        public const int MaxProductionVersionLength = 4;

        public const int MaxMaterialOriginLength = 1;

        public const int MaxOriginGroupLength = 4;

        [StringLength(MaxMaterialTypeLength)]
        public virtual string MaterialType { get; set; }//Loại vật liệu

        [StringLength(MaxMaterialCodeLength)]
        public virtual string MaterialCode { get; set; }//Mã vật liệu

        [StringLength(MaxDescriptionLength)]
        public virtual string Description { get; set; }//Sự mô tả

        [StringLength(MaxMaterialGroupLength)]
        public virtual string MaterialGroup { get; set; }//Nhóm vật liệu

        [StringLength(MaxBaseUnitOfMeasureLength)]
        public virtual string BaseUnitOfMeasure { get; set; }//Đơn Vị Đo Cơ Bản

        [StringLength(MaxPlantLength)]
        public virtual string Plant { get; set; }//~Factory Code

        [StringLength(MaxStorageLocationLength)]
        public virtual string StorageLocation { get; set; }//Địa điểm lưu trữ

        [StringLength(MaxProductionGroupLength)]
        public virtual string ProductionGroup { get; set; }//Nhóm sản xuất

        [StringLength(MaxProductionPurposeLength)]
        public virtual string ProductionPurpose { get; set; }//Mục đích sản xuất

        [StringLength(MaxReservedStockLength)]
        public virtual string ReservedStock { get; set; }

        [StringLength(MaxLotCodeLength)]
        public virtual string LotCode { get; set; }//Số lô

        [StringLength(MaxProductionStorageLocationLength)]
        public virtual string ProductionStorageLocation { get; set; }//Vị trí lưu trữ sản xuất

        public virtual decimal? CostingLotSize { get; set; }//Kích thước lô chi phí

        [StringLength(MaxProductionVersionLength)]
        public virtual string ProductionVersion { get; set; }//Phiên bản sản xuất

        public virtual decimal? StandardPrice { get; set; }//Giá chuẩn

        public virtual decimal? MovingPrice { get; set; }//Giá vận chuyển

        [StringLength(MaxMaterialOriginLength)]
        public virtual string MaterialOrigin { get; set; }//Nguồn gốc nguyên liệu

        [StringLength(MaxOriginGroupLength)]
        public virtual string OriginGroup { get; set; }//Nhóm xuất xứ

        public virtual DateTime? EffectiveDateFrom { get; set; }

        public virtual DateTime? EffectiveDateTo { get; set; }

        [StringLength(MaxProductionTypeLength)]
        public virtual string ProductionType { get; set; }
    }

}

