using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace tmss.Master.PartList
{
    public class MasterPartListDto : EntityDto<long?>
    {
        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual long? SupplierId { get; set; }

        public virtual long? MaterialId { get; set; }

        public virtual string CarfamilyCode { get; set; }

        public virtual DateTime? StartProductionMonth { get; set; }

        public virtual DateTime? EndProductionMonth { get; set; }

        public virtual string Remark { get; set; }
    }

    public class GetMasterPartListInput : PagedAndSortedResultRequestDto
    {
        public virtual string PartNo { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string CarfamilyCode { get; set; }

    }

    public class GetMasterPartListExportInput
    {
        public virtual string PartNo { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string CarfamilyCode { get; set; }

    }

    public class CreateOrEditMasterPartListDto : EntityDto<long?>
    {
        [StringLength(15)]
        public virtual string PartNo { get; set; }

        [StringLength(500)]
        public virtual string PartName { get; set; }

        [StringLength(10)]
        public virtual string SupplierNo { get; set; }

        public virtual long? MaterialId { get; set; }

        [StringLength(4)]
        public virtual string CarfamilyCode { get; set; }

        public virtual DateTime? StartProductionMonth { get; set; }

        public virtual DateTime? EndProductionMonth { get; set; }

        [StringLength(5000)]
        public virtual string Remark { get; set; }
    }
}
