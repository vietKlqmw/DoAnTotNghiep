import { GridApi } from '@ag-grid-enterprise/all-modules';
import { DatePipe } from '@angular/common';
import { Component, Injector, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ProdOrderPartDto, ProdOrderPartServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ceil } from 'lodash';
import { finalize } from 'rxjs/operators';
import * as moment from 'moment';
import { DataFormatService } from '@app/shared/common/services/data-format.service';

@Component({
    selector: 'app-orderpart',
    templateUrl: './order-part.component.html',
    styleUrls: ['../../../screen-modal.less'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class OrderPartComponent extends AppComponentBase implements OnInit {
    // @ViewChild('editModal', { static: true }) editModal: EditOrderPartModalComponent;

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
    selectedRow: ProdOrderPartDto = new ProdOrderPartDto();
    saveSelectedRow: ProdOrderPartDto = new ProdOrderPartDto();
    datas: ProdOrderPartDto = new ProdOrderPartDto();
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    isLoading: boolean = false;

    partNo: string = '';
    supplierNo: string = '';
    cfc: string = '';
    status: string = '';
    containerNo: string = '';
    shipmentNo: string = '';
    orderDateFrom: any;
    orderDateTo: any;
    _selectrow;
    notDeleted: boolean = false;
    listStatus = [
        { label: 'Status', value: '' },
        { label: 'PENDING', value: 'PENDING' },
        { label: 'ORDERED', value: 'ORDERED' }
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
        private _service: ProdOrderPartServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService
    ) {
        super(injector);

        this.colDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 80 },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', flex: 1 },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName', flex: 1 },
            { headerName: this.l('Cfc'), headerTooltip: this.l('Cfc'), field: 'cfc', flex: 1 },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', flex: 1 },
            {
                headerName: this.l('Order Date'), headerTooltip: this.l('Order Date'), field: 'orderDate', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.orderDate, 'dd/MM/yyyy')
            },
            {
                headerName: this.l('Qty'), headerTooltip: this.l('Qty'), field: 'qty', flex: 1, type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.qty)
            },
            {
                headerName: this.l('Amount Unit'), headerTooltip: this.l('Amount Unit'), field: 'amountUnit', flex: 1, type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.amountUnit)
            },
            {
                headerName: this.l('Total Amount'), headerTooltip: this.l('Total Amount'), field: 'totalAmount', flex: 1, type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.totalAmount)
            },
            { headerName: this.l('Status'), headerTooltip: this.l('Status'), field: 'status', flex: 1 },
            { headerName: this.l('Container No'), headerTooltip: this.l('Container No'), field: 'containerNo', flex: 1 },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', flex: 1 },
            { headerName: this.l('Remark'), headerTooltip: this.l('Remark'), field: 'remark', flex: 1 }
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
        this._service.getProdOrderPartSearch(
            this.partNo,
            this.supplierNo,
            this.cfc,
            this.status,
            this.containerNo,
            this.shipmentNo,
            this.orderDateFrom ? moment(this.orderDateFrom) : undefined,
            this.orderDateTo ? moment(this.orderDateTo) : undefined,
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
        this.partNo = '';
        this.supplierNo = '';
        this.cfc = '';
        this.status = '';
        this.containerNo = '';
        this.shipmentNo = '';
        this.orderDateFrom = '';
        this.orderDateTo = '';
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getProdOrderPartSearch(
            this.partNo,
            this.supplierNo,
            this.cfc,
            this.status,
            this.containerNo,
            this.shipmentNo,
            this.orderDateFrom ? moment(this.orderDateFrom) : undefined,
            this.orderDateTo ? moment(this.orderDateTo) : undefined,
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

    onChangeRowSelection(params: { api: { getSelectedRows: () => ProdOrderPartDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new ProdOrderPartDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);

        this._selectrow = this.saveSelectedRow.id;
    }

    exportToExcel(): void {
        this.isLoading = true;
        this._service.getProdOrderPartToExcel(
            this.partNo,
            this.supplierNo,
            this.cfc,
            this.status,
            this.containerNo,
            this.shipmentNo,
            this.orderDateFrom ? moment(this.orderDateFrom) : undefined,
            this.orderDateTo ? moment(this.orderDateTo) : undefined)
            .pipe(finalize(() => this.isLoading = false))
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            });
    }

    // deleteBill() {
    //     this.message.confirm(this.l('Are you sure Delete?'), 'Delete Bill Of Lading', (isConfirmed) => {
    //         if (isConfirmed) {
    //             this._service.deleteOrderPart(this._selectrow).subscribe(() => {
    //                 this.callBackDataGrid(this.dataParams!);
    //                 this.notify.success(this.l('SuccessfullyDeleted'));
    //             }, error => {
    //                 this.notify.error(this.l('FailedDeleted'));
    //             });
    //         }
    //     });
    // }

    // editBill(): void {
    //     this.editModal.show(this.saveSelectedRow);
    // }
}

