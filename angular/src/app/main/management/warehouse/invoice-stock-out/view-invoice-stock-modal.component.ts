import { DatePipe, formatDate } from '@angular/common';
import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ProdOthersServiceProxy, ProdInvoiceStockOutServiceProxy, InvoiceStockHistoryExportInput } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppConsts } from '@shared/AppConsts';
import { HttpClient } from '@angular/common/http';
import * as saveAs from 'file-saver';
import * as moment from 'moment';
import { InvoiceStockOutComponent } from './invoice-stock-out.component';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'viewInvoiceStock',
    templateUrl: './view-invoice-stock-modal.component.html'
})
export class ViewInvoiceStockModalComponent extends AppComponentBase {
    @ViewChild('viewInvoiceStock', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    data: any[] = [];
    viewColDefs: any;
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 1000000000,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    dataParams: GridParams | undefined;

    defaultColDef = {
        resizable: true,
        sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        filter: true,
        floatingFilter: true,
        suppressHorizontalScroll: true,
        textFormatter: function (r: any) {
            if (r == null) return null;
            return r.toLowerCase();
        },
        tooltip: (params) => params.value
    };

    saving = false;
    _invoice;
    _warehouse;
    _goodsDelivery;
    _deliveryDate;
    _address;
    _date;
    isDisable: boolean = true;
    list = [{ value: '', label: '', date: undefined, wh: '', address: '', gdn: '' }];

    constructor(injector: Injector,
        private _service: ProdInvoiceStockOutServiceProxy,
        private _component: InvoiceStockOutComponent,
        private _other: ProdOthersServiceProxy,
        private _httpClient: HttpClient,
        private _fm: DataFormatService
    ) {
        super(injector);

        this.viewColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 80 },
            { headerName: this.l('Cfc'), headerTooltip: this.l('Cfc'), field: 'listCfc', width: 70 },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'listPartNo', width: 120 },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'listPartName', width: 250 },
            {
                headerName: this.l('Order Qty'), headerTooltip: this.l('Order Qty'), field: 'totalOrderQty', width: 110, type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.totalOrderQty),
            },
            { 
                headerName: this.l('Delivery Qty'), headerTooltip: this.l('Delivery Qty'), field: 'totalDeliveryQty', width: 140, type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.totalDeliveryQty), 
            },
            {
                headerName: this.l('Standard Price'), headerTooltip: this.l('Standard Price'), field: 'standardPrice', width: 130, type: 'rightAligned',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.standardPrice)
            },
            {
                headerName: this.l('Moving Price'), headerTooltip: this.l('Moving Price'), field: 'movingPrice', width: 130, type: 'rightAligned',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.movingPrice)
            },
            {
                headerName: this.l('Amount'), headerTooltip: this.l('Amount'), field: 'totalAmount', width: 130, type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.totalAmount)
            }
        ];

        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent
        };
    }

    ngOnInit() {
        this._other.getListInvocieStockOut()
            .subscribe(result => {
                result.forEach(e => {
                    this.list.push({ value: e.invoiceNoOut, label: e.invoiceNoOut, date: e.invoiceDate, wh: e.warehouse, address: e.addressLanguageVn, gdn: e.goodsDeliveryNoteNo });
                })
            })
    }

    show(): void {
        this.isDisable = true;
        this.data = [];
        this._warehouse = '';
        this._deliveryDate = '';
        this._goodsDelivery = '';
        this._invoice = null;
        this._address = '';
        this.modal.show();

    }

    callBackDataGrid(params: GridParams) {
        this.dataParams = params;
    }

    save(ev): void {

        let input = Object.assign(new InvoiceStockHistoryExportInput(), {
            invoice: this._invoice,
            isExcel: ev,
            deliveryDate: formatDate(new Date(this._date.toString()), 'yyyyMMdd', 'en-US'),
            goodsDeliveryNoteNo: this._goodsDelivery,
            warehouse: this._warehouse,
            address: this._address
        });

        this.saving = true;
        if (ev) {
            this._httpClient.post(`${AppConsts.remoteServiceBaseUrl}/api/ProdFile/ExportGoodsDeliveryNoteHistoryExcel`, input, { responseType: 'blob' })
                .pipe(finalize(() => this.saving = false))
                .subscribe(blob => {
                    saveAs(blob, "GoodsDeliveryNote_" + formatDate(new Date(), 'yyyyMMdd', 'en-US') + ".xlsx");
                    this.notify.success(this.l('Export Successfully'));
                });
        } else {
            this._httpClient.post(`${AppConsts.remoteServiceBaseUrl}/api/ProdFile/ExportGoodsDeliveryNoteHistoryPdf`, input, { responseType: 'blob' })
                .pipe(finalize(() => this.saving = false))
                .subscribe(blob => {
                    saveAs(blob, "GoodsDeliveryNote_" + formatDate(new Date(), 'yyyyMMdd', 'en-US') + ".pdf");
                    this.notify.success(this.l('Export Successfully'));
                });
        }
    }

    changeInvoice(ev) {
        if (ev == null || ev == undefined || ev == '') {
            this._deliveryDate = '';
            this._warehouse = '';
            this._address = '';
            this._goodsDelivery = '';
            this.isDisable = true;
        }
        else {
            this._date = this.list.filter(e => e.value == ev)[0].date;
            this._deliveryDate = formatDate(this._date, 'dd/MM/yyyy', 'en-US');
            this._warehouse = this.list.filter(e => e.value == ev)[0].wh;
            this._address = this.list.filter(e => e.value == ev)[0].address;
            this._goodsDelivery = this.list.filter(e => e.value == ev)[0].gdn;

            this._service.viewHistoryDelivery(ev)
                .subscribe((result) => {
                    this.data = result ?? [];

                    if (this.data.length > 0) this.isDisable = false;
                    else this.isDisable = true;
                });
        }
    }

    close(): void {
        this.modal.hide();
        this.modalClose.emit(null);
    }
}
