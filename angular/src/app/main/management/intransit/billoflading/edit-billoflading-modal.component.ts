import { Component, Injector, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { ProdBillOfLadingDto, ProdBillOfLadingServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from "ngx-bootstrap/modal";
import { finalize } from "rxjs/operators";
import { BsDatepickerDirective } from "ngx-bootstrap/datepicker";
import * as moment from 'moment';
import { BillofladingComponent } from "./billoflading.component";

@Component({
    selector: 'editModal',
    templateUrl: './edit-billoflading-modal.component.html'
})
export class EditBillOfLadingModalComponent extends AppComponentBase {
    @ViewChild('editModal', { static: true }) modal: ModalDirective;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    rowData: ProdBillOfLadingDto = new ProdBillOfLadingDto();
    saving = false;
    _status: string = '';
    _billDate: any;
    list = [];
    listWhenDateEmpty = [
        { value: 'NEW', label: "NEW" },
        { value: 'PENDING', label: "PENDING" },
        { value: 'PAID', label: "PAID" }
    ];
    listWhenDateNotEmpty = [
        { value: 'PENDING', label: "PENDING" },
        { value: 'PAID', label: "PAID" }
    ];

    constructor(
        private injector: Injector,
        private _service: ProdBillOfLadingServiceProxy,
        private _component: BillofladingComponent
    ) {
        super(injector);
    }

    ngOnInit() {
        this.list = this.listWhenDateEmpty;
    }

    show(material: ProdBillOfLadingDto): void {
        this.rowData = material;

        const dateValue = this.rowData.billDate ? new Date(this.rowData.billDate?.toString()) : new Date();
        this.datepicker?.bsValueChange.emit(dateValue);

        this._status = this.rowData.statusCode == 'NEW' ? 'PAID' : this.rowData.statusCode;

        this.modal.show();
    }

    save(): void {
        this.rowData.billDate = this._billDate ? moment(this._billDate) : undefined;
        this.rowData.statusCode = this._status;
        this.saving = true;
        this._service.editBillOfLading(this.rowData)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this._component.searchDatas();
                this.close();
            });
    }

    changeStatus(event) {
        this._billDate = event;
        if (this._billDate != undefined) {
            this.list = this.listWhenDateNotEmpty;
            this._status = 'PAID';
        } else {
            this.list = this.listWhenDateEmpty;
            this._status = 'NEW';
        }
    }

    close(): void {
        this.modal.hide();
    }
}
