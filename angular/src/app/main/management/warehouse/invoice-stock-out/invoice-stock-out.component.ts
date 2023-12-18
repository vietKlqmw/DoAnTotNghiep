import { GridApi } from '@ag-grid-enterprise/all-modules';
import { DatePipe } from '@angular/common';
import { Component, Injector, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ProdInvoiceStockOutDto, ProdInvoiceStockOutServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ceil } from 'lodash';
import { finalize } from 'rxjs/operators';
import * as moment from 'moment';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { AgDropdownRendererComponent } from '@app/shared/common/grid/ag-dropdown-renderer/ag-dropdown-renderer.component';

@Component({
    selector: 'app-invoiceStockOut',
    templateUrl: './invoice-stock-out.component.html',
    styleUrls: ['../../../screen-modal.less'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class InvoiceStockOutComponent extends AppComponentBase implements OnInit {

    defaultColDefs: CustomColDef[] = [];
    colDefs: any;
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    selectedRow: ProdInvoiceStockOutDto = new ProdInvoiceStockOutDto();
    saveSelectedRow: ProdInvoiceStockOutDto = new ProdInvoiceStockOutDto();
    datas: ProdInvoiceStockOutDto = new ProdInvoiceStockOutDto();
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    isLoading: boolean = false;

    invoiceStockOut: string = '';
    invoiceDateFrom: any;
    invoiceDateTo: any;
    _selectrow;
    status: string = '';
    listWarehouse = [
        { label: 'Select Warehouse', value: "" },
        { label: 'Warehouse A1', value: "A1" },
        { label: 'Warehouse A2', value: "A2" },
        { label: 'Warehouse B1', value: "B1" },
        { label: 'Warehouse C1', value: "C1" },
        { label: 'Warehouse C2', value: "C2" }
    ];
    warehouse: string = '';
    listStatus = [
        { key: 'NEW', value: 'NEW' },
        { key: 'NOT PAID (REQUESTED)', value: 'NOT PAID (REQUESTED)' },
        { key: 'PENDING', value: 'PENDING' },
        { key: 'PAID', value: 'PAID' }
    ];

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
    };

    constructor(
        injector: Injector,
        private _service: ProdInvoiceStockOutServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService
    ) {
        super(injector);

        this.colDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 60 },
            { headerName: this.l('Invoice Stock Out No'), headerTooltip: this.l('InvoiceStockOutNo'), field: 'invoiceNoOut', flex: 1 },
            {
                headerName: this.l('Invoice Date'), headerTooltip: this.l('Invoice Date'), field: 'invoiceDate', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.invoiceDate, 'dd/MM/yyyy')
            },
            { 
                headerName: this.l('Status'), headerTooltip: this.l('Status'), field: 'status', flex: 1,
                cellRenderer: 'agSelectRendererComponent',
                list: this.listStatus,
                cellClass: ['RendererCombobox', 'text-center'] 
            },
            { headerName: this.l('Cfc'), headerTooltip: this.l('Cfc'), field: 'listCfc', flex: 1 },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'listPartNo', flex: 1 },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'listPartName', flex: 1 },
            {
                headerName: this.l('Total Order Qty'), headerTooltip: this.l('Total Order Qty'), field: 'totalOrderQty', flex: 1, type: 'rightAligned', aggFunc: this.calTotal,
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.totalOrderQty)
            },
            {
                headerName: this.l('Total Amount'), headerTooltip: this.l('Total Amount'), field: 'totalAmount', flex: 1, type: 'rightAligned', aggFunc: this.calTotal,
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.totalAmount)
            },
            { headerName: this.l('Warehouse'), headerTooltip: this.l('Warehouse'), field: 'warehouse', flex: 1 }
        ];

        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
            agSelectRendererComponent: AgDropdownRendererComponent
        };
    }

    ngOnInit() {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "stt") {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView() {

        setTimeout(() => {
            this.dataParams.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSizeAll();
        })
    }

    searchDatas(): void {
        this.isLoading = true;
        this._service.getProdInvoiceStockOutSearch(
            this.invoiceStockOut,
            this.invoiceDateFrom ? moment(this.invoiceDateFrom) : undefined,
            this.invoiceDateTo ? moment(this.invoiceDateTo) : undefined,
            this.status,
            this.warehouse,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                if (result.totalCount > 0) {
                    var _sumOrderQty = 0;
                    var _sumOrderAmount = 0;
                    _sumOrderQty = result.items[0].grandTotalOrderQty;
                    _sumOrderAmount = result.items[0].grandTotalAmount;
                    var rows = this.createRow(1, _sumOrderQty, _sumOrderAmount);
                    this.dataParams!.api.setPinnedBottomRowData(rows);
                } else {
                    this.dataParams!.api.setPinnedBottomRowData(null);
                }
                this.resetGridView();
                this.isLoading = false;
            });
    }

    clearTextSearch() {
        this.invoiceStockOut = '';
        this.invoiceDateFrom = '';
        this.invoiceDateTo = '';
        this.status = '';
        this.warehouse = '';
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getProdInvoiceStockOutSearch(
            this.invoiceStockOut,
            this.invoiceDateFrom ? moment(this.invoiceDateFrom) : undefined,
            this.invoiceDateTo ? moment(this.invoiceDateTo) : undefined,
            this.status,
            this.warehouse,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatas(this.paginationParams).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.resetGridView();
            this.isLoading = false;
        });
    }

    callBackDataGrid(params: GridParams) {
        this.isLoading = true;
        this.dataParams = params;
        params.api.paginationSetPageSize(this.paginationParams.pageSize);
        this.paginationParams.skipCount =
            ((this.paginationParams.pageNum ?? 1) - 1) * (this.paginationParams.pageSize ?? 0);
        this.paginationParams.pageSize = this.paginationParams.pageSize;
        this.getDatas(this.paginationParams)
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                if (result.totalCount > 0) {
                    var _sumOrderQty = 0;
                    var _sumOrderAmount = 0;
                    _sumOrderQty = result.items[0].grandTotalOrderQty;
                    _sumOrderAmount = result.items[0].grandTotalAmount;
                    var rows = this.createRow(1, _sumOrderQty, _sumOrderAmount);
                    this.dataParams!.api.setPinnedBottomRowData(rows);
                } else {
                    this.dataParams!.api.setPinnedBottomRowData(null);
                }
                this.resetGridView();
                this.isLoading = false;
            });
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => ProdInvoiceStockOutDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new ProdInvoiceStockOutDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);

        this._selectrow = this.saveSelectedRow.id;
    }

    exportToExcel(): void {
        this.isLoading = true;
        this._service.getProdInvoiceStockOutToExcel(
            this.invoiceStockOut,
            this.invoiceDateFrom ? moment(this.invoiceDateFrom) : undefined,
            this.invoiceDateTo ? moment(this.invoiceDateTo) : undefined,
            this.status,
            this.warehouse)
            .pipe(finalize(() => this.isLoading = false))
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            });
    }

    createRow(count: number, sumTotalOrderQty: number, sumTotalOrderAmount: number): any[] {
        let result: any[] = [];

        for (var i = 0; i < count; i++) {
            result.push({
                invoiceNoOut: 'Grand Total',
                totalOrderQty: sumTotalOrderQty,
                totalAmount: sumTotalOrderAmount
            });
        }
        return result;
    }

    calTotal(values) {
        var sum = 0;
        values.forEach(function (value) { sum += Number(value); });
        return sum;
    }

    onCellValueChanged(ev) {
        if((ev.oldValue == 'PENDING' || ev.oldValue == 'NOT PAID (REQUESTED)') && ev.newValue == 'NEW'){
            this.message.warn('Invoice is pending payment, status cannot be changed to NEW!');
            this.callBackDataGrid(this.dataParams!);
        }else if(ev.oldValue == 'PAID'){
            this.message.warn('Invoice already Paid, cannot change status!');
            this.callBackDataGrid(this.dataParams!);
        }else{
            this._service.updateStatusInvoiceStock(ev.data.id.toString(), ev.newValue).subscribe(() => {
                this.callBackDataGrid(this.dataParams!);
                this.notify.success(this.l('SavedSuccessfully'));
            });
        }
    }
}

