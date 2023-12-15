import { Component, Injector, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { ProdStockReceivingDto, ProdStockReceivingServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from "ngx-bootstrap/modal";
import { finalize } from "rxjs/operators";
import { BsDatepickerDirective } from "ngx-bootstrap/datepicker";
import { DataFormatService } from "@app/shared/common/services/data-format.service";
import { StockReceivingComponent } from "./stock-receiving.component";

@Component({
    selector: 'editModal',
    templateUrl: './update-order-stock-modal.component.html'
})
export class UpdateOrderStockModalComponent extends AppComponentBase {
    @ViewChild('editModal', { static: true }) modal: ModalDirective;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    rowData: ProdStockReceivingDto = new ProdStockReceivingDto();
    saving = false;
    _orderQty;
    _standardPrice;
    _movingPrice;
    _amount;

    constructor(
        private injector: Injector,
        private _service: ProdStockReceivingServiceProxy,
        private _component: StockReceivingComponent,
        private _fm: DataFormatService
    ) {
        super(injector);
    }

    ngOnInit() { }

    show(material: ProdStockReceivingDto): void {
        this.rowData = material;

        this._orderQty = this._fm.formatMoney(this.rowData.orderQty);
        this._standardPrice = this._fm.formatMoney(this.rowData.standardPrice);
        this._movingPrice = this._fm.formatMoney(this.rowData.movingPrice);
        this._amount = this._fm.formatMoney(this.rowData.amountOrder);

        this.modal.show();
    }

    save(): void {
        if (this._orderQty <= 0) {
            this.notify.warn('Order Qty must be greater than 0!');
            return;
        }
        if (this._orderQty >= this.rowData.remainQty){
            this.notify.warn('Remain Qty cannot be less than Order Qty!');
            return;
        }
        this.rowData.orderQty = this._orderQty;
        this.saving = true;
        this._service.updateOrderQtyStock(this.rowData.id, this.rowData.orderQty)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this._component.searchDatas();
                this.close();
            });
    }

    changeOrderQty(event) {
            this._orderQty = this._fm.formatMoney(event);
            this.rowData.orderQty = event.length > 3 ? Number(event.replace(/,/g, '')) : Number(event);
            this._amount = this._fm.formatMoney(Number(this.rowData.orderQty) * Number(this.rowData.standardPrice) + Number(this.rowData.movingPrice));
    }

    close(): void {
        this.modal.hide();
    }
}
