import { DatePipe, formatDate } from '@angular/common';
import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GoodsReceivedNoteHistoryExportInput, ProdContainerRentalWHPlanServiceProxy, ProdOthersServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { HttpClient } from '@angular/common/http';
import { AppConsts } from '@shared/AppConsts';
import { finalize } from 'rxjs/operators';
import * as saveAs from 'file-saver';

@Component({
    selector: 'viewHistoryReceive',
    templateUrl: './view-history-receive-modal.component.html'
})
export class ViewHistoryReceiveModalComponent extends AppComponentBase {
    @ViewChild('viewHistoryReceive', { static: true }) modal: ModalDirective;
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

    list = [{ value: '', label: '', date: undefined, wh: '' }];
    _warehouse;
    _goodsReceived;
    _receiveDate;
    _date;
    isDisable: boolean = true;

    constructor(injector: Injector,
        private _service: ProdContainerRentalWHPlanServiceProxy,
        private _other: ProdOthersServiceProxy,
        private _httpClient: HttpClient,
        private _fm: DataFormatService
    ) {
        super(injector);

        this.viewColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 80 },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', width: 120 },
            {
                headerName: this.l('Qty'), headerTooltip: this.l('Qty'), field: 'usageQty', width: 100, type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.usageQty)
            },
            {
                headerName: this.l('Actual Qty'), headerTooltip: this.l('Actual Qty'), field: 'realQty', width: 110, type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.realQty)
            },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName', width: 250 },
            { headerName: this.l('Container No'), headerTooltip: this.l('Container No'), field: 'containerNo', width: 130 },
            { headerName: this.l('Invoice No'), headerTooltip: this.l('Invoice No'), field: 'invoiceNo', width: 130 },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', width: 120 },
            { headerName: this.l('Forwarder'), headerTooltip: this.l('Forwarder'), field: 'forwarder', width: 120 },
            {
                headerName: this.l('Cost'), headerTooltip: this.l('Cost'), field: 'cost', width: 100, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.cost != null ? this._fm.formatMoney_decimal(params.data?.cost) : 0),
            },
            {
                headerName: this.l('Insurance'), headerTooltip: this.l('Insurance'), field: 'insurance', width: 110, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.insurance != null ? this._fm.formatMoney_decimal(params.data?.insurance) : 0),
            },
            {
                headerName: this.l('Freight'), headerTooltip: this.l('Freight'), field: 'freight', width: 100, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.freight != null ? this._fm.formatMoney_decimal(params.data?.freight) : 0),
            },
            {
                headerName: this.l('C.I.F'), headerTooltip: this.l('Cif'), field: 'cif', width: 100, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.cif != null ? this._fm.formatMoney_decimal(params.data?.cif) : 0),
            },
            {
                headerName: this.l('THC'), headerTooltip: this.l('THC'), field: 'thc', width: 100, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.thc != null ? this._fm.formatMoney_decimal(params.data?.thc) : 0),
            },
            {
                headerName: this.l('TAX'), headerTooltip: this.l('Tax'), field: 'tax', width: 100, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.tax != null ? this._fm.formatMoney_decimal(params.data?.tax) : 0),
            },
            {
                headerName: this.l('VAT'), headerTooltip: this.l('Vat'), field: 'vat', width: 100, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.vat != null ? this._fm.formatMoney_decimal(params.data?.vat) : 0),
            },
            { headerName: this.l('Currency'), headerTooltip: this.l('Currency'), field: 'currency', width: 100 }
        ];

        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent
        };
    }

    ngOnInit() {
        this._other.getListGRNForViewHistory()
            .subscribe(result => {
                result.forEach(e => {
                    this.list.push({ value: e.goodsReceivedNoteNo, label: e.goodsReceivedNoteNo, date: e.receiveDate, wh: e.warehouse });
                })
            })
    }

    show(): void {
        this.list = [{ value: '', label: '', date: undefined, wh: '' }];
        this._other.getListGRNForViewHistory()
            .subscribe(result => {
                result.forEach(e => {
                    this.list.push({ value: e.goodsReceivedNoteNo, label: e.goodsReceivedNoteNo, date: e.receiveDate, wh: e.warehouse });
                })
            })
        this.isDisable = true;
        this.data = [];
        this._warehouse = '';
        this._receiveDate = '';
        this._goodsReceived = null;
        this.modal.show();

    }

    callBackDataGrid(params: GridParams) {
        this.dataParams = params;
    }

    save(ev): void {
        let input = Object.assign(new GoodsReceivedNoteHistoryExportInput(), {
            goodsReceivedNoteNo: this._goodsReceived,
            isExcel: ev,
            receiveDate: formatDate(new Date(this._date.toString()), 'yyyyMMdd', 'en-US')
        });

        this.saving = true;
        if (ev) {
            this._httpClient.post(`${AppConsts.remoteServiceBaseUrl}/api/ProdFile/ExportGoodsReceivedNoteHistoryExcel`, input, { responseType: 'blob' })
                .pipe(finalize(() => this.saving = false))
                .subscribe(blob => {
                    saveAs(blob, "GoodsReceivedNote_" + formatDate(new Date(this._date.toString()), 'yyyyMMdd', 'en-US') + ".xlsx");
                    this.notify.success(this.l('Export Successfully'));
                });
        } else {
            this._httpClient.post(`${AppConsts.remoteServiceBaseUrl}/api/ProdFile/ExportGoodsReceivedNoteHistoryPdf`, input, { responseType: 'blob' })
                .pipe(finalize(() => this.saving = false))
                .subscribe(blob => {
                    saveAs(blob, "GoodsReceivedNote_" + formatDate(new Date(this._date.toString()), 'yyyyMMdd', 'en-US') + ".pdf");
                    this.notify.success(this.l('Export Successfully'));
                });
        }
    }

    changeGRN(ev) {
        if (ev == null || ev == undefined || ev == '') {
            this._receiveDate = '';
            this._warehouse = '';
            this.isDisable = true;
        }
        else {
            this._date = this.list.filter(e => e.value == ev)[0].date;
            this._receiveDate = formatDate(this._date, 'dd/MM/yyyy', 'en-US');
            this._warehouse = this.list.filter(e => e.value == ev)[0].wh;

            this._service.viewHistoryReceive(ev)
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
