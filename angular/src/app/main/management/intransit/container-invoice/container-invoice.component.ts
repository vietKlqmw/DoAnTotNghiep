import { GridApi } from '@ag-grid-enterprise/all-modules';
import { DatePipe } from '@angular/common';
import { Component, Injector, OnInit, ViewEncapsulation } from '@angular/core';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ProdContainerInvoiceDto, ProdContainerInvoiceServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ceil } from 'lodash';
import { finalize } from 'rxjs/operators';
import * as moment from 'moment';
import { DataFormatService } from '@app/shared/common/services/data-format.service';

@Component({
    selector: 'app-container-invoice',
    templateUrl: './container-invoice.component.html',
    styleUrls: ['../../../screen-modal.less'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ContainerInvoiceComponent extends AppComponentBase implements OnInit {
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
    selectedRow: ProdContainerInvoiceDto = new ProdContainerInvoiceDto();
    saveSelectedRow: ProdContainerInvoiceDto = new ProdContainerInvoiceDto();
    datas: ProdContainerInvoiceDto = new ProdContainerInvoiceDto();
    dataParams: GridParams | undefined;
    dataParamsColor: GridParams | undefined;
    rowData: any[] = [];
    rowDataColor: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    isLoading: boolean = false;

    containerNo: string = '';
    billOfLadingNo: string = '';
    invoiceNo: string = '';
    supplierNo: string = '';
    sealNo: string = '';
    billDateFrom: any;
    billDateTo: any;
    status: string = '';
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
        private _service: ProdContainerInvoiceServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService
    ) {
        super(injector);

        this.colDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 60 },
            { headerName: this.l('Container No'), headerTooltip: this.l('Container No'), field: 'containerNo' },
            { headerName: this.l('Invoice No'), headerTooltip: this.l('Invoice No'), field: 'invoiceNo', },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo' },
            { headerName: this.l('Seal No'), headerTooltip: this.l('Seal No'), field: 'sealNo' },
            { headerName: this.l('Bill Of Lading No'), headerTooltip: this.l('Bill Of Lading No'), field: 'billOfLadingNo' },
            {
                headerName: this.l('Bill Date'), headerTooltip: this.l('Bill Date'), field: 'billDate', flex: 1,
                valueFormatter: (params) => this.pipe.transform(params.data?.billDate, 'dd/MM/yyyy')
            },
            { headerName: this.l('Container Size'), headerTooltip: this.l('Container Size'), field: 'containerSize', flex: 1 },
            {
                headerName: this.l('Plan Devanning Date'), headerTooltip: this.l('Plan Devanning Date'), field: 'planDevanningDate', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.plandedvanningDate, 'dd/MM/yyyy')
            },
            {
                headerName: this.l('Actual Devanning Date'), headerTooltip: this.l('Actual Devanning Date'), field: 'actualDevanningDate', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.actualvanningDate, 'dd/MM/yyyy')
            },
            { headerName: this.l('Status'), headerTooltip: this.l('Status'), flex: 1, field: 'status' },
            {
                headerName: this.l('Freight'), headerTooltip: this.l('Freight'), field: 'freight', flex: 1,
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.freight, 4),
                // aggFunc: this.SumA
            },
            {
                headerName: this.l('Insurance'), headerTooltip: this.l('Insurance'), field: 'insurance', flex: 1,
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.insurance, 4),
                // aggFunc: this.SumA
            },
            {
                headerName: this.l('Tax'), headerTooltip: this.l('Tax'), field: 'tax', flex: 1,
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.tax, 4),
                // aggFunc: this.SumA
            },
            {
                headerName: this.l('Amount'), headerTooltip: this.l('Amount'), field: 'amount', flex: 1,
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.amount, 4),
                // aggFunc: this.SumA
            },
            {
                headerName: this.l('Tax VN'), headerTooltip: this.l('Tax VN'), field: 'taxVnd', flex: 1,
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.taxVnd, 0, false, true),
                // aggFunc: this.SumA
            },
            {
                headerName: this.l('Vat VN'), headerTooltip: this.l('Vat VN'), field: 'vatVnd', flex: 1,
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.vatVnd, 0, false, true),
                // aggFunc: this.SumA
            },
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
        this._service.getProdContainerInvoiceSearch(
            this.billOfLadingNo,
            this.containerNo,
            this.invoiceNo,
            this.sealNo,
            this.status,
            this.supplierNo,
            this.billDateFrom ? moment(this.billDateFrom) : undefined,
            this.billDateTo ? moment(this.billDateTo) : undefined,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.resetGridView();
                this.isLoading = false;
            });
    }

    clearTextSearch() {
        this.billOfLadingNo = '';
        this.containerNo = '';
        this.invoiceNo = '';
        this.sealNo = '';
        this.status = '';
        this.supplierNo = '';
        this.billDateFrom = '';
        this.billDateTo = '';
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getProdContainerInvoiceSearch(
            this.billOfLadingNo,
            this.containerNo,
            this.invoiceNo,
            this.sealNo,
            this.status,
            this.supplierNo,
            this.billDateFrom ? moment(this.billDateFrom) : undefined,
            this.billDateTo ? moment(this.billDateTo) : undefined,
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

    onChangeRowSelection(params: { api: { getSelectedRows: () => ProdContainerInvoiceDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new ProdContainerInvoiceDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);

        this._selectrow = this.saveSelectedRow.id;
    }

    exportToExcel(): void {
        this.isLoading = true;
        this._service.getProdContainerInvoiceToExcel(
            this.billOfLadingNo,
            this.containerNo,
            this.invoiceNo,
            this.sealNo,
            this.status,
            this.supplierNo,
            this.billDateFrom ? moment(this.billDateFrom) : undefined,
            this.billDateTo ? moment(this.billDateTo) : undefined)
            .pipe(finalize(() => this.isLoading = false))
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            });
    }
}

