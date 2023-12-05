using Abp.Application.Services.Dto;
using System;

namespace tmss.Master.Material
{
    public class MasterMaterialDto : EntityDto<long?>
    {
        public virtual string MaterialType { get; set; }//Loại vật liệu
        public virtual string MaterialCode { get; set; }//Mã vật liệu
        public virtual string Description { get; set; }//Sự mô tả
        public virtual string MaterialGroup { get; set; }//Nhóm vật liệu
        public virtual string BaseUnitOfMeasure { get; set; }//Đơn Vị Đo Cơ Bản
        public virtual string StorageLocation { get; set; }//Địa điểm lưu trữ
        public virtual string ProductionType { get; set; }
        public virtual decimal? StandardPrice { get; set; }//Giá chuẩn
        public virtual decimal? MovingPrice { get; set; }//Giá vận chuyển
        public virtual string MaterialOrigin { get; set; }//Nguồn gốc nguyên liệu
        public virtual DateTime? EffectiveDateFrom { get; set; }
        public virtual DateTime? EffectiveDateTo { get; set; }
    }

    public class GetMasterMaterialInput : PagedAndSortedResultRequestDto
    {
        public virtual string MaterialCode { get; set; }
        public virtual string MaterialGroup { get; set; }
    }

    public class GetMasterMaterialExportInput
    {
        public virtual string MaterialCode { get; set; }
        public virtual string MaterialGroup { get; set; }
    }

    public class MasterMaterialImportDto
    {
        public virtual string Guid { get; set; }
        public virtual string MaterialType { get; set; }//Loại vật liệu
        public virtual string MaterialCode { get; set; }//Mã vật liệu
        public virtual string Description { get; set; }//Sự mô tả
        public virtual string MaterialGroup { get; set; }//Nhóm vật liệu
        public virtual string BaseUnitOfMeasure { get; set; }//Đơn Vị Đo Cơ Bản
        public virtual string StorageLocation { get; set; }//Địa điểm lưu trữ
        public virtual string ProductionType { get; set; }
        public virtual decimal? StandardPrice { get; set; }//Giá chuẩn
        public virtual decimal? MovingPrice { get; set; }//Giá vận chuyển
        public virtual string MaterialOrigin { get; set; }//Nguồn gốc nguyên liệu
        public virtual DateTime? EffectiveDateFrom { get; set; }
        public virtual DateTime? EffectiveDateTo { get; set; }
        public virtual string ErrorDescription { get; set; }
        public virtual long? CreatorUserId { get; set; }
    }
}
