import { DatePipe, formatDate } from '@angular/common';
import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ProdContainerRentalWHPlanServiceProxy, ListPartForOrderDto, ProdOthersServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { AppConsts } from '@shared/AppConsts';
import { HttpClient } from '@angular/common/http';
import * as saveAs from 'file-saver';
import * as moment from 'moment';
import { finalize } from 'rxjs/operators';
import { AgCellTextRendererComponent } from '@app/shared/common/grid/ag-cell-text-renderer/ag-cell-text-renderer.component';
import { CellValueChangedEvent, EditableCallbackParams, GridOptions } from '@ag-grid-enterprise/all-modules';
import { StockReceivingComponent } from './stock-receiving.component';
@Component({
    selector: 'addPurchaseOrder',
    templateUrl: './order-stock-receiving-modal.component.html'
})
export class AddPurchaseOrderModalComponent extends AppComponentBase {
    @ViewChild('addPurchaseOrder', { static: true }) modal: ModalDirective;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
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
    selectedRow: ListPartForOrderDto = new ListPartForOrderDto();
    saveSelectedRow: ListPartForOrderDto = new ListPartForOrderDto();
    rowSelection = 'multiple';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    dataParams: GridParams | undefined;
    gridOptions: GridOptions = {
        columnTypes: {
            editableColumn: {

                editable: (params: EditableCallbackParams) => { return true; },
                valueParser: 'Number(newValue)',
                cellRenderer: 'agAnimateShowChangeCellRenderer',
                filter: 'agNumberColumnFilter',

                cellClass: (params) => { return this.cellEditGetsClass(params); },
                valueGetter: (params) => { return this.syncValidateValueGetter(params) },
                valueSetter: (params) => { return this.syncValidateValueSetter(params) },
            },
            valueColumn: {
                editable: true,
                valueParser: 'Number(newValue)',
                cellClass: 'number-cell',
                cellRenderer: 'agAnimateShowChangeCellRenderer',
                filter: 'agNumberColumnFilter',
            },
        },
    };
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
        tooltip: (params) => params.value,
        autoGroupColumnDef: {
            minWidth: 400,
            cellRendererParams: {
                suppressCount: true,
                checkbox: false,
            },
            pinned: true,
        },
        suppressAggFuncInHeader: true,
        enableCellChangeFlash: true,
        animateRows: true
    };

    saving = false;

    list = [{ value: '', label: '', address: '' }];
    _warehouse;
    _goodsReceived;
    _receiveDate = new Date();
    _selectrow;
    contId = '';
    listCont = '';
    listActualQty = [];
    datasEdit: ListPartForOrderDto[] = [];
    valueChange: string = '';
    columnChange: string = '';
    processNoUpdate: boolean = false;

    constructor(injector: Injector,
        private _service: ProdContainerRentalWHPlanServiceProxy,
        private _component: StockReceivingComponent,
        private _other: ProdOthersServiceProxy,
        private _httpClient: HttpClient,
        private _fm: DataFormatService
    ) {
        super(injector);

        this.viewColDefs = [
            { headerName: this.l('Cfc'), headerTooltip: this.l('Cfc'), field: 'cfc', width: 70 },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', flex: 1 },
            {
                headerName: this.l('Remain Qty'), headerTooltip: this.l('Qty'), field: 'qty', flex: 1, type: 'rightAligned',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.qty),
            },
            //cột cần edit
            {
                headerName: this.l('Order Qty'), headerTooltip: this.l('Order Qty'), field: 'orderQty', flex: 1, type: 'editableColumn', aggFunc: this.calTotal,
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.orderQty)
            },
            {
                headerName: this.l('Standard Price'), headerTooltip: this.l('Standard Price'), field: 'standardPrice', flex: 1, type: 'rightAligned',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.standardPrice)
            },
            {
                headerName: this.l('Moving Price'), headerTooltip: this.l('Moving Price'), field: 'movingPrice', flex: 1, type: 'rightAligned',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.movingPrice)
            },
            {
                headerName: this.l('Amount'), headerTooltip: this.l('Amount'), field: 'orderAmount', flex: 1, type: 'rightAligned', aggFunc: this.calTotal,
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.orderAmount)
            }
        ];

        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
            AgCellTextComponent: AgCellTextRendererComponent
        };
    }

    syncValidateValueGetter(params) {

        try {
            if (params.data == undefined) return 0;

            let field = params.colDef.field;
            let v = params.data[field];
            if (v == null || v == undefined || Number.isNaN(v)) {    // không đúng định dạng
                return 0;
            }
            else {   //hợp lệ
                return v;
            }
        } catch (e) {
            return 0;
        }
    }

    syncValidateValueSetter(params) {
        try {
            if (params.data == undefined || params.data == null) return 0;
            let field = params.colDef.field;
            let v = params.newValue;

            if (v == null || v == undefined || Number.isNaN(v)) {
                params.data[field] = 0;
                params.data['orderAmount'] = params.data['orderQty'] * params.data['standardPrice'] + params.data['movingPrice'];
                params.api.applyTransaction({ update: [params.data] });
                return 0;
            }
            else {  //hợp lệ
                params.newValue = ((params.newValue) < 0) ? params.oldValue : params.newValue;
                params.data[field] = params.newValue;

                if (params.newValue > params.data['qty']) {
                    this.message.warn(this.l('Order Qty không được vượt quá Remain Qty!'), 'Warning');
                    params.data[field] = params.oldValue;
                }
                if (params.newValue == 0) params.data['orderQty'] = 0;
                params.data['orderAmount'] = params.data['orderQty'] * params.data['standardPrice'] + params.data['movingPrice'];
                params.api.applyTransaction({ update: [params.data] });

                return params.newValue;
            }
        } catch (e) {
            return 0;
        }
    }


    onCellValueChanged(params: CellValueChangedEvent) {
        params.data['orderAmount'] = params.data['orderQty'] * params.data['standardPrice'] + params.data['movingPrice'];
        params.api.applyTransaction({ update: [params.data] });

        if (params.newValue == null || Number.isNaN(params.newValue) || params.newValue == undefined) return;

        let ischange = true;
        this.datasEdit.forEach((item) => {  // kiểm tra row change đã tồn tại trong mảng edit chưa
            if (item.keyRow == params.data.keyRow) ischange = false;
        });
        if (ischange && this.processNoUpdate != true) {
            this.datasEdit.push(params.data);
            this.valueChange = params.newValue;
            this.columnChange = params.column.getId();
        }

        params.api.deselectAll();

        var _sumOrderQty = 0;
        var _sumOrderAmount = 0;
        this.data.forEach(e => {
            _sumOrderQty += e.orderQty;
            _sumOrderAmount += e.orderAmount;
        })

        var rows = this.createRow(1, _sumOrderQty, _sumOrderAmount);
        this.dataParams!.api.setPinnedBottomRowData(rows);
    }

    cellEditGetsClass(params) {
        if (params.data == undefined || params.data == null) return ['cell-edit', 'number-cell',];
        else return ['cell-edit', 'number-cell', 'cell-edit-edited'];
    }

    headerEditClass(params) {
        if (params.data == undefined || params.data == null) return [''];
        else return ['cell-edit', 'number-cell', 'cell-edit-edited'];
    }

    ngOnInit() {
        this._other.getListWarehouse()
            .subscribe(result => {
                result.forEach(e => {
                    this.list.push({ value: e.storageLocation, label: e.storageLocation, address: e.addressLanguageVn })
                })
            })
    }

    show(listId: any): void {
        this._other.getListPartForOrder(listId)
            .subscribe((result) => {
                this.data = result ?? [];
                if (result.length > 0) {
                    var _sumOrderQty = 0;
                    var _sumOrderAmount = 0;
                    _sumOrderQty = result[0].grandOrderQty;
                    _sumOrderAmount = result[0].grandOrderAmount;
                    var rows = this.createRow(1, _sumOrderQty, _sumOrderAmount);
                    this.dataParams!.api.setPinnedBottomRowData(rows);
                } else {
                    this.dataParams!.api.setPinnedBottomRowData(null);
                }
            });
        this._warehouse = '';
        this._receiveDate = new Date();
        this._goodsReceived = '';
        this.modal.show();

    }

    callBackDataGrid(params: GridParams) {
        this.dataParams = params;
    }

    save(): void {

    }

    rowClickData: ListPartForOrderDto;
    onRowClick(params) {

        let _rows = document.querySelectorAll<HTMLElement>("body .ag-theme-alpine .ag-center-cols-container .ag-row.ag-row-level-0.ag-row-position-absolute");
        for (let i = 0; _rows[i]; i++) { _rows[i].classList.remove("setcolor_background_rowclick"); }

        if (this.rowClickData && this.rowClickData.id == params.data.id) this.rowClickData = undefined;
        else {
            this.rowClickData = params.data;
            let _row = document.querySelector<HTMLElement>("body .ag-theme-alpine .ag-center-cols-container div[row-id='" + params.node.rowIndex + "'].ag-row.ag-row-level-0.ag-row-position-absolute");
            if (_row) _row.classList.add("setcolor_background_rowclick");
        }

    }

    createRow(count: number, sumOrderQty: number, sumOrderAmount): any[] {
        let result: any[] = [];

        for (var i = 0; i < count; i++) {
            result.push({
                partNo: 'Grand Total',
                orderQty: sumOrderQty,
                orderAmount: sumOrderAmount
            });
        }
        return result;
    }

    calTotal(values) {
        var sum = 0;
        values.forEach(function (value) { sum += Number(value); });
        return sum;
    }

    close(): void {
        this.datasEdit = [];
        this.valueChange = "";
        this.columnChange = "";
        this.modal.hide();
        this.modalClose.emit(null);
    }
}
