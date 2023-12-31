import { Component, Injector, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { ProdInvoiceDetailsDto, ProdInvoiceDto, ProdInvoiceServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from "ngx-bootstrap/modal";
import { finalize } from "rxjs/operators";
import { BsDatepickerDirective } from "ngx-bootstrap/datepicker";
import * as moment from 'moment';
import { InvoiceComponent } from "./invoice.component";
import { formatDate } from "@angular/common";
import { DataFormatService } from "@app/shared/common/services/data-format.service";
import { CellValueChangedEvent, EditableCallbackParams, GridOptions, GridParams } from "@ag-grid-enterprise/all-modules";
import { FrameworkComponent, PaginationParamsModel } from "@app/shared/common/models/base.model";
import { AgCellButtonRendererComponent } from "@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component";

@Component({
    selector: 'editModal',
    templateUrl: './edit-invoice-modal.component.html'
})
export class EditInvoiceModalComponent extends AppComponentBase {
    @ViewChild('editModal', { static: true }) modal: ModalDirective;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    data: any[] = [];
    viewColDefs: any;
    editColDefs: any;
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 1000000000,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
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
        autoGroupColumnDef: {
            minWidth: 400,
            cellRendererParams: {
                suppressCount: true,
                checkbox: false,
            },
            pinned: true,
        },
        suppressAggFuncInHeader: true,
        enableCellChangeFlash: true,
        animateRows: true
    };

    gridOptions: GridOptions = {
        columnTypes: {
            editableColumn: {

                editable: (params: EditableCallbackParams) => { return true; },
                valueParser: 'Number(newValue)',
                cellRenderer: 'agAnimateShowChangeCellRenderer',
                filter: 'agNumberColumnFilter',

                cellClass: (params) => { return this.cellEditGetsClass(params); },
                valueGetter: (params) => { return this.syncValidateValueGetter(params) },
                valueSetter: (params) => { return this.syncValidateValueSetter(params) },
            },
            valueColumn: {
                editable: true,
                valueParser: 'Number(newValue)',
                cellClass: 'number-cell',
                cellRenderer: 'agAnimateShowChangeCellRenderer',
                filter: 'agNumberColumnFilter',
            },
        },
    };
    frameworkComponents: FrameworkComponent;
    dataParams: GridParams | undefined;

    rowData: ProdInvoiceDto = new ProdInvoiceDto();
    saving = false;
    isEdit: boolean = false;
    header: string = '';
    _billDate: any;
    _invoiceDate: any;
    _freight;
    _insurance;
    _cif;
    _thc;
    _tax;
    _vat;
    list = [
        { value: 'NEW', label: "NEW" },
        { value: 'PRE CUSTOMS', label: "PRE CUSTOMS (PAID)" }
    ];

    datasEdit: ProdInvoiceDetailsDto[] = [];
    valueChange: string = '';
    columnChange: string = '';
    processNoUpdate: boolean = false;

    constructor(
        private injector: Injector,
        private _service: ProdInvoiceServiceProxy,
        private _component: InvoiceComponent,
        private _fm: DataFormatService
    ) {
        super(injector);

        this.editColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 80, pinned: true },
            { headerName: this.l('Container No'), headerTooltip: this.l('Container No'), field: 'containerNo', width: 130, pinned: true },
            {
                headerName: this.l('Cost'), headerTooltip: this.l('Cost'), field: 'cost', width: 100, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.cost != null ? this._fm.formatMoney_decimal(params.data?.cost) : 0)
            },
            {
                headerName: this.l('Insurance'), headerTooltip: this.l('Insurance'), field: 'insurance', width: 110, type: 'editableColumn',
                cellRenderer: (params) => (params.data?.insurance != null ? this._fm.formatMoney_decimal(params.data?.insurance) : 0)
            },
            {
                headerName: this.l('Freight'), headerTooltip: this.l('Freight'), field: 'freight', width: 100, type: 'editableColumn',
                cellRenderer: (params) => (params.data?.freight != null ? this._fm.formatMoney_decimal(params.data?.freight) : 0)
            },
            {
                headerName: this.l('C.I.F'), headerTooltip: this.l('Cif'), field: 'cif', width: 100, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.cif != null ? this._fm.formatMoney_decimal(params.data?.cif) : 0)
            },
            {
                headerName: this.l('THC'), headerTooltip: this.l('THC'), field: 'thc', width: 100, type: 'editableColumn',
                cellRenderer: (params) => (params.data?.thc != null ? this._fm.formatMoney_decimal(params.data?.thc) : 0)
            },
            {
                headerName: this.l('TAX'), headerTooltip: this.l('Tax'), field: 'tax', width: 100, type: 'editableColumn',
                cellRenderer: (params) => (params.data?.tax != null ? this._fm.formatMoney_decimal(params.data?.tax) : 0)
            },
            {
                headerName: this.l('VAT'), headerTooltip: this.l('Vat'), field: 'vat', width: 100, type: 'editableColumn',
                cellRenderer: (params) => (params.data?.vat != null ? this._fm.formatMoney_decimal(params.data?.vat) : 0)
            },
            { headerName: this.l('Currency'), headerTooltip: this.l('Currency'), field: 'currency', width: 100 },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', width: 120 },
            { headerName: this.l('Cfc'), headerTooltip: this.l('Carfamily Code'), field: 'carfamilyCode', width: 100 },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', width: 120 },
            {
                headerName: this.l('Qty'), headerTooltip: this.l('Qty'), field: 'usageQty', width: 100, type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.usageQty)
            },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName', width: 200 }
        ];

        this.viewColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 80, pinned: true },
            { headerName: this.l('Container No'), headerTooltip: this.l('Container No'), field: 'containerNo', width: 130, pinned: true },
            {
                headerName: this.l('Freight'), headerTooltip: this.l('Freight'), field: 'freight', width: 100, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.freight != null ? this._fm.formatMoney_decimal(params.data?.freight) : 0)
            },
            {
                headerName: this.l('Insurance'), headerTooltip: this.l('Insurance'), field: 'insurance', width: 110, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.insurance != null ? this._fm.formatMoney_decimal(params.data?.insurance) : 0)
            },
            {
                headerName: this.l('C.I.F'), headerTooltip: this.l('Cif'), field: 'cif', width: 100, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.cif != null ? this._fm.formatMoney_decimal(params.data?.cif) : 0)
            },
            {
                headerName: this.l('THC'), headerTooltip: this.l('THC'), field: 'thc', width: 100, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.thc != null ? this._fm.formatMoney_decimal(params.data?.thc) : 0)
            },
            {
                headerName: this.l('TAX'), headerTooltip: this.l('Tax'), field: 'tax', width: 100, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.tax != null ? this._fm.formatMoney_decimal(params.data?.tax) : 0)
            },
            {
                headerName: this.l('VAT'), headerTooltip: this.l('Vat'), field: 'vat', width: 100, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.vat != null ? this._fm.formatMoney_decimal(params.data?.vat) : 0)
            },
            { headerName: this.l('Currency'), headerTooltip: this.l('Currency'), field: 'currency', width: 100 },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', width: 120 },
            { headerName: this.l('Cfc'), headerTooltip: this.l('Carfamily Code'), field: 'carfamilyCode', width: 100 },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', width: 120 },
            {
                headerName: this.l('Qty'), headerTooltip: this.l('Qty'), field: 'usageQty', width: 100, type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.usageQty)
            },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName', width: 200 }
        ];

        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent
        };
    }

    syncValidateValueGetter(params) {

        try {
            if (params.data == undefined) return 0;

            let field = params.colDef.field;
            let v = params.data[field];
            if (v == null || v == undefined || Number.isNaN(v)) {    // không đúng định dạng
                return 0;
            }
            else {   //hợp lệ
                return v;
            }
        } catch (e) {
            return 0;
        }
    }

    syncValidateValueSetter(params) {
        try {
            if (params.data == undefined || params.data == null) return 0;
            let field = params.colDef.field;
            let v = params.newValue;

            if (v == null || v == undefined || Number.isNaN(v)) {
                params.data[field] = 0;
                params.data['cif'] = params.data['cost'] + params.data['insurance'] + params.data['freight'];
                params.api.applyTransaction({ update: [params.data] });
                return 0;
            }
            else {  //hợp lệ
                params.newValue = ((params.newValue) < 0) ? params.oldValue : params.newValue;
                params.data[field] = params.newValue;

                params.data['cif'] = params.data['cost'] + params.data['insurance'] + params.data['freight'];
                params.api.applyTransaction({ update: [params.data] });

                return params.newValue;
            }
        } catch (e) {
            return 0;
        }
    }

    onCellValueChanged(params: CellValueChangedEvent) {
        params.data['cif'] = params.data['cost'] + params.data['insurance'] + params.data['freight'];
        params.api.applyTransaction({ update: [params.data] });

        if (params.newValue == null || Number.isNaN(params.newValue) || params.newValue == undefined) return;

        let ischange = true;
        this.datasEdit.forEach((item) => {  // kiểm tra row change đã tồn tại trong mảng edit chưa
            if (item.keyRow == params.data.keyRow) ischange = false;
        });
        if (ischange && this.processNoUpdate != true) {
            this.datasEdit.push(params.data);
            this.valueChange = params.newValue;
            this.columnChange = params.column.getId();
            this.saveChange();
        }
    }

    cellEditGetsClass(params) {
        if (params.data == undefined || params.data == null) return ['cell-edit', 'number-cell',];
        else return ['cell-edit', 'number-cell', 'cell-edit-edited'];
    }

    headerEditClass(params) {
        if (params.data == undefined || params.data == null) return [''];
        else return ['cell-edit', 'number-cell', 'cell-edit-edited'];
    }

    ngOnInit() { }

    show(type, material?: ProdInvoiceDto): void {
        if (type == 'Edit') this.isEdit = true;
        else this.isEdit = false;
        this.header = type;
        if (material) this.rowData = material;
        else this.rowData = new ProdInvoiceDto();

        if (type == 'Edit') {
            const dateValue = this.rowData.invoiceDate ? new Date(this.rowData.invoiceDate?.toString()) : new Date();
            this.datepicker?.bsValueChange.emit(dateValue);
        }
        else {
            this._invoiceDate = this.rowData.invoiceDate ? formatDate(new Date(this.rowData.invoiceDate?.toString()), 'dd/MM/yyyy', 'en-US') : undefined;
        }

        this._billDate = this.rowData.billDate ? formatDate(new Date(this.rowData.billDate?.toString()), 'dd/MM/yyyy', 'en-US') : undefined;

        this._service.getProdInvoiceDetailsSearch(this.rowData.id, '', this.paginationParams.skipCount, this.paginationParams.pageSize)
            .subscribe((result) => {
                this.data = result.items ?? [];
            });

        setTimeout(() => {
            this.modal.show();
        }, 300);
    }

    save(): void {
        if (this._invoiceDate == undefined) {
            this.notify.warn('Invoice Date is Required!');
            return;
        }

        if (this._invoiceDate == undefined && this.rowData.status != 'NEW') {
            this.notify.warn('Invoice Date is Required!');
            return;
        }

        if (this._invoiceDate != undefined && this._invoiceDate < this.rowData.billDate) {
            this.notify.warn('InvoiceDate cannot be less than BillDate!!');
            return;
        }
        this.rowData.invoiceDate = moment(this._invoiceDate);

        this.saving = true;
        this._service.editInvoice(this.rowData)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this._component.searchDatas();
                this.close();
            });
    }

    saveChange() {
        if (this.datasEdit) {
            if (this.datasEdit.length > 0) {
                let input = Object.assign(new ProdInvoiceDetailsDto(), {
                    //invoice
                    id: this.datasEdit[0].id,
                    insurance: this.datasEdit[0].insurance,
                    freight: this.datasEdit[0].freight,
                    thc: this.datasEdit[0].thc,
                    tax: this.datasEdit[0].tax,
                    vat: this.datasEdit[0].vat
                });

                this._service.updateAmountInvoice(input)
                    .subscribe((result) => {
                        this.notify.success(this.l('SavedSuccessfully'));
                        this.datasEdit = [];
                        this.valueChange = "";
                        this.columnChange = "";
                    });
            }
        }
    }

    callBackDataGrid(params: GridParams) {
        this.dataParams = params;
    }

    close(): void {
        this.modal.hide();
    }
}
