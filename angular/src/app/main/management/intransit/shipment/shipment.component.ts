import { GridApi } from '@ag-grid-enterprise/all-modules';
import { DatePipe } from '@angular/common';
import { Component, Injector, OnInit, ViewEncapsulation } from '@angular/core';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ProdShipmentDto, ProdShipmentServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ceil } from 'lodash';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'app-shipment',
    templateUrl: './shipment.component.html',
    styleUrls: ['../../../screen-modal.less'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ShipmentComponent extends AppComponentBase implements OnInit {
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
    selectedRow: ProdShipmentDto = new ProdShipmentDto();
    saveSelectedRow: ProdShipmentDto = new ProdShipmentDto();
    datas: ProdShipmentDto = new ProdShipmentDto();
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

    shipmentNo: string = '';
    shippingcompanyCode: string = '';
    supplierNo: string = '';
    fromPort: string = '';
    toPort: string = '';
    shipmentDate: string = '';
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
        private _service: ProdShipmentServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);

        this.colDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 60 },
            { headerName: this.l('Shipment No'), headerTooltip: this.l('Shipment No'), field: 'shipmentNo', flex: 1 },
            { headerName: this.l('Shipping Company Code'), headerTooltip: this.l('Shippingcompany Code'), field: 'shippingcompanyCode', flex: 1 },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', flex: 1 },
            { headerName: this.l('Buyer'), headerTooltip: this.l('Buyer'), field: 'buyer', flex: 1 },
            { headerName: this.l('From Port'), headerTooltip: this.l('From Port'), field: 'fromPort', flex: 1 },
            { headerName: this.l('To Port'), headerTooltip: this.l('To Port'), field: 'toPort', flex: 1 },
            { headerName: this.l('Shipment Date'), headerTooltip: this.l('Shipment Date'), field: 'shipmentDate', flex: 1 },
            {
                headerName: this.l('ETD'), headerTooltip: this.l('Etd'), field: 'etd', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.etd, 'dd/MM/yyyy')
            },
            {
                headerName: this.l('ETA'), headerTooltip: this.l('Eta'), field: 'eta', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.eta, 'dd/MM/yyyy')
            },
            {
                headerName: this.l('ATA'), headerTooltip: this.l('Ata'), field: 'ata', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.ata, 'dd/MM/yyyy')
            },
            { headerName: this.l('Oceanvessel Name'), headerTooltip: this.l('Oceanvessel Name'), field: 'oceanvesselName', flex: 1 },
            {
                headerName: this.l('ATD'), headerTooltip: this.l('Atd'), field: 'atd', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.atd, 'dd/MM/yyyy')
            },
            { headerName: this.l('Status'), headerTooltip: this.l('Status'), field: 'status', flex: 1 }
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
        this._service.getProdShipmentSearch(
            this.shipmentNo,
            this.shippingcompanyCode,
            this.supplierNo,
            this.fromPort,
            this.toPort,
            this.shipmentDate,
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
        this.shipmentNo = '';
        this.shippingcompanyCode = '';
        this.supplierNo = '';
        this.fromPort = '';
        this.toPort = '';
        this.shipmentDate = '';
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getProdShipmentSearch(
            this.shipmentNo,
            this.shippingcompanyCode,
            this.supplierNo,
            this.fromPort,
            this.toPort,
            this.shipmentDate,
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

    onChangeRowSelection(params: { api: { getSelectedRows: () => ProdShipmentDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new ProdShipmentDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);

        this._selectrow = this.saveSelectedRow.id;
    }

    exportToExcel(): void {
        this.isLoading = true;
        this._service.getProdShipmentToExcel(
            this.shipmentNo,
            this.shippingcompanyCode,
            this.supplierNo,
            this.fromPort,
            this.toPort,
            this.shipmentDate)
            .pipe(finalize(() => this.isLoading = false))
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            });
    }
}
