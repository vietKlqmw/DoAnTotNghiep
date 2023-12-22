import { Component, Injector, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { ProdOthersServiceProxy, ProdOrderPartDto, ProdOrderPartServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from "ngx-bootstrap/modal";
import { finalize } from "rxjs/operators";
import { OrderPartComponent } from "./order-part.component";
import { DataFormatService } from "@app/shared/common/services/data-format.service";
import { BsDatepickerDirective } from "ngx-bootstrap/datepicker";
import * as moment from "moment";

@Component({
    selector: 'editModal',
    templateUrl: './edit-order-part-modal.component.html'
})
export class EditOrderPartModalComponent extends AppComponentBase {
    @ViewChild('editModal', { static: true }) modal: ModalDirective;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    rowData: ProdOrderPartDto = new ProdOrderPartDto();
    shipmentData = [];
    saving = false;

    isEdit: boolean = false;
    header: string = '';
    listPart = [{ label: '', value: '', supplier: '', cfc: '', price: undefined, partname: '', bom: '', material: undefined }];
    _qty;
    _orderDate = new Date();
    _amountUnit;
    _totalAmount;
    list = [
        { value: 'PENDING', label: "PENDING" },
        { value: 'ORDER', label: "ORDER" }
    ];
    _status;

    constructor(
        private injector: Injector,
        private _service: ProdOrderPartServiceProxy,
        private _other: ProdOthersServiceProxy,
        private _component: OrderPartComponent,
        private _fm: DataFormatService
    ) {
        super(injector);
    }

    ngOnInit() {
        this._other.getListPartForOrderToWarehouse().subscribe(result => {
            this.shipmentData = result;
            result.forEach(e => {
                this.listPart.push({ label: e.partNo + '/' + e.partName, value: e.partNo, supplier: e.supplierNo, cfc: e.carfamilyCode, price: e.standardPrice, partname: e.partName, bom: e.baseUnitOfMeasure, material: e.materialId })
            })
        });

    }

    show(type, material?: ProdOrderPartDto): void {
        this._amountUnit = '';
        this._qty = '';
        this._totalAmount = '';
        if (type == 'Edit') this.isEdit = true;
        else this.isEdit = false;
        this.header = type;
        if (material) this.rowData = material;
        else this.rowData = new ProdOrderPartDto();

        this._status = this.rowData.status == null ? 'ORDER' : this.rowData.status;

        const dateValue = this.rowData.orderDate ? new Date(this.rowData.orderDate?.toString()) : new Date();
        this.datepicker?.bsValueChange.emit(dateValue);

        if (this.rowData.qty == undefined){
            this.rowData.qty = 0;
            this._qty = 0;
        }
        else this._qty = this._fm.formatMoney(this.rowData.qty);

        this._amountUnit = this._fm.formatMoney(this.rowData.amountUnit);

        if (this.rowData.totalAmount == undefined){
            this.rowData.totalAmount = 0;
            this._totalAmount = 0;
        }
        else this._totalAmount = this._fm.formatMoney(this.rowData.totalAmount);

        this.modal.show();

    }

    save(): void {
        if (this.rowData.partNo == null) {
            this.notify.warn('Part is Required!');
            return;
        }
        if (this.rowData.qty == null || this.rowData.qty < 1) {
            this.notify.warn('Qty is Required!');
            return;
        }
        if (this._orderDate == null || this._orderDate == undefined) {
            this.notify.warn('Orde rDate is Required!');
            return;
        }
        this.rowData.status = this._status;
        this.rowData.orderDate = moment(this._orderDate);

        this.saving = true;
        this._service.editOrderPart(this.rowData)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this._component.searchDatas();
                this.close();
            });
    }

    changePart(ev) {
       this.rowData.supplierNo = this.listPart.filter(e => e.value == ev)[0].supplier;
       this.rowData.carfamilyCode = this.listPart.filter(e => e.value == ev)[0].cfc;
       this.rowData.partName = this.listPart.filter(e => e.value == ev)[0].partname;
       this.rowData.amountUnit = this.listPart.filter(e => e.value == ev)[0].price;
       this.rowData.baseUnitOfMeasure = this.listPart.filter(e => e.value == ev)[0].bom;
       this.rowData.materialId = this.listPart.filter(e => e.value == ev)[0].material;
       this._amountUnit = this._fm.formatMoney(this.rowData.amountUnit);
    }

    changeQty(event) {
        this.rowData.qty = event.length > 3 ? Number(event.replace(/,/g,'')) : Number(event);
        this.rowData.totalAmount = Number(this.rowData.qty) * this.rowData.amountUnit;
        this._qty = this._fm.formatMoney(event);
        this._totalAmount = this._fm.formatMoney(this.rowData.totalAmount);
    }

    close(): void {
        this.modal.hide();
    }
}
