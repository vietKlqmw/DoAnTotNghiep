using Abp.Application.Services.Dto;
using System;

namespace tmss.MaterialManagement.Invoice
{
    public class ProdInvoiceDto : EntityDto<long?>
    {
        //invoice
        public virtual string InvoiceNo { get; set; } //mã hóa đơn
        public virtual long? BillId { get; set; }
        public virtual DateTime? InvoiceDate { get; set; }
        public virtual string Forwarder { get; set; }
        public virtual string Status { get; set; }

        public virtual string ShipmentNo { get; set; }
        public virtual string BillNo { get; set; }
        public virtual DateTime? BillDate { get; set; }


        //invoice details
        public virtual string PartNo { get; set; }
        public virtual decimal? Insurance { get; set; } //Bảo hiểm
        public virtual string ContainerNo { get; set; }
        public virtual long? InvoiceId { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual decimal? Freight { get; set; } // Cước vẫn chuyển
        public virtual decimal? Thc { get; set; } //phí xếp dỡ bến
        public virtual decimal? Cif { get; set; }  //Cost + Insurance + Freight
        public virtual decimal? Tax { get; set; } //thuế
        public virtual decimal? TaxRate { get; set; } //% thuế
        public virtual decimal? Vat { get; set; } //giá trị gia tăng
        public virtual decimal? VatRate { get; set; } // % GTGT
        public virtual int? UsageQty { get; set; }
        public virtual string PartName { get; set; }
        public virtual string CarfamilyCode { get; set; }
        public virtual decimal? GrossWeight { get; set; } //khối lượng toàn phần
        public virtual string Currency { get; set; } //Tiền tệ

        public virtual double? GrandQty { get; set; }
        public virtual decimal? GrandCif { get; set; }
        public virtual decimal? GrandFreight { get; set; }
        public virtual decimal? GrandInsurance { get; set; }
        public virtual decimal? GrandTax { get; set; }
        public virtual decimal? GrandVat { get; set; }
        public virtual decimal? GrandThc { get; set; }

        //for Export
        public virtual string BaseUnitOfMeasure { get; set; }
        public virtual decimal? StandardPrice { get; set; }

        //other
        public virtual string KeyRow
        {
            get { return string.Format("{0}_{1}", PartNo, PartName); }
            set { }
        }
        public virtual int? ActualQty
        {
            get { return UsageQty; }
            set { }
        }
    }

    public class GetProdInvoiceInput : PagedAndSortedResultRequestDto
    {
        public virtual string InvoiceNo { get; set; }
        public virtual DateTime? InvoiceDateFrom { get; set; }
        public virtual DateTime? InvoiceDateTo { get; set; }
        public virtual string BillNo { get; set; }
        public virtual string ShipmentNo { get; set; }
        public virtual string ContainerNo { get; set; }
        public virtual DateTime? BillDateFrom { get; set; }
        public virtual DateTime? BillDateTo { get; set; }
        public virtual string SupplierNo { get; set; }

    }

    public class GetProdInvoiceExportInput
    {
        public virtual string InvoiceNo { get; set; }
        public virtual DateTime? InvoiceDateFrom { get; set; }
        public virtual DateTime? InvoiceDateTo { get; set; }
        public virtual string BillNo { get; set; }
        public virtual string ShipmentNo { get; set; }
        public virtual string ContainerNo { get; set; }
        public virtual DateTime? BillDateFrom { get; set; }
        public virtual DateTime? BillDateTo { get; set; }
        public virtual string SupplierNo { get; set; }

    }

    public class GetProdInvoiceDetailsInput : PagedAndSortedResultRequestDto
    {
        public virtual long? InvoiceId { get; set; }

    }

    public class GetProdInvoiceDetailsExportInput
    {
        public virtual long? InvoiceId { get; set; }

    }

    public class ProdInvoiceDetailsDto : EntityDto<long?>
    {
        public virtual string PartNo { get; set; }
        public virtual decimal? Insurance { get; set; } //Bảo hiểm
        public virtual string ContainerNo { get; set; }
        public virtual decimal? Freight { get; set; } // Cước vẫn chuyển
        public virtual decimal? Thc { get; set; } //phí xếp dỡ bến
        public virtual decimal? Tax { get; set; } //thuế
        public virtual decimal? Vat { get; set; } //giá trị gia tăng
        public virtual string CarfamilyCode { get; set; }
        public virtual string KeyRow
        {
            get { return string.Format("{0}_{1}_{2}",ContainerNo, PartNo, CarfamilyCode); }
            set { }
        }
    }
}
