import { Component, Injector, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { ProdOthersServiceProxy, ProdContainerIntransitDto, ProdContainerIntransitServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from "ngx-bootstrap/modal";
import { finalize } from "rxjs/operators";
import { BsDatepickerDirective } from "ngx-bootstrap/datepicker";
import * as moment from 'moment';
import { ContainerIntransitComponent } from "./container-intransit.component";
import { formatDate } from "@angular/common";

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
    shipmentData = [];
    saving = false;

    isEdit: boolean = false;
    header: string = '';
    _shippingDate: any;
    _shippingDateSub: any;
    _portDate: any;
    _transDate: any;
    listShipment = [{ label: '', value: undefined }];
    listPart = [{ label: '', value: undefined }];
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

        this._other.getListShipmentNewOrPending().subscribe(result => {
            this.shipmentData = result;
            result.forEach(e => {
                this.listShipment.push({ label: e.shipmentNo, value: e.shipmentId })
            })
        });

        this._other.getListPart().subscribe(result => {
            this.shipmentData = result;
            result.forEach(e => {
                this.listPart.push({ label: e.partNo, value: e.partId })
            })
        });

    }

    show(type, material?: ProdContainerIntransitDto): void {
        if (type == 'Edit') this.isEdit = true;
        else this.isEdit = false;
        this.header = type;
        if (material) this.rowData = material;
        else this.rowData = new ProdContainerIntransitDto();

        // const dateValue = this.rowData.shippingDate ? new Date(this.rowData.shippingDate?.toString()) : undefined;
        // this.datepicker?.bsValueChange.emit(dateValue);
        const dateValue2 = this.rowData.portDate ? new Date(this.rowData.portDate?.toString()) : undefined;
        this.datepicker2?.bsValueChange.emit(dateValue2);
        const dateValue3 = this.rowData.transactionDate ? new Date(this.rowData.transactionDate?.toString()) : undefined;
        this.datepicker3?.bsValueChange.emit(dateValue3);

        this._shippingDate = this.rowData.shippingDate ? formatDate(new Date(this.rowData.shippingDate?.toString()), 'dd/MM/yyyy', 'en-US') : undefined;
        this._shippingDateSub = this.rowData.shippingDate ? formatDate(new Date(this.rowData.shippingDate?.toString()), 'MM/dd/yyyy', 'en-US') : undefined;

        setTimeout(() => {
            this.modal.show();
        }, 300)

    }

    save(): void {
        this.rowData.shippingDate = this._shippingDateSub ? moment(this._shippingDateSub) : undefined;
        this.rowData.portDate = this._portDate ? moment(this._portDate) : undefined;
        this.rowData.transactionDate = this._transDate ? moment(this._transDate) : undefined;
        this.rowData.forwarder = this._forwarder;

        if (this.rowData.containerNo == null) {
            this.notify.warn('ContainerNo is Required!');
            return;
        }
        if(this.rowData.transactionDate != undefined){
            if(this.rowData.portDate == undefined){
                this.notify.warn('PortDate is Required!');
                return;
            }
        }
        if (this.rowData.partListId == null) {
            this.notify.warn('PartListId is Required!');
            return;
        }
        if (this.rowData.usageQty == null) {
            this.notify.warn('Usage Qty is Required!');
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

    changeSupplier(e){
        this._other.getListShipmentById(e).subscribe(result => {
            if(result.length > 0){
                this.rowData.supplierNo = result[0].supplierNo;
                this._shippingDate = result[0].shipmentDate ? formatDate(new Date(result[0].shipmentDate?.toString()), 'dd/MM/yyyy', 'en-US') : undefined;
                this._shippingDateSub = result[0].shipmentDate ? formatDate(new Date(result[0].shipmentDate?.toString()), 'MM/dd/yyyy', 'en-US') : undefined;
            }
        })
    }

    close(): void {
        this.modal.hide();
    }
}
