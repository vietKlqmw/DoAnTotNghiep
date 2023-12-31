import { GridApi } from '@ag-grid-enterprise/all-modules';
import { DatePipe } from '@angular/common';
import { Component, Injector, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ProdInvoiceDto, ProdInvoiceServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ceil } from 'lodash';
import { finalize } from 'rxjs/operators';
import * as moment from 'moment';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { EditInvoiceModalComponent } from './edit-invoice-modal.component';

@Component({
    selector: 'app-invoice',
    templateUrl: './invoice.component.html',
    styleUrls: ['../../../screen-modal.less'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class InvoiceComponent extends AppComponentBase implements OnInit {
    @ViewChild('editModal', { static: true }) editModal: EditInvoiceModalComponent;

    defaultColDefs: CustomColDef[] = [];
    colDefs: any;
    colDefsDetails: any;
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    paginationParamsDetails: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    selectedRow: ProdInvoiceDto = new ProdInvoiceDto();
    saveSelectedRow: ProdInvoiceDto = new ProdInvoiceDto();
    selectedRowDetails: ProdInvoiceDto = new ProdInvoiceDto();
    saveSelectedRowDetails: ProdInvoiceDto = new ProdInvoiceDto();
    datas: ProdInvoiceDto = new ProdInvoiceDto();
    dataParams: GridParams | undefined;
    dataParamsDetails: GridParams | undefined;
    rowData: any[] = [];
    rowDataDetails: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    isLoading: boolean = false;

    shipmentNo: string = '';
    billNo: string = '';
    invoiceNo: string = '';
    supplierNo: string = '';
    containerNo: string = '';
    billDateFrom: any;
    billDateTo: any;
    invoiceDateFrom: any;
    invoiceDateTo: any;
    _selectrow;
    _pageSizeDetails = 500;
    notDelete: boolean = false;
    listStatus = [
        { value: '', label: "Status" },
        { value: 'NEW', label: "NEW" },
        { value: 'PRE CUSTOMS', label: "PRE CUSTOMS (PAID)" },
        { value: 'CUSTOMS DECLARED', label: "CUSTOMS DECLARED" }
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
        private _service: ProdInvoiceServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService
    ) {
        super(injector);

        this.colDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 80 },
            { headerName: this.l('Invoice No'), headerTooltip: this.l('Invoice No'), field: 'invoiceNo', flex: 1 },
            {
                headerName: this.l('Invoice Date'), headerTooltip: this.l('Invoice Date'), field: 'invoiceDate', flex: 1,
                valueFormatter: (params) => this.pipe.transform(params.data?.invoiceDate, 'dd/MM/yyyy')
            },
            { headerName: this.l('Bill No'), headerTooltip: this.l('Bill No'), field: 'billNo', flex: 1 },
            {
                headerName: this.l('Bill Date'), headerTooltip: this.l('Bill Date'), field: 'billDate', flex: 1,
                valueFormatter: (params) => this.pipe.transform(params.data?.billDate, 'dd/MM/yyyy')
            },
            { headerName: this.l('Shipment No'), headerTooltip: this.l('Shipment No'), field: 'shipmentNo', flex: 1 },
            { headerName: this.l('Forwarder'), headerTooltip: this.l('Forwarder'), field: 'forwarder', flex: 1 },
            { headerName: this.l('Status'), headerTooltip: this.l('Status'), field: 'status', flex: 1 }
        ];

        this.colDefsDetails = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParamsDetails.pageSize * (this.paginationParamsDetails.pageNum - 1), cellClass: ['text-center'], width: 60, pinned: true },
            { headerName: this.l('Container No'), headerTooltip: this.l('Container No'), field: 'containerNo', flex: 1, pinned: true },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', pinned: true },
            {
                headerName: this.l('Qty'), headerTooltip: this.l('Qty'), field: 'usageQty', flex: 1, type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.usageQty),
                aggFunc: this.calTotal
            },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', flex: 1 },
            {
                headerName: this.l('Cost'), headerTooltip: this.l('Cost'), field: 'cost', flex: 1, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.cost != null ? this._fm.formatMoney_decimal(params.data?.cost) : 0),
                aggFunc: this.calTotal
            },
            {
                headerName: this.l('Insurance'), headerTooltip: this.l('Insurance'), field: 'insurance', flex: 1, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.insurance != null ? this._fm.formatMoney_decimal(params.data?.insurance) : 0),
                aggFunc: this.calTotal
            },
            {
                headerName: this.l('Freight'), headerTooltip: this.l('Freight'), field: 'freight', flex: 1, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.freight != null ? this._fm.formatMoney_decimal(params.data?.freight) : 0),
                aggFunc: this.calTotal
            },
            {
                headerName: this.l('C.I.F'), headerTooltip: this.l('Cif'), field: 'cif', flex: 1, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.cif != null ? this._fm.formatMoney_decimal(params.data?.cif) : 0),
                aggFunc: this.calTotal
            },
            {
                headerName: this.l('THC'), headerTooltip: this.l('THC'), field: 'thc', flex: 1, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.thc != null ? this._fm.formatMoney_decimal(params.data?.thc) : 0),
                aggFunc: this.calTotal
            },
            {
                headerName: this.l('TAX'), headerTooltip: this.l('Tax'), field: 'tax', flex: 1, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.tax != null ? this._fm.formatMoney_decimal(params.data?.tax) : 0),
                aggFunc: this.calTotal
            },
            {
                headerName: this.l('VAT'), headerTooltip: this.l('Vat'), field: 'vat', flex: 1, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.vat != null ? this._fm.formatMoney_decimal(params.data?.vat) : 0),
                aggFunc: this.calTotal
            },
            // { headerName: this.l('TAX Rate'), headerTooltip: this.l('Tax Rate'), field: 'taxRate', flex: 1, type: 'rightAligned' },
            // { headerName: this.l('VAT Rate'), headerTooltip: this.l('Vat Rate'), field: 'vatRate', Flex: 1, type: 'rightAligned' },
            { headerName: this.l('Currency'), headerTooltip: this.l('Currency'), field: 'currency', flex: 1 },
            // {
            //     headerName: this.l('Gross Weight'), headerTooltip: this.l('Gross Weight'), field: 'grossWeight', flex: 1, type: 'rightAligned',
            //     valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.grossWeight, 4)
            // },
            { headerName: this.l('Cfc'), headerTooltip: this.l('Carfamily Code'), field: 'carfamilyCode', flex: 1 },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName', flex: 1 }
        ];

        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit() {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.paginationParamsDetails = { pageNum: 1, pageSize: 500, totalCount: 0 };
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
        this._service.getProdInvoiceSearch(
            this.invoiceNo,
            this.invoiceDateFrom ? moment(this.invoiceDateFrom) : undefined,
            this.invoiceDateTo ? moment(this.invoiceDateTo) : undefined,
            this.billNo,
            this.shipmentNo,
            this.containerNo,
            this.billDateFrom ? moment(this.billDateFrom) : undefined,
            this.billDateTo ? moment(this.billDateTo) : undefined,
            this.supplierNo,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                if (this.rowData.length > 0) {
                    this.searchDatasDetails(this.rowData[0].id);
                } else {
                    this.rowDataDetails = [];
                }
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                //this.resetGridView();
                this.isLoading = false;
            });
    }

    clearTextSearch() {
        this.billNo = '';
        this.containerNo = '';
        this.invoiceNo = '';
        this.shipmentNo = '';
        this.invoiceDateFrom = '';
        this.invoiceDateTo = '';
        this.supplierNo = '';
        this.billDateFrom = '';
        this.billDateTo = '';
        this.paginationParamsDetails = { pageNum: 1, pageSize: this._pageSizeDetails, totalCount: 0 };
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getProdInvoiceSearch(
            this.invoiceNo,
            this.invoiceDateFrom ? moment(this.invoiceDateFrom) : undefined,
            this.invoiceDateTo ? moment(this.invoiceDateTo) : undefined,
            this.billNo,
            this.shipmentNo,
            this.containerNo,
            this.billDateFrom ? moment(this.billDateFrom) : undefined,
            this.billDateTo ? moment(this.billDateTo) : undefined,
            this.supplierNo,
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
            //this.resetGridView();
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
                //this.resetGridView();
                this.isLoading = false;
            });
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => ProdInvoiceDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new ProdInvoiceDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);

        this._selectrow = this.saveSelectedRow.id;
        this.paginationParamsDetails.pageNum = 1;
        this.paginationParamsDetails.skipCount = 0;
        this.searchDatasDetails(this.saveSelectedRow.id);

    }

    exportToExcel(): void {
        this.isLoading = true;
        this._service.getProdInvoiceToExcel(
            this.invoiceNo,
            this.invoiceDateFrom ? moment(this.invoiceDateFrom) : undefined,
            this.invoiceDateTo ? moment(this.invoiceDateTo) : undefined,
            this.billNo,
            this.shipmentNo,
            this.containerNo,
            this.billDateFrom ? moment(this.billDateFrom) : undefined,
            this.billDateTo ? moment(this.billDateTo) : undefined,
            this.supplierNo)
            .pipe(finalize(() => this.isLoading = false))
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            });
    }

    //Details
    autoSizeAllDetails() {
        const allColumnIds: string[] = [];
        this.dataParamsDetails.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "stt") {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParamsDetails.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridViewDetails() {

        setTimeout(() => {
            this.dataParamsDetails.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSizeAllDetails();
        })
    }

    searchDatasDetails(id): void {
        this.isLoading = true;
        this._service.getProdInvoiceDetailsSearch(
            id,
            '',
            this.paginationParamsDetails.skipCount,
            this.paginationParamsDetails.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParamsDetails!.api)))
            .subscribe((result) => {
                this.paginationParamsDetails.totalCount = result.totalCount;
                this.rowDataDetails = result.items;
                this.paginationParamsDetails.totalPage = ceil(result.totalCount / (this.paginationParamsDetails.pageSize ?? 0));

                if (result.totalCount > 0) {
                    var _sumCif = 0;
                    var _sumQty = 0;
                    var _sumTax = 0;
                    var _sumVat = 0;
                    var _sumFreight = 0;
                    var _sumInsurance = 0;
                    var _sumThc = 0;
                    var _sumCost = 0;
                    _sumQty = result.items[0].grandQty;
                    _sumCif = result.items[0].grandCif;
                    _sumTax = result.items[0].grandTax;
                    _sumVat = result.items[0].grandVat;
                    _sumFreight = result.items[0].grandFreight;
                    _sumInsurance = result.items[0].grandInsurance;
                    _sumThc = result.items[0].grandThc;
                    _sumCost = result.items[0].grandCost;
                    var rows = this.createRow(1, _sumQty, _sumCif, _sumTax, _sumVat, _sumFreight, _sumInsurance, _sumThc, _sumCost);
                    this.dataParamsDetails!.api.setPinnedBottomRowData(rows);
                } else {
                    this.dataParamsDetails!.api.setPinnedBottomRowData(null);
                    this.saveSelectedRowDetails = new ProdInvoiceDto();
                }

                this.resetGridViewDetails();
                this.isLoading = false;
            });
    }

    getDatasDetails(paginationParamsDetails?: PaginationParamsModel) {
        return this._service.getProdInvoiceDetailsSearch(
            this._selectrow,
            '',
            this.paginationParamsDetails.skipCount,
            this.paginationParamsDetails.pageSize
        );
    }

    callBackDataGridDetails(params: GridParams) {
        this.isLoading = true;
        this.dataParamsDetails = params;
        params.api.paginationSetPageSize(this.paginationParamsDetails.pageSize);
        this.paginationParamsDetails.skipCount =
            ((this.paginationParamsDetails.pageNum ?? 1) - 1) * (this.paginationParamsDetails.pageSize ?? 0);
        this.paginationParamsDetails.pageSize = this.paginationParamsDetails.pageSize;
        this.getDatasDetails(this.paginationParamsDetails)
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParamsDetails!.api)))
            .subscribe((result) => {
                this.paginationParamsDetails.totalCount = result.totalCount;
                this.rowDataDetails = result.items ?? [];
                this.paginationParamsDetails.totalPage = ceil(result.totalCount / (this.paginationParamsDetails.pageSize ?? 0));
                this.resetGridViewDetails();
                this.isLoading = false;
            });
    }

    changePageDetails(paginationParamsDetails) {
        this.isLoading = true;
        this.paginationParamsDetails = paginationParamsDetails;
        this.paginationParamsDetails.skipCount = (paginationParamsDetails.pageNum - 1) * paginationParamsDetails.pageSize;
        this.getDatasDetails(this.paginationParamsDetails).subscribe((result) => {
            this.paginationParamsDetails.totalCount = result.totalCount;
            this.rowDataDetails = result.items;
            this.paginationParamsDetails.totalPage = ceil(result.totalCount / (this.paginationParamsDetails.pageSize ?? 0));
            this.resetGridViewDetails();
            this.isLoading = false;
        });

        this._pageSizeDetails = this.paginationParamsDetails.pageSize;
    }

    onChangeRowSelectionDetails(params: { api: { getSelectedRows: () => ProdInvoiceDto[] } }) {
        this.saveSelectedRowDetails = params.api.getSelectedRows()[0] ?? new ProdInvoiceDto();
        this.selectedRowDetails = Object.assign({}, this.saveSelectedRowDetails);

        this._selectrow = this.saveSelectedRow.id;
        if(this._selectrow){
            if(this.saveSelectedRow.status == 'CUSTOMS DECLARED') this.notDelete = true;
            else this.notDelete = false;
        }else{
            this.notDelete = false;
        }
    }

    exportToExcelDetails(): void {
        this.isLoading = true;
        this._service.getProdInvoiceDetailsToExcel(this._selectrow)
            .pipe(finalize(() => this.isLoading = false))
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            });
    }

    createRow(count: number, sumSty: number, sumCif: number, sumTax: number,
        sumVat: number, sumFreight: number, sumInsurance: number, sumThc: number, sumCost): any[] {
        let result: any[] = [];

        for (var i = 0; i < count; i++) {
            result.push({
                partNo: 'Grand Total',
                freight: sumFreight,
                insurance: sumInsurance,
                cif: sumCif,
                tax: sumTax,
                vat: sumVat,
                usageQty: sumSty,
                thc: sumThc,
                cost: sumCost
            });
        }
        return result;
    }

    calTotal(values) {
        var sum = 0;
        values.forEach(function (value) { sum += Number(value); });
        return sum;
    }

    editInvoice(e): void {
        let input = Object.assign(new ProdInvoiceDto(), {
            //invoice
            id: this.saveSelectedRow.id,
            invoiceNo: this.saveSelectedRow.invoiceNo,
            invoiceDate: this.saveSelectedRow.invoiceDate,
            forwarder: this.saveSelectedRow.forwarder,
            status: this.saveSelectedRow.status,
            shipmentNo: this.saveSelectedRow.shipmentNo,
            billNo: this.saveSelectedRow.billNo,
            billDate: this.saveSelectedRow.billDate,
            //invoice details
            partNo: this.saveSelectedRowDetails.partNo,
            insurance: this.saveSelectedRowDetails.insurance,
            containerNo: this.saveSelectedRowDetails.containerNo,
            supplierNo: this.saveSelectedRowDetails.supplierNo,
            freight: this.saveSelectedRowDetails.freight,
            thc: this.saveSelectedRowDetails.thc,
            cif: this.saveSelectedRowDetails.cif,
            tax: this.saveSelectedRowDetails.tax,
            vat: this.saveSelectedRowDetails.vat,
            usageQty: this.saveSelectedRowDetails.usageQty,
            partName: this.saveSelectedRowDetails.partName,
            carfamilyCode: this.saveSelectedRowDetails.carfamilyCode,
            currency: this.saveSelectedRowDetails.currency
        });
        this.editModal.show(e, input);
    }
}

