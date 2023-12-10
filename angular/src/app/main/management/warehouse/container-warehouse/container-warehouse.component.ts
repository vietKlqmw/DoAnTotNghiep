import { GridApi } from '@ag-grid-enterprise/all-modules';
import { DatePipe } from '@angular/common';
import { Component, Injector, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ProdContainerRentalWHPlanDto, ProdContainerRentalWHPlanServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ceil } from 'lodash';
import { finalize } from 'rxjs/operators';
import * as moment from 'moment';
import { AgDropdownRendererComponent } from '@app/shared/common/grid/ag-dropdown-renderer/ag-dropdown-renderer.component';
import { EditContainerWarehouseComponent } from './edit-container-warehouse-modal.component';
import { ImportContainerWarehouseComponent } from './import-container-warehouse-modal.component';

@Component({
    selector: 'app-container-warehouse',
    templateUrl: './container-warehouse.component.html',
    styleUrls: ['../../../screen-modal.less'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ContainerWarehouseComponent extends AppComponentBase implements OnInit {
    @ViewChild('editContainerWarehouse', { static: true }) editContainerWarehouse: EditContainerWarehouseComponent;
    @ViewChild('importExcelModal', { static: true }) importExcelModal: ImportContainerWarehouseComponent;
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
    selectedRow: ProdContainerRentalWHPlanDto = new ProdContainerRentalWHPlanDto();
    saveSelectedRow: ProdContainerRentalWHPlanDto = new ProdContainerRentalWHPlanDto();
    datas: ProdContainerRentalWHPlanDto = new ProdContainerRentalWHPlanDto();
    dataParams: GridParams | undefined;
    rowData: any[] = [];
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
    requestDateFrom: any;
    requestDateTo: any;
    listStatus = [
        { key: 'R', value: "REQUESTED" },
        { key: 'P', value: "PENDING" },
        { key: 'F', value: "CONFIRM" },
        { key: 'C', value: "CANCEL" }
    ];
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
        private _service: ProdContainerRentalWHPlanServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);

        this.colDefs = [
            {
                headerName: "", headerTooltip: "", field: "checked", width: 30, pinned: true,
                headerClass: ["align-checkbox-header"],
                cellClass: ["check-box-center"],
                checkboxSelection: true,
                headerCheckboxSelection: true,
                headerCheckboxSelectionFilteredOnly: true
            },
            {
                headerName: this.l('Status'), headerTooltip: this.l('Status'), field: 'status', flex: 1, pinned: true,
                cellRenderer: 'agSelectRendererComponent',
                list: this.listStatus,
                cellClass: ['RendererCombobox', 'text-center']
            },
            { headerName: this.l('Container No'), headerTooltip: this.l('Container No'), field: 'containerNo', flex: 1, pinned: true },
            {
                headerName: this.l('Request Date'), headerTooltip: this.l('Request Date'), field: 'requestDate', flex: 1, pinned: true,
                valueGetter: (params) => this.pipe.transform(params.data?.requestDate, 'dd/MM/yyyy')
            },
            { headerName: this.l('Invoice No'), headerTooltip: this.l('Invoice No'), field: 'invoiceNo', flex: 1 },
            { headerName: this.l('Bill Of Lading No'), headerTooltip: this.l('Bill Of Lading No'), field: 'billofladingNo', flex: 1 },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', flex: 1 },
            {
                headerName: this.l('Devanning Date'), headerTooltip: this.l('Devanning Date'), field: 'devanningDate', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.devanningDate, 'dd/MM/yyyy')
            },
            { headerName: this.l('Devanning Time'), headerTooltip: this.l('Devanning Time'), field: 'devanningTime', flex: 1 },
            {
                headerName: this.l('Actual Devanning Date'), headerTooltip: this.l('Actual Devanning Date'), field: 'actualDevanningDate', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.actualDevanningDate, 'dd/MM/yyyy')
            },
            {
                headerName: this.l('Gate In Plan Time'), headerTooltip: this.l('Gate In Plan Time'), field: 'gateInPlanTime', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.gateInPlanTime, 'dd/MM/yyyy HH:mm:ss')
            },
            {
                headerName: this.l('Gate In Actual Date Time'), headerTooltip: this.l('Gate In Actual Date Time'), field: 'gateInActualDateTime', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.gateInActualDateTime, 'dd/MM/yyyy HH:mm:ss')
            },
            { headerName: this.l('Transport'), headerTooltip: this.l('Transport'), field: 'transport', flex: 1 }
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
        this._service.getProdContainerRentalWHPlanSearch(
            this.containerNo,
            this.invoiceNo,
            this.billOfLadingNo,
            this.supplierNo,
            this.sealNo,
            this.requestDateFrom ? moment(this.requestDateFrom) : undefined,
            this.requestDateTo ? moment(this.requestDateTo) : undefined,
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
        this.supplierNo = '';
        this.requestDateFrom = '';
        this.requestDateTo = '';
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getProdContainerRentalWHPlanSearch(
            this.containerNo,
            this.invoiceNo,
            this.billOfLadingNo,
            this.supplierNo,
            this.sealNo,
            this.requestDateFrom ? moment(this.requestDateFrom) : undefined,
            this.requestDateTo ? moment(this.requestDateTo) : undefined,
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

    onChangeRowSelection(params: { api: { getSelectedRows: () => ProdContainerRentalWHPlanDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new ProdContainerRentalWHPlanDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);

        this._selectrow = this.saveSelectedRow.id;
    }

    exportToExcel(): void {
        this.isLoading = true;
        this._service.getProdContainerRentalWHPlanToExcel(
            this.containerNo,
            this.invoiceNo,
            this.billOfLadingNo,
            this.supplierNo,
            this.sealNo,
            this.requestDateFrom ? moment(this.requestDateFrom) : undefined,
            this.requestDateTo ? moment(this.requestDateTo) : undefined)
            .pipe(finalize(() => this.isLoading = false))
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            });
    }

    deleteContWH() {
        this.message.confirm(this.l('Bạn có chắc chắn muốn xóa?'), 'Delete Container At Warehouse', (isConfirmed) => {
            if (isConfirmed) {
                this._service.deleteContWH(this._selectrow).subscribe(() => {
                    this.callBackDataGrid(this.dataParams!);
                    this.notify.success(this.l('SuccessfullyDeleted'));
                },error =>{
                    this.notify.error(this.l('FailedDeleted'));
                });
            }
        });
    }

    edit(e): void {
        if (e == 'Edit') this.editContainerWarehouse.show(e, this.saveSelectedRow);
        else this.editContainerWarehouse.show(e);
    }

    importFromExcel(){
        this.importExcelModal.show();
    }

    rowClickData: ProdContainerRentalWHPlanDto;
    onRowClick(params) {

        let _rows = document.querySelectorAll<HTMLElement>("body .ag-theme-alpine .ag-center-cols-container .ag-row.ag-row-level-0.ag-row-position-absolute");
        for (let i = 0; _rows[i]; i++) { _rows[i].classList.remove("setcolor_background_rowclick"); }

        if (this.rowClickData && this.rowClickData.id == params.data.id) this.rowClickData = undefined;
        else {
            this.rowClickData = params.data;
            let _row = document.querySelector<HTMLElement>("body .ag-theme-alpine .ag-center-cols-container div[row-id='" + params.node.rowIndex + "'].ag-row.ag-row-level-0.ag-row-position-absolute");
            if (_row) _row.classList.add("setcolor_background_rowclick");
        }

    }
}

