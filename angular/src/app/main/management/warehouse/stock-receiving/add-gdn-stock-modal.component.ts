import { DatePipe, formatDate } from '@angular/common';
import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ProdStockReceivingServiceProxy, ProdStockReceivingDto, ProdOthersServiceProxy, GoodsDeliveryNoteExportInput } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppConsts } from '@shared/AppConsts';
import { HttpClient } from '@angular/common/http';
import * as saveAs from 'file-saver';
import * as moment from 'moment';
import { finalize } from 'rxjs/operators';
import { AgCellTextRendererComponent } from '@app/shared/common/grid/ag-cell-text-renderer/ag-cell-text-renderer.component';
import { CellValueChangedEvent, EditableCallbackParams, GridOptions } from '@ag-grid-enterprise/all-modules';
import { StockReceivingComponent } from './stock-receiving.component';
@Component({
    selector: 'addGdnStock',
    templateUrl: './add-gdn-stock-modal.component.html'
})
export class AddGdnStockModalComponent extends AppComponentBase {
    @ViewChild('addGdnStock', { static: true }) modal: ModalDirective;
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
    selectedRow: ProdStockReceivingDto = new ProdStockReceivingDto();
    saveSelectedRow: ProdStockReceivingDto = new ProdStockReceivingDto();
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
    _warehouse;
    _goodsDelivery;
    _deliveryDate;
    isExcel: boolean = true;
    isPdf: boolean = false;
    _selectrow;
    contId = '';
    listCont = '';
    listActualQty = [];
    listOrderQty = [];
    _address;
    datasEdit: ProdStockReceivingDto[] = [];
    valueChange: string = '';
    columnChange: string = '';
    processNoUpdate: boolean = false;

    constructor(injector: Injector,
        private _service: ProdStockReceivingServiceProxy,
        private _component: StockReceivingComponent,
        private _other: ProdOthersServiceProxy,
        private _httpClient: HttpClient,
        private _fm: DataFormatService
    ) {
        super(injector);

        this.viewColDefs = [
            {
                headerName: "", headerTooltip: "", field: "checked", width: 30, pinned: true,
                headerClass: ["align-checkbox-header"],
                cellClass: ["check-box-center"],
                checkboxSelection: true,
                headerCheckboxSelection: true,
                headerCheckboxSelectionFilteredOnly: true
            },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', width: 120, pinned: true },
            {
                headerName: this.l('Remain Qty'), headerTooltip: this.l('Qty'), field: 'remainQty', width: 120, type: 'rightAligned', pinned: true,
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.remainQty),
            },
            {
                headerName: this.l('Order Qty'), headerTooltip: this.l('Order Qty'), field: 'orderQty', width: 110, type: 'rightAligned', pinned: true,
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.orderQty),
            },
            //cột cần edit
            { headerName: this.l('Actual Delivery Qty'), headerTooltip: this.l('Actual Delivery Qty'), field: 'actualDeliveryQty', width: 140, type: 'editableColumn' },

