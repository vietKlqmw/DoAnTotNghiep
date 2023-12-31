import { GridApi } from '@ag-grid-enterprise/all-modules';
import { DatePipe } from '@angular/common';
import { Component, Injector, OnInit, ViewEncapsulation } from '@angular/core';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ProdContainerListDto, ProdContainerListServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ceil } from 'lodash';
import { finalize } from 'rxjs/operators';
import * as moment from 'moment';
import { DataFormatService } from '@app/shared/common/services/data-format.service';

@Component({
    selector: 'app-containerlist',
    templateUrl: './container-list.component.html',
    styleUrls: ['../../../screen-modal.less'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class ContainerListComponent extends AppComponentBase implements OnInit {
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
    selectedRow: ProdContainerListDto = new ProdContainerListDto();
    saveSelectedRow: ProdContainerListDto = new ProdContainerListDto();
    datas: ProdContainerListDto = new ProdContainerListDto();
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    isLoading: boolean = false;

    containerNo: string = '';
    supplierNo: string = '';
    billNo: string = '';
    invoiceNo: string = '';
    receiveDateFrom: any;
    receiveDateTo: any;
    portDateFrom: any;
    portDateTo: any;
    containerStatus: string = '';
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
        private _service: ProdContainerListServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService
    ) {
        super(injector);

        this.colDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 60, pinned: true },
            { headerName: this.l('Container No'), headerTooltip: this.l('Container No'), field: 'containerNo', flex: 1, pinned: true },
            { headerName: this.l('Status'), headerTooltip: this.l('Status'), field: 'status', flex: 1, pinned: true },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', flex: 1 },
            { headerName: this.l('Bill Of Lading No'), headerTooltip: this.l('Bill Of Lading No'), field: 'billOfLadingNo', flex: 1 },
            {
                headerName: this.l('Bill Date'), headerTooltip: this.l('Bill Date'), field: 'billDate', flex: 1,
                valueFormatter: (params) => this.pipe.transform(params.data?.billDate, 'dd/MM/yyyy'),
            },
            {
                headerName: this.l('Shipping Date'), headerTooltip: this.l('Shipping Date (ETD)'), field: 'shippingDate', flex: 1,
                valueFormatter: (params) => this.pipe.transform(params.data?.shippingDate, 'dd/MM/yyyy')
            },
            {
                headerName: this.l('Port Date'), headerTooltip: this.l('Port Date (ETA)'), field: 'portDate', flex: 1,
                valueFormatter: (params) => this.pipe.transform(params.data?.portDate, 'dd/MM/yyyy')
            },
            {
                headerName: this.l('Receive Date'), headerTooltip: this.l('Receive Date'), field: 'receiveDate', flex: 1,
                valueFormatter: (params) => this.pipe.transform(params.data?.receiveDate, 'dd/MM/yyyy')
            },
            { headerName: this.l('Invoice No'), headerTooltip: this.l('Invoice No'), field: 'invoiceNo', flex: 1 },
            {
                headerName: this.l('Cost'), headerTooltip: this.l('Cost'), field: 'totalAmount', flex: 1,
                type: 'rightAligned', aggFunc: this.calTotal,
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.totalAmount)
            },
            {
                headerName: this.l('Insurance'), headerTooltip: this.l('Insurance'), field: 'insurance', flex: 1,
                type: 'rightAligned', aggFunc: this.calTotal,
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.insurance)
            },
            {
                headerName: this.l('Freight'), headerTooltip: this.l('Freight'), field: 'freight', flex: 1,
                type: 'rightAligned', aggFunc: this.calTotal,
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.freight)
            },
            {
                headerName: this.l('C.I.F'), headerTooltip: this.l('C.I.F'), field: 'cif', flex: 1,
                type: 'rightAligned', aggFunc: this.calTotal,
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.cif)
            },
            {
                headerName: this.l('TAX'), headerTooltip: this.l('TAX'), field: 'tax', flex: 1,
                type: 'rightAligned', aggFunc: this.calTotal,
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.tax)
            },
            {
                headerName: this.l('Amount'), headerTooltip: this.l('Amount'), field: 'amount', flex: 1,
                type: 'rightAligned', aggFunc: this.calTotal,
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.amount)
            },
            { headerName: this.l('Cfc'), headerTooltip: this.l('Carfamily Code'), field: 'carfamilyCode', flex: 1 },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', flex: 1 },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName', flex: 1 },
            { headerName: this.l('Base Unit Of Measure'), headerTooltip: this.l('Base Unit Of Measure'), field: 'baseUnitOfMeasure', flex: 1 },
            { headerName: this.l('Warehouse'), headerTooltip: this.l('Warehouse'), field: 'warehouse', flex: 1 },
            { headerName: this.l('Transport'), headerTooltip: this.l('Transport'), field: 'transport', flex: 1 },
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
        this._service.getProdContainerListSearch(
            this.containerNo,
            this.supplierNo,
            this.billNo,
            this.invoiceNo,
            this.portDateFrom ? moment(this.portDateFrom) : undefined,
            this.portDateTo ? moment(this.portDateTo) : undefined,
            this.receiveDateFrom ? moment(this.receiveDateFrom) : undefined,
            this.receiveDateTo ? moment(this.receiveDateTo) : undefined,
            this.containerStatus,
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
                    var _sumFreight = 0;
                    var _sumInsurance = 0;
                    var _sumTax = 0;
                    var _sumCif = 0;
                    var _sumAmount = 0;
                    var _sumCost = 0;
                    _sumFreight = result.items[0].grandFreight;
                    _sumInsurance = result.items[0].grandInsurance;
                    _sumTax = result.items[0].grandTax;
                    _sumCif = result.items[0].grandCif;
                    _sumAmount = result.items[0].grandAmount;
                    _sumCost = result.items[0].grandCost;
                    var rows = this.createRow(1, _sumFreight, _sumInsurance, _sumTax, _sumCif, _sumAmount, _sumCost);
                    this.dataParams!.api.setPinnedBottomRowData(rows);
                } else {
                    this.dataParams!.api.setPinnedBottomRowData(null);
                }
                this.resetGridView();
                this.isLoading = false;
            });
    }

    clearTextSearch() {
        this.billNo = '';
        this.containerNo = '';
        this.invoiceNo = '';
        this.containerStatus = '';
        this.portDateFrom = '';
        this.portDateTo = '';
        this.supplierNo = '';
        this.receiveDateFrom = '';
        this.receiveDateTo = '';
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getProdContainerListSearch(
            this.containerNo,
            this.supplierNo,
            this.billNo,
            this.invoiceNo,
            this.portDateFrom ? moment(this.portDateFrom) : undefined,
            this.portDateTo ? moment(this.portDateTo) : undefined,
            this.receiveDateFrom ? moment(this.receiveDateFrom) : undefined,
            this.receiveDateTo ? moment(this.receiveDateTo) : undefined,
            this.containerStatus,
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
                    var _sumFreight = 0;
                    var _sumInsurance = 0;
                    var _sumTax = 0;
                    var _sumCif = 0;
                    var _sumAmount = 0;
                    var _sumCost = 0;
                    _sumFreight = result.items[0].grandFreight;
                    _sumInsurance = result.items[0].grandInsurance;
                    _sumTax = result.items[0].grandTax;
                    _sumCif = result.items[0].grandCif;
                    _sumAmount = result.items[0].grandAmount;
                    _sumCost = result.items[0].grandCost;
                    var rows = this.createRow(1, _sumFreight, _sumInsurance, _sumTax, _sumCif, _sumAmount, _sumCost);
                    this.dataParams!.api.setPinnedBottomRowData(rows);
                } else {
                    this.dataParams!.api.setPinnedBottomRowData(null);
                }
                this.resetGridView();
                this.isLoading = false;
            });
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => ProdContainerListDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new ProdContainerListDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);

        this._selectrow = this.saveSelectedRow.id;
    }

    exportToExcel(): void {
        this.isLoading = true;
        this._service.getProdContainerListToExcel(
            this.containerNo,
            this.supplierNo,
            this.billNo,
            this.invoiceNo,
            this.portDateFrom ? moment(this.portDateFrom) : undefined,
            this.portDateTo ? moment(this.portDateTo) : undefined,
            this.receiveDateFrom ? moment(this.receiveDateFrom) : undefined,
            this.receiveDateTo ? moment(this.receiveDateTo) : undefined,
            this.containerStatus)
            .pipe(finalize(() => this.isLoading = false))
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            });
    }

    createRow(count: number, sumFreight: number, sumInsurance: number,
        sumTax: number, sumCif: number, sumAmount: number, sumCost: number): any[] {
        let result: any[] = [];

        for (var i = 0; i < count; i++) {
            result.push({
                containerNo: 'Grand Total',
                freight: sumFreight,
                insurance: sumInsurance,
                tax: sumTax,
                cif: sumCif,
                amount: sumAmount,
                totalAmount: sumCost
            });
        }
        return result;
    }

    calTotal(values) {
        var sum = 0;
        values.forEach(function (value) { sum += Number(value); });
        return sum;
    }

    setvalradio(i: string) {
        let _btnUncheck = document.querySelector('.actionButton_w' + i + '.active');
        if (_btnUncheck) {
            let objbtn = document.querySelectorAll('.groupBtn');
            for (let i = 0; objbtn[i]; i++) { objbtn[i].classList.remove('active'); }

            this.containerStatus = '';
        }
        else {
            let objbtn = document.querySelectorAll('.groupBtn');
            for (let i = 0; objbtn[i]; i++) { objbtn[i].classList.remove('active'); }

            let _btn = document.querySelector('.actionButton_w' + i);
            if (_btn) _btn.classList.add('active');
            this.containerStatus = i;
        }
        this.searchDatas();
    }
}

