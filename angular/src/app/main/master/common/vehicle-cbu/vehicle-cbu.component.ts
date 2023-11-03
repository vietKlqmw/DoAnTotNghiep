import { GridApi } from '@ag-grid-enterprise/all-modules';
import { DatePipe } from '@angular/common';
import { AfterViewInit, Component, Injector, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MasterVehicleCBUDto, MasterVehicleCBUServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ceil } from 'lodash';
import { LazyLoadEvent, Paginator, Table } from 'primeng';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'app-vehicle-cbu',
    templateUrl: './vehicle-cbu.component.html',
    styleUrls: ['../../../screen-modal.less'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class VehicleCbuComponent extends AppComponentBase implements OnInit, AfterViewInit {
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    defaultColDefs: CustomColDef[] = [];
    vehicleCBUColDefs: any;
    vehicleCBUColorColDefs: any;
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    paginationParamsColor: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 1000000000,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: MasterVehicleCBUDto = new MasterVehicleCBUDto();
    datas: MasterVehicleCBUDto = new MasterVehicleCBUDto();
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
    vehicleType: string = '';
    model: string = '';

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
        private _service: MasterVehicleCBUServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);

        this.vehicleCBUColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 100},
            { headerName: this.l('Vehicle Type'), headerTooltip: this.l('Vehicle Type'), field: 'vehicleType', flex: 1},
            { headerName: this.l('Model'), headerTooltip: this.l('Model'), field: 'model', flex: 1, cellClass: ['text-left']},
            { headerName: this.l('Marketing Code'), headerTooltip: this.l('Marketing Code'), field: 'marketingCode', flex: 1},
            { headerName: this.l('Production Code'), headerTooltip: this.l('Production Code'), field: 'productionCode', flex: 1}
        ];

        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    ngAfterViewInit(): void {
        //this.primengTableHelper.adjustScroll(this.dataTable);
    }

    // searchDatas(event?: LazyLoadEvent) {
    //     if (this.primengTableHelper.shouldResetPaging(event)) {
    //         this.paginator.changePage(0);
    //         return;
    //     }

    //     this.primengTableHelper.showLoadingIndicator();

    //     this._service.getVehicleCBUSearch(
    //         this.vehicleType,
    //         this.model,
    //         this.primengTableHelper.getSorting(this.dataTable),
    //         this.primengTableHelper.getSkipCount(this.paginator, event),
    //         this.primengTableHelper.getMaxResultCount(this.paginator, event)
    //     )
    //         .pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator()))
    //         .subscribe(result => {
    //             this.primengTableHelper.totalRecordsCount = result.totalCount;
    //             this.primengTableHelper.records = result.items;
    //             this.primengTableHelper.hideLoadingIndicator();
    //         });
    // }

    searchDatas(): void {
        this.isLoading = true;
        this._service.getVehicleCBUSearch(
            this.vehicleType,
            this.model,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                if (result.totalCount == 0) {
                    this.rowDataColor = [];
                    this.paginationParamsColor.totalCount = result.totalCount;
                    this.paginationParamsColor.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                } else {
                    //this.searchDatasColor(result.items[0].id);
                }
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
            });
    }

    clearTextSearch() {
        this.vehicleType = '';
        this.model = '';
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getVehicleCBUSearch(
            this.vehicleType,
            this.model,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MasterVehicleCBUDto[] } }) {
        const selected = params.api.getSelectedRows()[0];
        this.selectedRow = Object.assign({}, selected);
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

    // reloadPage(): void {
    //     this.paginator.changePage(this.paginator.getPage());
    // }

    exportToExcel(): void {
        this.isLoading = true;
        this._service.getVehicleCBUToExcel(
            this.vehicleType,
            this.model)
            .pipe(finalize(() => this.isLoading = false))
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            });
    }
}
