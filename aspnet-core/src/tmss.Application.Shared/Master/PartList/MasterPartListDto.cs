using Abp.Application.Services.Dto;
using System;

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

        public virtual string BaseUnitOfMeasure { get; set; }
        public virtual decimal? StandardPrice { get; set; }
        public virtual decimal? MovingPrice { get; set; }
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

    public class MasterPartListImportDto
    {
        public virtual string Guid { get; set; }
        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string CarfamilyCode { get; set; }

        public virtual DateTime? StartProductionMonth { get; set; }

        public virtual DateTime? EndProductionMonth { get; set; }

        public virtual string Remark { get; set; }

        public virtual string ErrorDescription { get; set; }

        public virtual long? CreatorUserId { get; set; }
    }
}
