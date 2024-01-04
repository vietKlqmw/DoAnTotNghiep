import { GridApi } from '@ag-grid-enterprise/all-modules';
import { DatePipe } from '@angular/common';
import { Component, Injector, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MasterStorageLocationDto, MasterStorageLocationServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ceil } from 'lodash';
import { finalize } from 'rxjs/operators';
import { EditWarehouseModalComponent } from './edit-storage-location-modal.component';

@Component({
    selector: 'app-storage-location',
    templateUrl: './storage-location.component.html',
    styleUrls: ['../../../screen-modal.less'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class StorageLocationComponent extends AppComponentBase implements OnInit {
    @ViewChild('editModal', { static: true }) editModal: EditWarehouseModalComponent;
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
    saveSelectedRow: MasterStorageLocationDto = new MasterStorageLocationDto();
    selectedRow: MasterStorageLocationDto = new MasterStorageLocationDto();
    datas: MasterStorageLocationDto = new MasterStorageLocationDto();
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

    address: string = '';
    status: string = '';
    listStatus = [
        { label: 'Status', value: '' },
        { label: 'Normal', value: 'Normal' },
        { label: 'Good', value: 'Good' },
        { label: 'Medium', value: 'Medium' },
        { label: 'High', value: 'High' },
        { label: 'Full', value: 'Full' }
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
        private _service: MasterStorageLocationServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService
    ) {
        super(injector);

        this.colDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 80 },
            { headerName: this.l('Warehouse'), headerTooltip: this.l('Storage Location'), field: 'storageLocation', width: 130 },
            //{ headerName: this.l('Storage Location Name'), headerTooltip: this.l('Storage Location Name'), field: 'storageLocationName', width: 180 },
            //{ headerName: this.l('Address Language En'), headerTooltip: this.l('Address Language En'), field: 'addressLanguageEn', flex: 1 },
            { headerName: this.l('Address'), headerTooltip: this.l('Address Language Vn'), field: 'addressLanguageVn', flex: 1 },
            { headerName: this.l('Category'), headerTooltip: this.l('Category'), field: 'category', width: 130 },
            {
                headerName: this.l('Max Stock'), headerTooltip: this.l('Max Stock'), field: 'maxStock', width: 110, type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.maxStock)
            },
            {
                headerName: this.l('Inventory'), headerTooltip: this.l('Inventory'), field: 'inventory', width: 110, type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.inventory)
            },
            {
                headerName: this.l('Status'), headerTooltip: this.l('Status'), field: 'status', width: 150,
                cellStyle: function (params: any) {
                    if (params.data.status === 'Normal') {
                        return {
                            'background-color': 'Green',
                            'color': 'white',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                        };
                    }
                    else if (params.data.status === 'Good') {
                        return {
                            'background-color': 'GreenYellow',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                        };
                    }
                    else if (params.data.status === 'Medium') {
                        return {
                            'background-color': 'Yellow',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                        };
                    }
                    else if (params.data.status === 'High') {
                        return {
                            'background-color': 'Orange',
                            'color': 'white',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                        };
                    }
                    else if (params.data.status === 'Full') {
                        return {
                            'background-color': 'Red',
                            'color': 'white',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                        };
                    }
                    else if (params.data.status === 'Empty') {
                        return {
                            'background-color': 'Whitesmoke',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                        };
                    }
                }
            },
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
        this._service.getStorageLocationSearch(
            this.address,
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
                this.isLoading = false;
            });
    }

    clearTextSearch() {
        this.address = '';
        this.status = '';
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getStorageLocationSearch(
            this.address,
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

    onChangeRowSelection(params: { api: { getSelectedRows: () => MasterStorageLocationDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MasterStorageLocationDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
    }

    exportToExcel(): void {
        this.isLoading = true;
        this._service.getStorageLocationToExcel(
            this.address,
            this.status)
            .pipe(finalize(() => this.isLoading = false))
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            });
    }

    deleteWH() {
        this.message.confirm(this.l('Are you sure Delete?'), 'Delete Warehouse', (isConfirmed) => {
            if (isConfirmed) {
                this._service.deleteWH(this.saveSelectedRow.id).subscribe(() => {
                    this.callBackDataGrid(this.dataParams!);
                    this.notify.success(this.l('SuccessfullyDeleted'));
                }, error => {
                    this.notify.error(this.l('FailedDeleted'));
                });
            }
        });
    }

    editwh(ev){
        this.editModal.show(ev, this.saveSelectedRow);
    }
}

