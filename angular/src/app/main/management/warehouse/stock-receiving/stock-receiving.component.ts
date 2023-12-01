import { GridApi } from '@ag-grid-enterprise/all-modules';
import { DatePipe } from '@angular/common';
import { Component, Injector, OnInit, ViewEncapsulation } from '@angular/core';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ProdStockReceivingDto, ProdStockReceivingServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ceil } from 'lodash';
import { finalize } from 'rxjs/operators';
import * as moment from 'moment';
import { DataFormatService } from '@app/shared/common/services/data-format.service';

@Component({
    selector: 'app-stock-receiving',
    templateUrl: './stock-receiving.component.html',
    styleUrls: ['../../../screen-modal.less'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class StockReceivingComponent extends AppComponentBase implements OnInit {
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
    selectedRow: ProdStockReceivingDto = new ProdStockReceivingDto();
    saveSelectedRow: ProdStockReceivingDto = new ProdStockReceivingDto();
    datas: ProdStockReceivingDto = new ProdStockReceivingDto();
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    isLoading: boolean = false;

    containerNo: string = '';
    partNo: string = '';
    invoiceNo: string = '';
    supplierNo: string = '';
    model: string = '';
    workingDateFrom: any;
    workingDateTo: any;
    _selectrow;

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
        private _service: ProdStockReceivingServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService
    ) {
        super(injector);

        this.colDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 60 },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNoNormalizedS4', flex: 1 },
            { headerName: this.l('Color Sfx'), headerTooltip: this.l('Color Sfx'), field: 'colorSfx', flex: 1 },
            { headerName: this.l('Cfc'), headerTooltip: this.l('Cfc'), field: 'cfc', flex: 1 },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', flex: 1 },
            { headerName: this.l('Container No'), headerTooltip: this.l('Container No'), field: 'containerNo', flex: 1 },
            { headerName: this.l('Invoice No'), headerTooltip: this.l('Invoice No'), field: 'invoiceNo', flex: 1 },
            {
                headerName: this.l('Qty'), headerTooltip: this.l('Qty'), field: 'qty', flex: 1, type: 'rightAligned',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.qty),
                aggFunc: this.calTotal
            },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName', flex: 1 },
            {
                headerName: this.l('Transaction Datetime'), headerTooltip: this.l('Transaction Datetime'), field: 'transactionDatetime', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.transactionDatetime, 'dd/MM/yyyy')
            },
            {
                headerName: this.l('Working Date'), headerTooltip: this.l('Working Date'), field: 'workingDate', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.workingDate, 'dd/MM/yyyy')
            },
            { headerName: this.l('Material Id'), headerTooltip: this.l('Material Id'), field: 'materialId', flex: 1 }
        ];

        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
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
        this._service.getProdStockReceivingSearch(
            this.partNo,
            this.workingDateFrom ? moment(this.workingDateFrom) : undefined,
            this.workingDateTo ? moment(this.workingDateTo) : undefined,
            this.supplierNo,
            this.containerNo,
            this.invoiceNo,
            this.model,
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
                    var _sumQty = 0;
                    _sumQty = result.items[0].grandQty;
                    var rows = this.createRow(1, _sumQty);
                    this.dataParams!.api.setPinnedBottomRowData(rows);
                } else {
                    this.dataParams!.api.setPinnedBottomRowData(null);
                }
                this.resetGridView();
                this.isLoading = false;
            });
    }

    clearTextSearch() {
        this.partNo = '';
        this.containerNo = '';
        this.invoiceNo = '';
        this.model = '';
        this.supplierNo = '';
        this.workingDateFrom = '';
        this.workingDateTo = '';
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getProdStockReceivingSearch(
            this.partNo,
            this.workingDateFrom ? moment(this.workingDateFrom) : undefined,
            this.workingDateTo ? moment(this.workingDateTo) : undefined,
            this.supplierNo,
            this.containerNo,
            this.invoiceNo,
            this.model,
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
                this.resetGridView();
                this.isLoading = false;
            });
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => ProdStockReceivingDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new ProdStockReceivingDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);

        this._selectrow = this.saveSelectedRow.id;
    }

    exportToExcel(): void {
        this.isLoading = true;
        this._service.getProdStockReceivingToExcel(
            this.partNo,
            this.workingDateFrom ? moment(this.workingDateFrom) : undefined,
            this.workingDateTo ? moment(this.workingDateTo) : undefined,
            this.supplierNo,
            this.containerNo,
            this.invoiceNo,
            this.model)
            .pipe(finalize(() => this.isLoading = false))
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            });
    }

    createRow(count: number, sumQty: number): any[] {
        let result: any[] = [];

        for (var i = 0; i < count; i++) {
            result.push({
                partNo: 'Grand Total',
                qty: sumQty,
            });
        }
        return result;
    }

    calTotal(values) {
        var sum = 0;
        values.forEach(function (value) { sum += Number(value); });
        return sum;
    }
}