            {
                headerName: this.l('Standard Price'), headerTooltip: this.l('Standard Price'), field: 'standardPrice', width: 130, type: 'rightAligned',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.standardPrice)
            },
            {
                headerName: this.l('Moving Price'), headerTooltip: this.l('Moving Price'), field: 'movingPrice', width: 130, type: 'rightAligned',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.movingPrice)
            },
            {
                headerName: this.l('Amount'), headerTooltip: this.l('Amount'), field: 'amountOrder', width: 130, type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.amountOrder)
            },
            { headerName: this.l('Cfc'), headerTooltip: this.l('Cfc'), field: 'model', width: 70 },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName', width: 250 }
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
                params.data['amountOrder'] = params.data['actualDeliveryQty'] * params.data['standardPrice'] + params.data['movingPrice'];
                params.api.applyTransaction({ update: [params.data] });
                return 0;
            }
            else {  //hợp lệ
                params.newValue = ((params.newValue) < 0) ? params.oldValue : params.newValue;
                params.data[field] = params.newValue;

                if (params.newValue > params.data['remainQty']) {
                    this.message.warn(this.l('Actual Order Qty không được vượt quá Remain Qty!'), 'Warning');
                    params.data[field] = params.oldValue;
                }
                if (params.newValue == 0) params.data['actualDeliveryQty'] = 0;
                params.data['amountOrder'] = params.data['actualDeliveryQty'] * params.data['standardPrice'] + params.data['movingPrice'];
                params.api.applyTransaction({ update: [params.data] });

                return params.newValue;
            }
        } catch (e) {
            return 0;
        }
    }


    onCellValueChanged(params: CellValueChangedEvent) {
        params.data['amountOrder'] = params.data['actualDeliveryQty'] * params.data['standardPrice'] + params.data['movingPrice'];
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
    }

    cellEditGetsClass(params) {
        if (params.data == undefined || params.data == null) return ['cell-edit', 'number-cell',];
        else return ['cell-edit', 'number-cell', 'cell-edit-edited'];
    }

    headerEditClass(params) {
        if (params.data == undefined || params.data == null) return [''];
        else return ['cell-edit', 'number-cell', 'cell-edit-edited'];
    }

    ngOnInit() { }

    show(listId, warehouse): void {
        this._warehouse = warehouse;
        this._deliveryDate = formatDate(new Date(), 'dd/MM/yyyy', 'en-US');
        this._goodsDelivery = warehouse + 'GDN/' + formatDate(new Date(), 'yyMMddHHmmss', 'en-US');
        this.onChangeToExcel(true);
        this._other.getListStockForDeliveryByWarehouse(warehouse)
            .subscribe((result) => {
                this.data = result ?? [];
                this._address = result[0].location;
            });
        this.modal.show();

    }

    callBackDataGrid(params: GridParams) {
        this.dataParams = params;
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => ProdStockReceivingDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new ProdStockReceivingDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);

        this._selectrow = this.saveSelectedRow.id;

        this.contId = '';
        this.listCont = '';
        this.listActualQty = [];
        this.listOrderQty = [];
        if (params.api.getSelectedRows().length) {
            for (var i = 0; i < params.api.getSelectedRows().length; i++) {
                if (i != params.api.getSelectedRows().length - 1) {
                    this.contId += params.api.getSelectedRows()[i].id + ',';
                    this.listCont += params.api.getSelectedRows()[i].id + '-' + params.api.getSelectedRows()[i].actualDeliveryQty + ';';
                } else {
                    this.contId += params.api.getSelectedRows()[i].id;
                    this.listCont += params.api.getSelectedRows()[i].id + '-' + params.api.getSelectedRows()[i].actualDeliveryQty;
                }
                this.listActualQty.push(params.api.getSelectedRows()[i].actualDeliveryQty);
                this.listOrderQty.push(params.api.getSelectedRows()[i].orderQty);
            }
        }
    }

    save(): void {
        if (!this.isExcel && !this.isPdf) {
            this.notify.warn('Export is Required!');
            return;
        }
        if (this.listCont.length < 1) {
            this.notify.warn('Need to choose at least 1 Stock!');
            return;
        }
        let input = Object.assign(new GoodsDeliveryNoteExportInput(), {
            stockId: this.contId,
            listStockId: this.listCont,
            listDeliveryQty: this.listOrderQty,
            deliveryDate: formatDate(new Date(), 'yyyyMMdd', 'en-US'),
            invoiceDate: moment(new Date()),
            goodsDeliveryNoteNo: this._goodsDelivery,
            isExcel: this.isExcel,
            warehouse: this._warehouse,
            address: this._address,
            listActualDeliveryQty: this.listActualQty
        });
        
        this.saving = true;
        this._service.addGdn(input).subscribe(result => {
            if (this.isExcel) {
                this._httpClient.post(`${AppConsts.remoteServiceBaseUrl}/api/ProdFile/ExportGoodsDeliveryNoteExcel`, input, { responseType: 'blob' })
                    .pipe(finalize(() => this.saving = false))
                    .subscribe(blob => {
                        saveAs(blob, "GoodsDeliveryNote_" + formatDate(new Date(), 'yyyyMMdd', 'en-US') + ".xlsx");
                        this.notify.success(this.l('Save Successfully'));
                        this._component.searchDatas();
                        this.close();
                    });
            } else {
                this._httpClient.post(`${AppConsts.remoteServiceBaseUrl}/api/ProdFile/ExportGoodsDeliveryNotePdf`, input, { responseType: 'blob' })
                    .pipe(finalize(() => this.saving = false))
                    .subscribe(blob => {
                        saveAs(blob, "GoodsDeliveryNote_" + formatDate(new Date(), 'yyyyMMdd', 'en-US') + ".pdf");
                        this.notify.success(this.l('Save Successfully'));
                        this._component.searchDatas();
                        this.close();
                    });
            }
        })

    }

    rowClickData: ProdStockReceivingDto;
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

    onChangeToExcel(event) {
        if (event && this.isPdf) {
            this.isExcel = event;
            this.isPdf = !event;
        } else {
            this.isExcel = event;
        }
    }

    onChangeToPdf(event) {
        if (event && this.isExcel) {
            this.isPdf = event;
            this.isExcel = !event;
        } else {
            this.isPdf = event;
        }
    }

    close(): void {
        this.datasEdit = [];
        this.valueChange = "";
        this.columnChange = "";
        this.modal.hide();
        this.modalClose.emit(null);
    }
}
