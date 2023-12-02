import { Component, Injector, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { ProdOthersServiceProxy, ProdShipmentDto, ProdShipmentServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from "ngx-bootstrap/modal";
import { finalize } from "rxjs/operators";
import { BsDatepickerDirective } from "ngx-bootstrap/datepicker";
import * as moment from 'moment';
import { ShipmentComponent } from "./shipment.component";

@Component({
    selector: 'editModal',
    templateUrl: './edit-shipment-modal.component.html'
})
export class EditShipmentModalComponent extends AppComponentBase {
    @ViewChild('editModal', { static: true }) modal: ModalDirective;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('datepicker2', { static: false }) datepicker2!: BsDatepickerDirective;
    @ViewChild('datepicker3', { static: false }) datepicker3!: BsDatepickerDirective;
    @ViewChild('datepicker4', { static: false }) datepicker4!: BsDatepickerDirective;
    @ViewChild('datepicker5', { static: false }) datepicker5!: BsDatepickerDirective;

    rowData: ProdShipmentDto = new ProdShipmentDto();
    saving = false;

    isEdit: boolean = false;
    header: string = '';
    _shipmentDate: any;
    _etd: any;
    _eta: any;
    _ata: any;
    _atd: any;
    listSupplier = [{ label: '', value: '' }];

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
        const dateValue4 = this.rowData.ata ? new Date(this.rowData.ata?.toString()) : undefined;
        this.datepicker4?.bsValueChange.emit(dateValue4);
        const dateValue5 = this.rowData.atd ? new Date(this.rowData.atd?.toString()) : undefined;
        this.datepicker5?.bsValueChange.emit(dateValue5);

        this.modal.show();
    }

    save(): void {
        this.rowData.shipmentDate = moment(this._shipmentDate);
        this.rowData.etd = moment(this._etd);
        this.rowData.eta = moment(this._eta);
        this.rowData.ata = moment(this._ata);
        this.rowData.atd = moment(this._atd);

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
            this._service.createOrEdit(this.rowData)
                .pipe(finalize(() => { this.saving = false; }))
                .subscribe(() => {
                    this.notify.info(this.l('SavedSuccessfully'));
                    this._component.searchDatas();
                    this.close();
                });
        }
    }

    close(): void {
        this.modal.hide();
    }
}
