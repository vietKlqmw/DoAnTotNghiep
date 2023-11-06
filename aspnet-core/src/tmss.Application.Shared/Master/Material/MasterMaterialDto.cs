using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace tmss.Master.Material
{
    public class MasterMaterialDto : EntityDto<long?>
    {
        public virtual string MaterialType { get; set; }//Loại vật liệu
        public virtual string MaterialCode { get; set; }//Mã vật liệu
        public virtual string Description { get; set; }//Sự mô tả
        public virtual string MaterialGroup { get; set; }//Nhóm vật liệu
        public virtual string BaseUnitOfMeasure { get; set; }//Đơn Vị Đo Cơ Bản
        public virtual string Plant { get; set; }//~Factory Code
        public virtual string StorageLocation { get; set; }//Địa điểm lưu trữ
        public virtual string ProductionGroup { get; set; }//Nhóm sản xuất
        public virtual string ProductionPurpose { get; set; }//Mục đích sản xuất
        public virtual string ReservedStock { get; set; }
        public virtual string LotCode { get; set; }//Số lô
        public virtual string ProductionStorageLocation { get; set; }//Vị trí lưu trữ sản xuất
        public virtual decimal? CostingLotSize { get; set; }//Kích thước lô chi phí
        public virtual string ProductionVersion { get; set; }//Phiên bản sản xuất
        public virtual decimal? StandardPrice { get; set; }//Giá chuẩn
        public virtual decimal? MovingPrice { get; set; }//Giá vận chuyển
        public virtual string MaterialOrigin { get; set; }//Nguồn gốc nguyên liệu
        public virtual string OriginGroup { get; set; }//Nhóm xuất xứ
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
}
