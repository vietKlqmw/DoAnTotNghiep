import { Component, Injector, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { ProdOthersServiceProxy, ProdShipmentDto, ProdShipmentServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from "ngx-bootstrap/modal";
import { finalize } from "rxjs/operators";
import { BsDatepickerDirective } from "ngx-bootstrap/datepicker";
import * as moment from 'moment';
import { ShipmentComponent } from "./shipment.component";
import { formatDate } from "@angular/common";

@Component({
    selector: 'editModal',
    templateUrl: './edit-shipment-modal.component.html'
})
export class EditShipmentModalComponent extends AppComponentBase {
    @ViewChild('editModal', { static: true }) modal: ModalDirective;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('datepicker2', { static: false }) datepicker2!: BsDatepickerDirective;
    @ViewChild('datepicker3', { static: false }) datepicker3!: BsDatepickerDirective;

    rowData: ProdShipmentDto = new ProdShipmentDto();
    saving = false;

    isEdit: boolean = false;
    header: string = '';
    _shipmentDate: any;
    _etd: any;
    _eta: any;
    _forwarder;
    listSupplier = [{ label: '', value: '' }];
    listForwarder = [{ label: '', value: '' }];
    list = [
        { value: 'NEW', label: "NEW" },
        { value: 'PENDING', label: "PENDING" },
        { value: 'ORDERED', label: "ORDERED" }
    ];
    isOrder: boolean = false;

    constructor(
        private injector: Injector,
        private _service: ProdShipmentServiceProxy,
        private _other: ProdOthersServiceProxy,
        private _component: ShipmentComponent
    ) {
        super(injector);
    }

    ngOnInit() {
        this._other.getListSupplier().subscribe(result => {
            result.forEach(e => {
                this.listSupplier.push({ label: e.supplierName, value: e.supplierNo })
            })
        });
    }

    show(type, material?: ProdShipmentDto): void {
        if (type == 'Edit') this.isEdit = true;
        else this.isEdit = false;
        this.header = type;
        if (material) this.rowData = material;
        else this.rowData = new ProdShipmentDto();

        const dateValue = this.rowData.shipmentDate ? new Date(this.rowData.shipmentDate?.toString()) : undefined;
        this.datepicker?.bsValueChange.emit(dateValue);
        const dateValue2 = this.rowData.etd ? new Date(this.rowData.etd?.toString()) : undefined;
        this.datepicker2?.bsValueChange.emit(dateValue2);
        const dateValue3 = this.rowData.eta ? new Date(this.rowData.eta?.toString()) : undefined;
        this.datepicker3?.bsValueChange.emit(dateValue3);

        // if(this.rowData.status == 'ORDERED') this.isOrder = true;
        // else this.isOrder = false;

        this.listForwarder = [{ label: '', value: '' }];
        this._forwarder = '';
        this._other.getListForwarder(this.rowData.supplierNo).subscribe(result => {
            result.forEach(e => {
                this.listForwarder.push({ label: e.name, value: e.code })
                this._forwarder = this.rowData.forwarder;
            })
        });

        if(type != 'Edit') this.rowData.shipmentNo = 'S' + formatDate(new Date(), 'HH', 'en-US') + 'H' + formatDate(new Date(), 'mm', 'en-US') + 'I' + formatDate(new Date(), 'ss', 'en-US') + 'P';

        setTimeout(() => {
            this.modal.show();
        }, 300)
    }

    save(): void {
        this.rowData.shipmentDate = this._shipmentDate ? moment(this._shipmentDate) : undefined;
        this.rowData.etd = this._etd ? moment(this._etd) : undefined;
        this.rowData.eta = this._eta ? moment(this._eta) : undefined;
        this.rowData.forwarder = this._forwarder;

        if (this.rowData.shipmentNo == null) {
            this.notify.warn('ShipmentNo is Required!');
            return;
        }
        if (this.rowData.supplierNo == null) {
            this.notify.warn('SupplierNo is Required!');
            return;
        }
        if (this.rowData.buyer == null) {
            this.notify.warn('Buyer is Required!');
            return;
        }
        if (this.rowData.fromPort == null) {
            this.notify.warn('From Port is Required!');
            return;
        }
        if (this.rowData.forwarder == null || this.rowData.forwarder == '') {
            this.notify.warn('Forwarder is Required!');
            return;
        }
        if (this.rowData.status != 'NEW' && this.rowData.shipmentDate == undefined) {
            this.notify.warn('ShipmentDate is Required!');
            return;
        }

        if (!this.isEdit) {
            this.saving = true;
            this._service.addShipment(this.rowData)
                .pipe(finalize(() => { this.saving = false; }))
                .subscribe(() => {
                    this.notify.info(this.l('SavedSuccessfully'));
                    this._component.searchDatas();
                    this.close();
                });
        } else {
            this.saving = true;
            this._service.editShipment(this.rowData)
                .pipe(finalize(() => { this.saving = false; }))
                .subscribe(() => {
                    this.notify.info(this.l('SavedSuccessfully'));
                    this._component.searchDatas();
                    this.close();
                });
        }
    }

    changeSupplier(event) {
        this.listForwarder = [{ label: '', value: '' }];
        this._other.getListForwarder(event).subscribe(result => {
            result.forEach(e => {
                this.listForwarder.push({ label: e.name, value: e.code })
            })
        });
    }

    close(): void {
        this.modal.hide();
    }
}
