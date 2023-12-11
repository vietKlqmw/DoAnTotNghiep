import { GridApi } from '@ag-grid-enterprise/all-modules';
import { DatePipe } from '@angular/common';
import { Component, Injector, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ProdContainerIntransitDto, ProdContainerIntransitServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ceil } from 'lodash';
import { finalize } from 'rxjs/operators';
import * as moment from 'moment';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { EditContainerIntransitModalComponent } from './edit-container-intransit-modal.component';

@Component({
    selector: 'app-container-intransit',
    templateUrl: './container-intransit.component.html',
    styleUrls: ['../../../screen-modal.less'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ContainerIntransitComponent extends AppComponentBase implements OnInit {
    @ViewChild('editModal', { static: true }) editModal: EditContainerIntransitModalComponent;

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
    selectedRow: ProdContainerIntransitDto = new ProdContainerIntransitDto();
    saveSelectedRow: ProdContainerIntransitDto = new ProdContainerIntransitDto();
    datas: ProdContainerIntransitDto = new ProdContainerIntransitDto();
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
    shippingDate: any;
    portDate: any;
    transactionDate: any;
    _selectrow;
    notDelete: boolean = false;

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
        private _service: ProdContainerIntransitServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService
    ) {
        super(injector);

        this.colDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 80 },
            { headerName: this.l('Container No'), headerTooltip: this.l('Container No'), field: 'containerNo', flex: 1 },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', flex: 1 },
            {
                headerName: this.l('Shipping Date'), headerTooltip: this.l('ShippingDate'), field: 'shippingDate', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.shippingDate, 'dd/MM/yyyy')
            },
            {
                headerName: this.l('Port Date'), headerTooltip: this.l('PortDate'), field: 'portDate', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.portDate, 'dd/MM/yyyy')
            },
            // {
            //     headerName: this.l('Transaction Date'), headerTooltip: this.l('TransactionDate'), field: 'transactionDate', flex: 1,
            //     valueGetter: (params) => this.pipe.transform(params.data?.transactionDate, 'dd/MM/yyyy')
            // },
            {
                headerName: this.l('Qty'), headerTooltip: this.l('Qty'), field: 'usageQty', flex: 1, type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.usageQty)
            },
            { headerName: this.l('Status'), headerTooltip: this.l('Status'), field: 'status', flex: 1 },
            { headerName: this.l('Shipment Id'), headerTooltip: this.l('Shipment Id'), field: 'shipmentId', flex: 1 },
            { headerName: this.l('Part List Id'), headerTooltip: this.l('Part List Id'), field: 'partListId', flex: 1 }
        ];

        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit() {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    searchDatas(): void {
        this.isLoading = true;
        this._service.getProdContainerIntransitSearch(
            this.containerNo,
            this.shippingDate ? moment(this.shippingDate) : undefined,
            this.portDate ? moment(this.portDate) : undefined,
            this.transactionDate ? moment(this.transactionDate) : undefined,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
            });
    }

    clearTextSearch() {
        this.containerNo = '';
        this.shippingDate = '';
        this.portDate = '';
        this.transactionDate = '';
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getProdContainerIntransitSearch(
            this.containerNo,
            this.shippingDate ? moment(this.shippingDate) : undefined,
            this.portDate ? moment(this.portDate) : undefined,
            this.transactionDate ? moment(this.transactionDate) : undefined,
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
                this.isLoading = false;
            });
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => ProdContainerIntransitDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new ProdContainerIntransitDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);

        this._selectrow = this.saveSelectedRow.id;

        if(this._selectrow){
            if(this.saveSelectedRow.status != 'NEW') this.notDelete = true;
            else this.notDelete = false;
        }else{
            this.notDelete = false;
        }
    }

    exportToExcel(): void {
        this.isLoading = true;
        this._service.getProdContainerIntransitToExcel(
            this.containerNo,
            this.shippingDate ? moment(this.shippingDate) : undefined,
            this.portDate ? moment(this.portDate) : undefined,
            this.transactionDate ? moment(this.transactionDate) : undefined)
            .pipe(finalize(() => this.isLoading = false))
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            });
    }

    deleteContIntransit() {
        this.message.confirm(this.l('Bạn có chắc chắn muốn xóa?'), 'Delete Shipment', (isConfirmed) => {
            if (isConfirmed) {
                this._service.deleteContainerIntransit(this._selectrow).subscribe(() => {
                    this.callBackDataGrid(this.dataParams!);
                    this.notify.success(this.l('SuccessfullyDeleted'));
                }, error => {
                    this.notify.error(this.l('FailedDeleted'));
                });
            }
        });
    }

    editContIntransit(e): void {
        if (e == 'Edit') this.editModal.show(e, this.saveSelectedRow);
        else this.editModal.show(e);
    }
}

