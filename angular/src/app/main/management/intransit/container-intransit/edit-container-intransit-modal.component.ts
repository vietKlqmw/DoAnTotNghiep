import { Component, Injector, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { ProdOthersServiceProxy, ProdContainerIntransitDto, ProdContainerIntransitServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from "ngx-bootstrap/modal";
import { finalize } from "rxjs/operators";
import { BsDatepickerDirective } from "ngx-bootstrap/datepicker";
import * as moment from 'moment';
import { ContainerIntransitComponent } from "./container-intransit.component";

@Component({
    selector: 'editModal',
    templateUrl: './edit-container-intransit-modal.component.html'
})
export class EditContainerIntransitModalComponent extends AppComponentBase {
    @ViewChild('editModal', { static: true }) modal: ModalDirective;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('datepicker2', { static: false }) datepicker2!: BsDatepickerDirective;
    @ViewChild('datepicker3', { static: false }) datepicker3!: BsDatepickerDirective;

    rowData: ProdContainerIntransitDto = new ProdContainerIntransitDto();
    saving = false;

    isEdit: boolean = false;
    header: string = '';
    _shippingDate: any;
    _portDate: any;
    _transDate: any;
    listSupplier = [{ label: '', value: '' }];
    listShipment = [{ label: '', value: undefined }];
    listForwarder = [{ label: '', value: '' }];
    _forwarder: string = '';

    constructor(
        private injector: Injector,
        private _service: ProdContainerIntransitServiceProxy,
        private _other: ProdOthersServiceProxy,
        private _component: ContainerIntransitComponent
    ) {
        super(injector);
    }

    ngOnInit() {
        this._other.getListSupplier().subscribe(result => {
            result.forEach(e => {
                this.listSupplier.push({ label: e.supplierName, value: e.supplierNo })
            })
        });

        this._other.getListShipmentNewOrPending().subscribe(result => {
            result.forEach(e => {
                this.listShipment.push({ label: e.shipmentNo, value: e.shipmentId })
            })
        });

    }

    show(type, material?: ProdContainerIntransitDto): void {
        if (type == 'Edit') this.isEdit = true;
        else this.isEdit = false;
        this.header = type;
        if (material) this.rowData = material;
        else this.rowData = new ProdContainerIntransitDto();

        const dateValue = this.rowData.shippingDate ? new Date(this.rowData.shippingDate?.toString()) : undefined;
        this.datepicker?.bsValueChange.emit(dateValue);
        const dateValue2 = this.rowData.portDate ? new Date(this.rowData.portDate?.toString()) : undefined;
        this.datepicker2?.bsValueChange.emit(dateValue2);
        const dateValue3 = this.rowData.transactionDate ? new Date(this.rowData.transactionDate?.toString()) : undefined;
        this.datepicker3?.bsValueChange.emit(dateValue3);

        this.listForwarder = [{ label: '', value: '' }];
        this._forwarder = '';
        this._other.getListForwarder(this.rowData.supplierNo).subscribe(result => {
            result.forEach(e => {
                this.listForwarder.push({ label: e.name, value: e.code })
                this._forwarder = this.rowData.forwarder;
            })
        });

        setTimeout(() => {
            this.modal.show();
        }, 200)

    }

    save(): void {
        this.rowData.shippingDate = this._shippingDate ? moment(this._shippingDate) : undefined;
        this.rowData.portDate = this._portDate ? moment(this._portDate) : undefined;
        this.rowData.transactionDate = this._transDate ? moment(this._transDate) : undefined;
        this.rowData.forwarder = this._forwarder;

        if (this.rowData.containerNo == null) {
            this.notify.warn('ContainerNo is Required!');
            return;
        }
        if (this.rowData.supplierNo == null) {
            this.notify.warn('SupplierNo is Required!');
            return;
        }
        if (this.rowData.shipmentId == null) {
            this.notify.warn('SupplierId is Required!');
            return;
        }
        this.saving = true;
        this._service.editContainerIntransit(this.rowData)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this._component.searchDatas();
                this.close();
            });
    }

    changeForwarder(event){
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
