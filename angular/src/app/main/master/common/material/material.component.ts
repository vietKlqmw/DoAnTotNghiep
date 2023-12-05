import { GridApi } from '@ag-grid-enterprise/all-modules';
import { DatePipe } from '@angular/common';
import { Component, Injector, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MasterMaterialDto, MasterMaterialServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ceil } from 'lodash';
import { finalize } from 'rxjs/operators';
import { ViewMaterialModalComponent } from './view-material-modal.component';
import { EditMaterialModalComponent } from './edit-material-modal.component';
import { ImportMaterialComponent } from './import-material-modal.component';

@Component({
    selector: 'app-material',
    templateUrl: './material.component.html',
    styleUrls: ['../../../screen-modal.less'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class MaterialComponent extends AppComponentBase implements OnInit {
    @ViewChild('viewMaterialModal', { static: true }) viewMaterialModal: ViewMaterialModalComponent;
    @ViewChild('editMaterialModal', { static: true }) editMaterialModal: EditMaterialModalComponent;
    @ViewChild('importExcelModal', { static: true }) importExcelModal: ImportMaterialComponent;

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
    selectedRow: MasterMaterialDto = new MasterMaterialDto();
    saveSelectedRow: MasterMaterialDto = new MasterMaterialDto();
    datas: MasterMaterialDto = new MasterMaterialDto();
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

    materialCode: string = '';
    materialGroup: string = '';
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
        private _service: MasterMaterialServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);

        this.colDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 100, pinned: 'left' },
            {
                headerName: this.l('Material Type'),
                headerTooltip: this.l('Material Type'),
                field: 'materialType',
                flex: 1,
                pinned: 'left'
            },
            {
                headerName: this.l('Material Code'),
                headerTooltip: this.l('Material Code'),
                field: 'materialCode',
                flex: 1,
                pinned: 'left'
            },
            {
                headerName: this.l('Description'),
                headerTooltip: this.l('Description'),
                field: 'description',
                flex: 1
            },
            {
                headerName: this.l('Material Group'),
                headerTooltip: this.l('Material Group'),
                field: 'materialGroup',
                flex: 1
            },
            {
                headerName: this.l('Base Unit Of Measure'),
                headerTooltip: this.l('Base Unit Of Measure'),
                field: 'baseUnitOfMeasure',
                flex: 1
            },
            {
                headerName: this.l('Storage Location'),
                headerTooltip: this.l('Storage Location'),
                field: 'storageLocation',
                flex: 1
            },
            {
                headerName: this.l('Production Type'),
                headerTooltip: this.l('Production Type'),
                field: 'productionType',
                flex: 1
            },
            {
                headerName: this.l('Standard Price'),
                headerTooltip: this.l('Standard Price'),
                field: 'standardPrice',
                flex: 1
            },
            {
                headerName: this.l('Moving Price'),
                headerTooltip: this.l('Moving Price'),
                field: 'movingPrice',
                flex: 1
            },
            {
                headerName: this.l('Material Origin'),
                headerTooltip: this.l('Material Origin'),
                field: 'materialOrigin',
                cellClass: ['text-center'],
                flex: 1
            },
            {
                headerName: this.l('Effective Date From'),
                headerTooltip: this.l('Effective Date From'),
                field: 'effectiveDateFrom',
                valueGetter: (params) => this.pipe.transform(params.data?.effectiveDateFrom, 'dd/MM/yyyy'),
                flex: 1
            },
            {
                headerName: this.l('Effective Date To'),
                headerTooltip: this.l('Effective Date To'),
                field: 'effectiveDateTo',
                valueGetter: (params) => this.pipe.transform(params.data?.effectiveDateTo, 'dd/MM/yyyy'),
                flex: 1
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
        this._service.getMaterialSearch(
            this.materialCode,
            this.materialGroup,
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
        this.materialCode = '';
        this.materialGroup = '';
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getMaterialSearch(
            this.materialCode,
            this.materialGroup,
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

    onChangeRowSelection(params: { api: { getSelectedRows: () => MasterMaterialDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MasterMaterialDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);

        this._selectrow = this.saveSelectedRow.id;
    }

    exportToExcel(): void {
        this.isLoading = true;
        this._service.getMaterialMasterToExcel(
            this.materialCode,
            this.materialGroup)
            .pipe(finalize(() => this.isLoading = false))
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            });
    }

    deleteMaterial() {
        this.message.confirm(this.l('Bạn có chắc chắn muốn xóa?'), 'Delete Material', (isConfirmed) => {
            if (isConfirmed) {
                this._service.deleteMaterial(this._selectrow).subscribe(() => {
                    this.callBackDataGrid(this.dataParams!);
                    this.notify.success(this.l('SuccessfullyDeleted'));
                },error =>{
                    this.notify.error(this.l('FailedDeleted'));
                });
            }
        });
    }

    viewMaterial(): void {
        this.viewMaterialModal.show(this.saveSelectedRow);
    }

    editMaterial(e): void {
        if (e == 'Edit') this.editMaterialModal.show(e, this.saveSelectedRow);
        else this.editMaterialModal.show(e);
    }

    importFromExcel(){
        this.importExcelModal.show();
    }
}

