using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.Master
{
    [Table("MasterMaterial_T")]
    public class MasterMaterial_T : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxMaterialTypeLength = 4;

        public const int MaxMaterialCodeLength = 40;

        public const int MaxDescriptionLength = 40;

        public const int MaxMaterialGroupLength = 9;

        public const int MaxBaseUnitOfMeasureLength = 3;

        public const int MaxStorageLocationLength = 4;

        public const int MaxMaterialOriginLength = 50;

        public const int MaxProductionTypeLength = 10;

        [StringLength(128)]
        public virtual string Guid { get; set; }

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

        [StringLength(MaxStorageLocationLength)]
        public virtual string StorageLocation { get; set; }//Địa điểm lưu trữ

        public virtual decimal? StandardPrice { get; set; }//Giá chuẩn

        public virtual decimal? MovingPrice { get; set; }//Giá vận chuyển

        [StringLength(MaxMaterialOriginLength)]
        public virtual string MaterialOrigin { get; set; }//Nguồn gốc nguyên liệu

        public virtual DateTime? EffectiveDateFrom { get; set; }

        public virtual DateTime? EffectiveDateTo { get; set; }

        [StringLength(MaxProductionTypeLength)]
        public virtual string ProductionType { get; set; }

        [StringLength(5000)]
        public string ErrorDescription { get; set; }
    }
}
