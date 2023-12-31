import { GridApi } from '@ag-grid-enterprise/all-modules';
import { DatePipe } from '@angular/common';
import { Component, Injector, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ProdStockReceivingDto, ProdStockReceivingServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ceil } from 'lodash';
import { finalize } from 'rxjs/operators';
import * as moment from 'moment';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { ViewMaterialComponent } from '@app/main/master/other/view-material/view-material.component';
import { AddPurchaseOrderModalComponent } from './order-stock-receiving-modal.component';
import { AddGdnStockModalComponent } from './add-gdn-stock-modal.component';
import { UpdateOrderStockModalComponent } from './update-order-stock-modal.component';

@Component({
    selector: 'app-stock-receiving',
    templateUrl: './stock-receiving.component.html',
    styleUrls: ['../../../screen-modal.less'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class StockReceivingComponent extends AppComponentBase implements OnInit {
    @ViewChild('viewMaterial', { static: true }) viewMaterial: ViewMaterialComponent;
    @ViewChild('addPurchaseOrder', { static: true }) addPurchaseOrder: AddPurchaseOrderModalComponent;
    @ViewChild('addGdnStock', { static: true }) addGdnStock: AddGdnStockModalComponent;
    @ViewChild('editModal', { static: true }) editModal: UpdateOrderStockModalComponent;

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
    selectedRow: ProdStockReceivingDto = new ProdStockReceivingDto();
    saveSelectedRow: ProdStockReceivingDto = new ProdStockReceivingDto();
    datas: ProdStockReceivingDto = new ProdStockReceivingDto();
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
    model: string = '';
    requestDateFrom: any;
    requestDateTo: any;
    warehouse: string = 'A1';
    listPartId: string = '';
    stockStatus: string = '1';
    _selectrow;
    listWarehouse = [
        { label: 'Warehouse A1', value: "A1" },
        { label: 'Warehouse A2', value: "A2" },
        { label: 'Warehouse B1', value: "B1" },
        { label: 'Warehouse C1', value: "C1" },
        { label: 'Warehouse C2', value: "C2" }
    ];
    btnGDN: boolean = false;
    btnPO: boolean = false;
    isDisable: boolean = true;
    isEdit: boolean = true;

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
        private _service: ProdStockReceivingServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService
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
            { headerName: this.l('Warehouse'), headerTooltip: this.l('Warehouse'), field: 'warehouse', flex: 1, pinned: true },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', flex: 1 },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName', flex: 1 },
            { headerName: this.l('Carfamily Code'), headerTooltip: this.l('Cfc'), field: 'model', flex: 1 },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', flex: 1 },
            //{ headerName: this.l('Container No'), headerTooltip: this.l('Container No'), field: 'containerNo', flex: 1 },
            //{ headerName: this.l('Invoice No'), headerTooltip: this.l('Invoice No'), field: 'invoiceNo', flex: 1 },
            {
                headerName: this.l('Qty'), headerTooltip: this.l('Qty'), field: 'qty', flex: 1, type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.qty),
                aggFunc: this.calTotal
            },
            {
                headerName: this.l('Actual Qty'), headerTooltip: this.l('Actual Qty'), field: 'actualQty', flex: 1, type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.actualQty),
                aggFunc: this.calTotal
            },
            {
                headerName: this.l('Ordered Qty'), headerTooltip: this.l('Ordered Qty'), field: 'orderedQty', flex: 1, type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.orderedQty),
                aggFunc: this.calTotal
            },
            {
                headerName: this.l('Remain Qty'), headerTooltip: this.l('Remain Qty'), field: 'remainQty', flex: 1, type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.remainQty),
                aggFunc: this.calTotal
            },
            {
                headerName: this.l('Order Qty'), headerTooltip: this.l('Order Qty'), field: 'orderQty', flex: 1, type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.orderQty),
                aggFunc: this.calTotal
            },
            {
                headerName: this.l('Request Date'), headerTooltip: this.l('Request Date'), field: 'requestDate', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.requestDate, 'dd/MM/yyyy')
            },
            { headerName: this.l('Request Status'), headerTooltip: this.l('Request Status'), field: 'requestStatus', flex: 1 },
            {
                headerName: this.l('Delivery Date'), headerTooltip: this.l('Delivery Date'), field: 'deliveryDate', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.deliveryDate, 'dd/MM/yyyy')
            },
            // {
            //     headerName: this.l('Working Date'), headerTooltip: this.l('Working Date'), field: 'workingDate', flex: 1,
            //     valueGetter: (params) => this.pipe.transform(params.data?.workingDate, 'dd/MM/yyyy')
            // },
            { headerName: this.l('Invoice Delivery'), headerTooltip: this.l('Invoice Delivery'), field: 'invoiceNoOut', flex: 1 }
            //{ headerName: this.l('Material Id'), headerTooltip: this.l('Material Id'), field: 'materialId', flex: 1 }

        ];

        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit() {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.isDisable = true;
        this.isEdit = true;
        this.setvalradio('1');
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
        this._service.getProdStockReceivingSearch(
            this.partNo,
            this.requestDateFrom ? moment(this.requestDateFrom) : undefined,
            this.requestDateTo ? moment(this.requestDateTo) : undefined,
            this.supplierNo,
            this.model,
            this.warehouse,
            this.stockStatus,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                if (result.totalCount > 0) {
                    var _sumQty = 0;
                    var _sumActualQty = 0;
                    var _sumOrderQty = 0;
                    var _sumRemainQty = 0;
                    var _sumOrderedQty = 0;
                    _sumQty = result.items[0].grandQty;
                    _sumActualQty = result.items[0].grandActualQty;
                    _sumOrderQty = result.items[0].grandOrderQty;
                    _sumRemainQty = result.items[0].grandRemainQty;
                    _sumOrderedQty = result.items[0].grandOrderedQty;
                    var rows = this.createRow(1, _sumQty, _sumActualQty, _sumOrderQty, _sumRemainQty, _sumOrderedQty);
                    this.dataParams!.api.setPinnedBottomRowData(rows);
                } else {
                    this.dataParams!.api.setPinnedBottomRowData(null);
                    this.isDisable = true;
                    this.isEdit = true;
                }
                this.resetGridView();
                this.isLoading = false;
            });
    }

    clearTextSearch() {
        this.partNo = '';
        this.warehouse = 'A1';
        this.model = '';
        this.supplierNo = '';
        this.requestDateFrom = '';
        this.requestDateTo = '';
        this.stockStatus = '1';
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getProdStockReceivingSearch(
            this.partNo,
            this.requestDateFrom ? moment(this.requestDateFrom) : undefined,
            this.requestDateTo ? moment(this.requestDateTo) : undefined,
            this.supplierNo,
            this.model,
            this.warehouse,
            this.stockStatus,
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
                if (result.totalCount > 0) {
                    var _sumQty = 0;
                    var _sumActualQty = 0;
                    var _sumOrderQty = 0;
                    var _sumRemainQty = 0;
                    var _sumOrderedQty = 0;
                    _sumQty = result.items[0].grandQty;
                    _sumActualQty = result.items[0].grandActualQty;
                    _sumOrderQty = result.items[0].grandOrderQty;
                    _sumRemainQty = result.items[0].grandRemainQty;
                    _sumOrderedQty = result.items[0].grandOrderedQty;
                    var rows = this.createRow(1, _sumQty, _sumActualQty, _sumOrderQty, _sumRemainQty, _sumOrderedQty);
                    this.dataParams!.api.setPinnedBottomRowData(rows);
                } else {
                    this.dataParams!.api.setPinnedBottomRowData(null);
                }
                this.resetGridView();
                this.isLoading = false;
            });
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => ProdStockReceivingDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new ProdStockReceivingDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);

        this._selectrow = this.saveSelectedRow.id;

        this.listPartId = '';
        if (params.api.getSelectedRows().length) {
            for (var i = 0; i < params.api.getSelectedRows().length; i++) {
                if (i != params.api.getSelectedRows().length - 1) {
                    this.listPartId += params.api.getSelectedRows()[i].id + ',';
                } else {
                    this.listPartId += params.api.getSelectedRows()[i].id;
                }
            }
        }

        if(this.saveSelectedRow.id){
            if(this.saveSelectedRow.orderQty > 0){
                this.isDisable = true;
                this.isEdit = false;
            }else{
                this.isDisable = false;
                this.isEdit = true;
            }
        }else{
            this.isDisable = true;
            this.isEdit = true;
        }
    }

    exportToExcel(): void {
        this.isLoading = true;
        this._service.getProdStockReceivingToExcel(
            this.partNo,
            this.requestDateFrom ? moment(this.requestDateFrom) : undefined,
            this.requestDateTo ? moment(this.requestDateTo) : undefined,
            this.supplierNo,
            this.model,
            this.warehouse,
            this.stockStatus)
            .pipe(finalize(() => this.isLoading = false))
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            });
    }

    createRow(count: number, sumQty: number, sumActualQty: number, sumOrderQty: number, sumRemainQty: number, sumOrderedQty: number): any[] {
        let result: any[] = [];

        for (var i = 0; i < count; i++) {
            result.push({
                warehouse: 'Grand Total',
                qty: sumQty,
                actualQty: sumActualQty,
                orderQty: sumOrderQty,
                remainQty: sumRemainQty,
                orderedQty: sumOrderedQty
            });
        }
        return result;
    }

    calTotal(values) {
        var sum = 0;
        values.forEach(function (value) { sum += Number(value); });
        return sum;
    }


    viewMaterialById(): void {
        this.viewMaterial.show(this.saveSelectedRow.materialId);
    }

    setvalradio(i: string) {
        if(i == '2') this.btnGDN = true;
        else this.btnGDN = false;
        if(i == '1') this.btnPO = true;
        else this.btnPO = false;
        let _btnUncheck = document.querySelector('.actionButton_w' + i + '.active');
        if (_btnUncheck) {
            let objbtn = document.querySelectorAll('.groupBtn');
            for (let i = 0; objbtn[i]; i++) { objbtn[i].classList.remove('active'); }

            this.stockStatus = '';
        }
        else {
            let objbtn = document.querySelectorAll('.groupBtn');
            for (let i = 0; objbtn[i]; i++) { objbtn[i].classList.remove('active'); }

            let _btn = document.querySelector('.actionButton_w' + i);
            if (_btn) _btn.classList.add('active');
            this.stockStatus = i;
        }
        this.searchDatas();
    }

    rowClickData: ProdStockReceivingDto;
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

    addOrder(): void {
        this.addPurchaseOrder.show(this.listPartId);
    }

    addGoodsDeliveryNote(): void {
        this.addGdnStock.show(this.listPartId, this.warehouse);
    }

    editOrderQty(): void {
        this.editModal.show(this.saveSelectedRow)
    }

    onChangeWarehouse(){
        this.searchDatas();
    }

    cancelOrder(){
        this.message.confirm(this.l('Are you sure Cancel Order?'), 'Cancel Order', (isConfirmed) => {
            if (isConfirmed) {
                this._service.deleteOrderWarehouse(this.listPartId).subscribe(() => {
                    this.callBackDataGrid(this.dataParams!);
                    this.notify.success(this.l('SuccessfullyDeleted'));
                }, error => {
                    this.notify.error(this.l('FailedDeleted'));
                });
            }
        });
    }
}

