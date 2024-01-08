import { GridApi } from '@ag-grid-enterprise/all-modules';
import { DatePipe } from '@angular/common';
import { Component, Injector, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MasterPartListDto, MasterPartListServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ceil } from 'lodash';
import { finalize } from 'rxjs/operators';
import { ViewMaterialComponent } from '../../other/view-material/view-material.component';
import { EditPartListModalComponent } from './edit-part-list-modal.component';
import { ImportPartListComponent } from './import-part-list-modal.component';
import { DataFormatService } from '@app/shared/common/services/data-format.service';

@Component({
    selector: 'app-part-list',
    templateUrl: './part-list.component.html',
    styleUrls: ['../../../screen-modal.less'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class PartListComponent extends AppComponentBase implements OnInit {
    @ViewChild('viewMaterial', { static: true }) viewMaterial: ViewMaterialComponent;
    @ViewChild('editPartListModal', { static: true }) editPartListModal: EditPartListModalComponent;
    @ViewChild('importExcelModal', { static: true }) importExcelModal: ImportPartListComponent;

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
    selectedRow: MasterPartListDto = new MasterPartListDto();
    saveSelectedRow: MasterPartListDto = new MasterPartListDto();
    datas: MasterPartListDto = new MasterPartListDto();
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
        private _service: MasterPartListServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService
    ) {
        super(injector);

        this.colDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 60 },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', flex: 1 },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName', flex: 1 },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', flex: 1 },
            { headerName: this.l('Carfamily Code'), headerTooltip: this.l('Carfamily Code'), field: 'carfamilyCode', flex: 1 },
            { headerName: this.l('Base Unit Of Measure'), headerTooltip: this.l('Base Unit Of Measure'), field: 'baseUnitOfMeasure', flex: 1 },
            {
                headerName: this.l('Standard Price (VND)'), headerTooltip: this.l('Standard Price'), field: 'standardPrice', flex: 1, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.standardPrice != null ? this._fm.formatMoney_decimal(params.data?.standardPrice) : 0)
            },
            {
                headerName: this.l('Moving Price (VND)'), headerTooltip: this.l('Moving Price'), field: 'movingPrice', flex: 1, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.movingPrice != null ? this._fm.formatMoney_decimal(params.data?.movingPrice) : 0)
            },
            {
                headerName: this.l('Start Production Month'), headerTooltip: this.l('Start Production Month'), field: 'startProductionMonth', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.startProductionMonth, 'dd/MM/yyyy')
            },
            {
                headerName: this.l('End Production Month'), headerTooltip: this.l('End Production Month'), field: 'endProductionMonth', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.endProductionMonth, 'dd/MM/yyyy')
            },
            { headerName: this.l('Remark'), headerTooltip: this.l('Remark'), field: 'remark', flex: 1 }
            // { headerName: this.l('Material Id'), headerTooltip: this.l('Material Id'), field: 'materialId', flex: 1 }
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
        this._service.getPartListSearch(
            this.partNo,
            this.supplierNo,
            this.cfc,
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
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getPartListSearch(
            this.partNo,
            this.supplierNo,
            this.cfc,
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

    onChangeRowSelection(params: { api: { getSelectedRows: () => MasterPartListDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MasterPartListDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);

        this._selectrow = this.saveSelectedRow.id;
    }

    exportToExcel(): void {
        this.isLoading = true;
        this._service.getPartListToExcel(
            this.partNo,
            this.supplierNo,
            this.cfc)
            .pipe(finalize(() => this.isLoading = false))
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            });
    }

    deletePartList() {
        this.message.confirm(this.l('Bạn có chắc chắn muốn xóa?'), 'Delete PartList', (isConfirmed) => {
            if (isConfirmed) {
                this._service.deletePartList(this._selectrow).subscribe(() => {
                    this.callBackDataGrid(this.dataParams!);
                    this.notify.success(this.l('SuccessfullyDeleted'));
                }, error => {
                    this.notify.error(this.l('FailedDeleted'));
                });
            }
        });
    }

    viewMaterialById(): void {
        this.viewMaterial.show(this.saveSelectedRow.materialId);
    }

    editPartList(e): void {
        if (e == 'Edit') this.editPartListModal.show(e, this.saveSelectedRow);
        else this.editPartListModal.show(e);
    }

    importFromExcel() {
        this.importExcelModal.show();
    }
}

