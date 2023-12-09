import { Component, Injector, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { ProdCustomsDeclareDto, ProdCustomsDeclareServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from "ngx-bootstrap/modal";
import { finalize } from "rxjs/operators";
import { BsDatepickerDirective } from "ngx-bootstrap/datepicker";
import * as moment from 'moment';
import { CustomsDeclareComponent } from "./customs-declare.component";

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
    _declareDate: any;
    _invoiceDate: any;
    list = [{ value: '', label: '' }];

    constructor(
        private injector: Injector,
        private _service: ProdCustomsDeclareServiceProxy,
        private _component: CustomsDeclareComponent
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

        this.modal.show();
    }

    save(): void {
        this.rowData.declareDate = this._declareDate ? moment(this._declareDate) : undefined;

        if (this.rowData.customsDeclareNo == null) {
            this.notify.warn('CustomsDeclareNo is Required!');
            return;
        }
        if (this.rowData.billOfLadingNo == null) {
            this.notify.warn('BillOfLadingNo is Required!');
            return;
        }
        if (this.rowData.invoiceNo == null) {
            this.notify.warn('InvoiceNo is Required!');
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

    close(): void {
        this.modal.hide();
    }
}