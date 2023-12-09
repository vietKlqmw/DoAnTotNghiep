import { Component, Injector, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { ProdInvoiceDto, ProdInvoiceServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from "ngx-bootstrap/modal";
import { finalize } from "rxjs/operators";
import { BsDatepickerDirective } from "ngx-bootstrap/datepicker";
import * as moment from 'moment';
import { InvoiceComponent } from "./invoice.component";
import { formatDate } from "@angular/common";
import { DataFormatService } from "@app/shared/common/services/data-format.service";

@Component({
    selector: 'editModal',
    templateUrl: './edit-invoice-modal.component.html'
})
export class EditInvoiceModalComponent extends AppComponentBase {
    @ViewChild('editModal', { static: true }) modal: ModalDirective;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    rowData: ProdInvoiceDto = new ProdInvoiceDto();
    saving = false;
    isEdit: boolean = false;
    header: string = '';
    _billDate: any;
    _invoiceDate: any;
    _expandInfo: boolean = true;
    _expandDetailsInfo: boolean = true;
    _freight;
    _insurance;
    _cif;
    _thc;
    _tax;
    _vat;
    list = [
        { value: 'NEW', label: "NEW" },
        { value: 'PRE CUSTOMS', label: "PRE CUSTOMS (PAID)" }
    ];

    constructor(
        private injector: Injector,
        private _service: ProdInvoiceServiceProxy,
        private _component: InvoiceComponent,
        private _fm: DataFormatService
    ) {
        super(injector);
    }

    ngOnInit() { }

    show(type, material?: ProdInvoiceDto): void {
        if (type == 'Edit') this.isEdit = true;
        else this.isEdit = false;
        this.header = type;
        if (material) this.rowData = material;
        else this.rowData = new ProdInvoiceDto();

        if(type == 'Edit'){
            const dateValue = this.rowData.invoiceDate ? new Date(this.rowData.invoiceDate?.toString()) : undefined;
            this.datepicker?.bsValueChange.emit(dateValue);
        }
        else{
            this._invoiceDate = this.rowData.invoiceDate ? formatDate(new Date(this.rowData.invoiceDate?.toString()), 'dd/MM/yyyy', 'en-US') : undefined;
        }

        this._billDate = this.rowData.billDate ? formatDate(new Date(this.rowData.billDate?.toString()), 'dd/MM/yyyy', 'en-US') : undefined;
        if (this.rowData.freight == undefined){
            this.rowData.freight = 0;
            this._freight = 0;
        }
        else this._freight = this._fm.formatMoney(this.rowData.freight);
        if (this.rowData.insurance == undefined){
            this.rowData.insurance = 0;
            this._insurance = 0;
        }
        else this._insurance = this._fm.formatMoney(this.rowData.insurance);
        if (this.rowData.cif == undefined){
            this.rowData.cif = 0;
            this._cif = 0;
        }
        else this._cif = this._fm.formatMoney(this.rowData.cif);
        if (this.rowData.thc == undefined){
            this.rowData.thc = 0;
            this._thc = 0;
        }
        else this._thc = this._fm.formatMoney(this.rowData.thc);
        if (this.rowData.tax == undefined) this._tax = 0;
        else this._tax = this._fm.formatMoney(this.rowData.tax);
        if (this.rowData.vat == undefined) this._vat = 0;
        else this._vat = this._fm.formatMoney(this.rowData.vat);

        setTimeout(() => {
            this.modal.show();
        }, 200);
    }

    save(): void {
        if (this._invoiceDate == undefined) {
            this.notify.warn('Invoice Date is Required!');
            return;
        }
        this.rowData.invoiceDate = moment(this._invoiceDate);

        if (this._invoiceDate == undefined && this.rowData.status != 'NEW') {
            this.notify.warn('Invoice Date is Required!');
            return;
        }

        this.rowData.freight = this._freight != null ? (this._freight.length > 3 ? Number(this._freight.replace(/,/g,'')) : Number(this._freight)) : 0;
        this.rowData.insurance = this._insurance != null ? (this._insurance.length > 3 ? Number(this._insurance.replace(/,/g,'')) : Number(this._insurance)) : 0;
        this.rowData.cif = this._cif != null ? (this._cif.length > 3 ? Number(this._cif.replace(/,/g,'')) : Number(this._cif)) : 0;
        this.rowData.thc = this._thc != null ? (this._thc.length > 3 ? Number(this._thc.replace(/,/g,'')) : Number(this._thc)) : 0;
        this.rowData.tax = this._tax != null ? (this._tax.length > 3 ? Number(this._tax.replace(/,/g,'')) : Number(this._tax)) : 0;
        this.rowData.vat = this._vat != null ? (this._vat.length > 3 ? Number(this._vat.replace(/,/g,'')) : Number(this._vat)) : 0;

        if (this.rowData.freight == 0) {
            this.notify.warn('Freight is Required!');
            return;
        }
        if (this.rowData.insurance == 0) {
            this.notify.warn('Insurance is Required!');
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
        this._service.editInvoice(this.rowData)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this._component.searchDatas();
                this.close();
            });
    }

    expand(header: number) {
        if (header === 1) {
            this._expandInfo = !this._expandInfo;
        }
        if (header === 2) {
            this._expandDetailsInfo = !this._expandDetailsInfo;
        }
    }

    changeFreight(event) {
        this.rowData.freight = event.length > 3 ? Number(event.replace(/,/g,'')) : Number(event);
        this._freight = this._fm.formatMoney(event);
        this.rowData.cif = Number(this.rowData.freight) + Number(this.rowData.insurance) + Number(this.rowData.thc);
        this._cif = this._fm.formatMoney(this.rowData.cif);
    }
    changeInsurance(event) {
        this.rowData.insurance = event.length > 3 ? Number(event.replace(/,/g,'')) : Number(event);
        this._insurance = this._fm.formatMoney(event);
        this.rowData.cif = Number(this.rowData.freight) + Number(this.rowData.insurance) + Number(this.rowData.thc);
        this._cif = this._fm.formatMoney(this.rowData.cif);
    }
    changeThc(event) {
        this.rowData.thc = event.length > 3 ? Number(event.replace(/,/g,'')) : Number(event);
        this._thc = this._fm.formatMoney(event);
        this.rowData.cif = Number(this.rowData.freight) + Number(this.rowData.insurance) + Number(this.rowData.thc);
        this._cif = this._fm.formatMoney(this.rowData.cif);
    }
    changeTax(event) {
        this._tax = this._fm.formatMoney(event);
    }
    changeVat(event) {
        this._vat = this._fm.formatMoney(event);
    }

    close(): void {
        this.modal.hide();
    }
}
