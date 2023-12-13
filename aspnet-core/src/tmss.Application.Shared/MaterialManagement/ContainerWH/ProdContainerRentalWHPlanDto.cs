using Abp.Application.Services.Dto;
using System;

namespace tmss.MaterialManagement.ContainerWH
{
    public class ProdContainerRentalWHPlanDto : EntityDto<long?>
    {
        public virtual string ContainerNo { get; set; }
        public virtual DateTime? RequestDate { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual string BillofladingNo { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual DateTime? ReceiveDate { get; set; }
        public virtual DateTime? DeliveryDate { get; set; }
        public virtual string Transport { get; set; }
        public virtual string Status { get; set; }
        public virtual string Warehouse { get; set; }
        public virtual long? InvoiceId { get; set; }
        public virtual long? BillId { get; set; }
        public virtual string GoodsReceivedNoteNo { get; set; }
        public virtual DateTime? DevanningDate { get; set; }
    }

    public class GetProdContainerRentalWHPlanInput : PagedAndSortedResultRequestDto
    {
        public virtual string ContainerNo { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual string BillofladingNo { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual DateTime? ReceiveDateFrom { get; set; }
        public virtual DateTime? ReceiveDateTo { get; set; }
        public virtual string Warehouse { get; set; }
    }

    public class GetProdContainerRentalWHPlanExportInput
    {
        public virtual string ContainerNo { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual string BillofladingNo { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual DateTime? ReceiveDateFrom { get; set; }
        public virtual DateTime? ReceiveDateTo { get; set; }
        public virtual string Warehouse { get; set; }
    }

    public class ProdContainerRentalWHPlanImportDto
    {
        public virtual string Guid { get; set; }
        public virtual string ContainerNo { get; set; }
        public virtual DateTime? RequestDate { get; set; }
        public virtual TimeSpan? RequestTime { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual string BillofladingNo { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string SealNo { get; set; }
        public virtual DateTime? DevanningDate { get; set; }
        public virtual string DevanningTime { get; set; }
        public virtual DateTime? ActualDevanningDate { get; set; }
        public virtual DateTime? GateInPlanTime { get; set; }
        public virtual DateTime? GateInActualDateTime { get; set; }
        public virtual string Transport { get; set; }
        public virtual string Status { get; set; }
        public virtual string ErrorDescription { get; set; }
        public virtual long? CreatorUserId { get; set; }
    }
}
