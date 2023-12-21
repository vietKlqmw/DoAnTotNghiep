import { GridApi } from '@ag-grid-enterprise/all-modules';
import { DatePipe } from '@angular/common';
import { Component, Injector, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ProdCustomsDeclareDto, ProdCustomsDeclareServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ceil } from 'lodash';
import { finalize } from 'rxjs/operators';
import * as moment from 'moment';
import { EditCustomsDeclareModalComponent } from './edit-customs-declare-modal.component';
import { DataFormatService } from '@app/shared/common/services/data-format.service';

@Component({
    selector: 'app-customsdeclare',
    templateUrl: './customs-declare.component.html',
    styleUrls: ['../../../screen-modal.less'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class CustomsDeclareComponent extends AppComponentBase implements OnInit {
    @ViewChild('editModal', { static: true }) editModal: EditCustomsDeclareModalComponent;

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
    selectedRow: ProdCustomsDeclareDto = new ProdCustomsDeclareDto();
    saveSelectedRow: ProdCustomsDeclareDto = new ProdCustomsDeclareDto();
    datas: ProdCustomsDeclareDto = new ProdCustomsDeclareDto();
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    isLoading: boolean = false;

    customsDeclareNo: string = '';
    declareDateFrom: any;
    declareDateTo: any;
    billOfLadingNo: string = '';
    invoiceNo: string = '';
    _selectrow;
    notDeleted: boolean = false;
    status: string = '';
    listStatus = [
        { value: '', label: "Status" },
        { value: 'CuS1', label: "NEW" },
        { value: 'CuS2', label: "NOT PAID (REQUESTED)" },
        { value: 'CuS3', label: "PAID" }
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
        private _service: ProdCustomsDeclareServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService
    ) {
        super(injector);

        this.colDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 80 },
            { headerName: this.l('Customs Declare No '), headerTooltip: this.l('Customs Declare No'), field: 'customsDeclareNo', flex: 1 },
            {
                headerName: this.l('Declare Date'), headerTooltip: this.l('Declare Date'), field: 'declareDate', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.declareDate, 'dd/MM/yyyy')
            },
            { headerName: this.l('Status'), headerTooltip: this.l('Status'), field: 'status', flex: 1 },
            {
                headerName: this.l('Tax (VND)'), headerTooltip: this.l('Tax (VND)'), field: 'tax', flex: 1, type: 'rightAligned',
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.tax, 0)
            },
            {
                headerName: this.l('Vat (VND)'), headerTooltip: this.l('Vat (VND)'), field: 'vat', flex: 1, type: 'rightAligned',
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.vat, 0, false, true)
            },
            {
                headerName: this.l('Sum'), headerTooltip: this.l('Sum'), field: 'sumCustomsDeclare', flex: 1, type: 'rightAligned',
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.sumCustomsDeclare, 0),
                sortable: true
            },
            { headerName: this.l('Forwarder'), headerTooltip: this.l('Forwarder'), field: 'forwarder', flex: 1 },
            { headerName: this.l('Invoice No'), headerTooltip: this.l('InvoiceNo'), field: 'invoiceNo', flex: 1 },
            {
                headerName: this.l('Invoice Date'), headerTooltip: this.l('Invoice Date'), field: 'invoiceDate', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.invoiceDate, 'dd/MM/yyyy')
            },
            { headerName: this.l('Bill Of Lading No'), headerTooltip: this.l('Bill Of Lading No'), field: 'billOfLadingNo', flex: 1 },
            {
                headerName: this.l('Bill Date'), headerTooltip: this.l('Bill Date'), field: 'billDate', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.billDate, 'dd/MM/yyyy')
            }
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
        this._service.getProdCustomsDeclareSearch(
            this.customsDeclareNo,
            this.declareDateFrom ? moment(this.declareDateFrom) : undefined,
            this.declareDateTo ? moment(this.declareDateTo) : undefined,
            this.billOfLadingNo,
            this.invoiceNo,
            this.status,
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
        this.customsDeclareNo = '';
        this.declareDateFrom = '';
        this.declareDateTo = '';
        this.billOfLadingNo = '';
        this.invoiceNo = '';
        this.status = '';
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getProdCustomsDeclareSearch(
            this.customsDeclareNo,
            this.declareDateFrom ? moment(this.declareDateFrom) : undefined,
            this.declareDateTo ? moment(this.declareDateTo) : undefined,
            this.billOfLadingNo,
            this.invoiceNo,
            this.status,
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

    onChangeRowSelection(params: { api: { getSelectedRows: () => ProdCustomsDeclareDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new ProdCustomsDeclareDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);

        this._selectrow = this.saveSelectedRow.id;

        if(this._selectrow){
            if(this.saveSelectedRow.status == 'PAID'){
                this.notDeleted = true;
            }else{
                this.notDeleted = false;
            }
        }else{
            this.notDeleted = false;
        }
    }

    exportToExcel(): void {
        this.isLoading = true;
        this._service.getProdCustomsDeclareToExcel(
            this.customsDeclareNo,
            this.declareDateFrom ? moment(this.declareDateFrom) : undefined,
            this.declareDateTo ? moment(this.declareDateTo) : undefined,
            this.billOfLadingNo,
            this.invoiceNo,
            this.status)
            .pipe(finalize(() => this.isLoading = false))
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            });
    }

    editDeclare(e): void {
        if (e == 'Edit') this.editModal.show(e, this.saveSelectedRow);
        else this.editModal.show(e);
    }
}

