import { Component, Injector, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { ProdCustomsDeclareDto, ProdCustomsDeclareServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from "ngx-bootstrap/modal";
import { finalize } from "rxjs/operators";
import { BsDatepickerDirective } from "ngx-bootstrap/datepicker";
import * as moment from 'moment';
import { CustomsDeclareComponent } from "./customs-declare.component";
import { DataFormatService } from "@app/shared/common/services/data-format.service";
import { formatDate } from "@angular/common";

@Component({
    selector: 'editModal',
    templateUrl: './edit-customs-declare-modal.component.html'
})
export class EditCustomsDeclareModalComponent extends AppComponentBase {
    @ViewChild('editModal', { static: true }) modal: ModalDirective;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    rowData: ProdCustomsDeclareDto = new ProdCustomsDeclareDto();
    saving = false;
    isEdit: boolean = false;
    header: string = '';
    _billDate: any;
    _billDateSub: any;
    _declareDate: any;
    _invoiceDate: any;
    _invoiceDateSub: any;
    _status;
    _tax;
    _vat;
    list = [
        { value: 'CuS1', label: "NEW" },
        { value: 'CuS2', label: "NOT PAID (REQUESTED)" },
        { value: 'CuS3', label: "PAID" }
    ];

    constructor(
        private injector: Injector,
        private _service: ProdCustomsDeclareServiceProxy,
        private _component: CustomsDeclareComponent,
        private _fm: DataFormatService
    ) {
        super(injector);
    }

    ngOnInit() { }

    show(type, material?: ProdCustomsDeclareDto): void {
        if (type == 'Edit') this.isEdit = true;
        else this.isEdit = false;
        this.header = type;
        if (material) this.rowData = material;
        else this.rowData = new ProdCustomsDeclareDto();

        const dateValue = this.rowData.declareDate ? new Date(this.rowData.declareDate?.toString()) : new Date();
        this.datepicker?.bsValueChange.emit(dateValue);

        this._billDate = this.rowData.billDate ? formatDate(new Date(this.rowData.billDate?.toString()), 'dd/MM/yyyy', 'en-US') : undefined;
        this._billDateSub = this.rowData.billDate ? formatDate(new Date(this.rowData.billDate?.toString()), 'MM/dd/yyyy', 'en-US') : undefined;
        this._invoiceDate = this.rowData.invoiceDate ? formatDate(new Date(this.rowData.invoiceDate?.toString()), 'dd/MM/yyyy', 'en-US') : undefined;
        this._invoiceDateSub = this.rowData.invoiceDate ? formatDate(new Date(this.rowData.invoiceDate?.toString()), 'MM/dd/yyyy', 'en-US') : undefined;

        this._status = (this.rowData.status == null || this.rowData.status == '') ? 'CuS1' : this.rowData.status;

        if (this.rowData.tax == undefined) this._tax = 0;
        else this._tax = this._fm.formatMoney(this.rowData.tax);
        if (this.rowData.vat == undefined) this._vat = 0;
        else this._vat = this._fm.formatMoney(this.rowData.vat);

        this.modal.show();
    }

    save(): void {
        this.rowData.declareDate = this._declareDate ? moment(this._declareDate) : undefined;
        this.rowData.status = this._status;
        this.rowData.tax = this._tax != null ? (this._tax.length > 3 ? Number(this._tax.replace(/,/g,'')) : Number(this._tax)) : 0;
        this.rowData.vat = this._vat != null ? (this._vat.length > 3 ? Number(this._vat.replace(/,/g,'')) : Number(this._vat)) : 0;
        this.rowData.billDate = moment(this._billDateSub);
        this.rowData.invoiceDate = moment(this._invoiceDateSub);

        if (this.rowData.customsDeclareNo == null) {
            this.notify.warn('CustomsDeclareNo is Required!');
            return;
        }
        if (this.rowData.invoiceNo == null) {
            this.notify.warn('InvoiceNo is Required!');
            return;
        }
        if (this.rowData.declareDate == undefined) {
            this.notify.warn('DeclareDate is Required!');
            return;
        }
        if (this.rowData.tax == 0) {
            this.notify.warn('TAX is Required!');
            return;
        }
        if (this.rowData.vat == 0) {
            this.notify.warn('VAT is Required!');
            return;
        }

        this.saving = true;
        this._service.editCustomsDeclare(this.rowData)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this._component.searchDatas();
                this.close();
            });
    }

    changeTax(event) {
        this._tax = this._fm.formatMoney(event);
    }
    changeVat(event) {
        this._vat = this._fm.formatMoney(event);
    }

    // changeSupplier(e){
    //     this._other.getListShipmentById(e).subscribe(result => {
    //         if(result.length > 0){
    //             this.rowData.supplierNo = result[0].supplierNo;
    //             this._shippingDate = result[0].shipmentDate ? formatDate(new Date(result[0].shipmentDate?.toString()), 'dd/MM/yyyy', 'en-US') : undefined;
    //             this._shippingDateSub = result[0].shipmentDate ? formatDate(new Date(result[0].shipmentDate?.toString()), 'MM/dd/yyyy', 'en-US') : undefined;
    //         }
    //     })
    // }

    close(): void {
        this.modal.hide();
    }
}
