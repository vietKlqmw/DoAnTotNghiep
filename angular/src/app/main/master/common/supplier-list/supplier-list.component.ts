import { GridApi } from '@ag-grid-enterprise/all-modules';
import { DatePipe } from '@angular/common';
import { Component, Injector, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MasterSupplierListDto, MasterSupplierListServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ceil } from 'lodash';
import { finalize } from 'rxjs/operators';
import { EditSupplierModalComponent } from './edit-supplier-modal.component';

@Component({
    selector: 'app-supplier-list',
    templateUrl: './supplier-list.component.html',
    styleUrls: ['../../../screen-modal.less'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class SupplierListComponent extends AppComponentBase implements OnInit {
    @ViewChild('editModal', { static: true }) editModal: EditSupplierModalComponent;
    
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
    saveSelectedRow: MasterSupplierListDto = new MasterSupplierListDto();
    selectedRow: MasterSupplierListDto = new MasterSupplierListDto();
    datas: MasterSupplierListDto = new MasterSupplierListDto();
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

    supplierNo: string = '';
    supplierName: string = '';

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
        private _service: MasterSupplierListServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);

        this.colDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 100 },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', flex: 1 },
            { headerName: this.l('Supplier Name'), headerTooltip: this.l('Supplier Name'), field: 'supplierName', flex: 1 },
            { headerName: this.l('Remarks'), headerTooltip: this.l('Remarks'), field: 'remarks', flex: 1 },
            { headerName: this.l('Supplier Type'), headerTooltip: this.l('Supplier Type'), field: 'supplierType', flex: 1 },
            { headerName: this.l('Supplier Name Vn'), headerTooltip: this.l('Supplier Name Vn'), field: 'supplierNameVn', flex: 1 },
            { headerName: this.l('Exporter'), headerTooltip: this.l('Exporter'), field: 'exporter', flex: 1 }
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
        this._service.getSupplierListSearch(
            this.supplierNo,
            this.supplierName,
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
        this.supplierNo = '';
        this.supplierName = '';
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getSupplierListSearch(
            this.supplierNo,
            this.supplierName,
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

    onChangeRowSelection(params: { api: { getSelectedRows: () => MasterSupplierListDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MasterSupplierListDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
    }

    deleteSupplier() {
        this.message.confirm(this.l('Are you sure Delete?'), 'Delete Supplier', (isConfirmed) => {
            if (isConfirmed) {
                this._service.deleteSupplier(this.saveSelectedRow.id).subscribe(() => {
                    this.callBackDataGrid(this.dataParams!);
                    this.notify.success(this.l('SuccessfullyDeleted'));
                }, error => {
                    this.notify.error(this.l('FailedDeleted'));
                });
            }
        });
    }

    edits(ev) {
        this.editModal.show(ev, this.saveSelectedRow);
    }
}

