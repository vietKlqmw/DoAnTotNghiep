import { DatePipe } from '@angular/common';
import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ProdContainerRentalWHPlanDto, ProdContainerRentalWHPlanServiceProxy, ProdInvoiceDto, ProdOthersServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { ContainerWarehouseComponent } from './container-warehouse.component';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';

@Component({
    selector: 'addGrnContWarehouse',
    templateUrl: './add-grn-container-warehouse-modal.component.html'
})
export class AddGrnContWarehouseModalComponent extends AppComponentBase {
    @ViewChild('addGrnContWarehouse', { static: true }) modal: ModalDirective;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    data: any[] = [];
    viewColDefs: any;
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 1000000000,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    selectedRow: ProdContainerRentalWHPlanDto = new ProdContainerRentalWHPlanDto();
    saveSelectedRow: ProdContainerRentalWHPlanDto = new ProdContainerRentalWHPlanDto();
    rowSelection = 'multiple';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    dataParams: GridParams | undefined;
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

    saving = false;

    list = [{ value: '', label: '', address: '' }];
    _warehouse;
    _goodsReceived;
    _receiveDate = new Date();
    isExcel: boolean = true;
    isPdf: boolean = false;
    _selectrow;
    listCont = '';

    constructor(injector: Injector,
        private _service: ProdContainerRentalWHPlanServiceProxy,
        private _component: ContainerWarehouseComponent,
        private _other: ProdOthersServiceProxy,
        private _fm: DataFormatService
    ) {
        super(injector);

        this.viewColDefs = [
            {
                headerName: "", headerTooltip: "", field: "checked", width: 30, pinned: true,
                headerClass: ["align-checkbox-header"],
                cellClass: ["check-box-center"],
                checkboxSelection: true,
                headerCheckboxSelection: true,
                headerCheckboxSelectionFilteredOnly: true
            },
            { headerName: this.l('Container No'), headerTooltip: this.l('Container No'), field: 'containerNo', width: 130, pinned: true },
            {
                headerName: this.l('Qty'), headerTooltip: this.l('Qty'), field: 'usageQty', width: 100, type: 'rightAligned', pinned: true,
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.usageQty),
            },
            { headerName: this.l('Invoice No'), headerTooltip: this.l('Invoice No'), field: 'invoiceNo', width: 130 },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', width: 120 },
            { headerName: this.l('Forwarder'), headerTooltip: this.l('Forwarder'), field: 'forwarder', width: 120 },
            {
                headerName: this.l('Freight'), headerTooltip: this.l('Freight'), field: 'freight', width: 100, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.freight != null ? this._fm.formatMoney_decimal(params.data?.freight) : 0),
            },
            {
                headerName: this.l('Insurance'), headerTooltip: this.l('Insurance'), field: 'insurance', width: 110, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.insurance != null ? this._fm.formatMoney_decimal(params.data?.insurance) : 0),
            },
            {
                headerName: this.l('C.I.F'), headerTooltip: this.l('Cif'), field: 'cif', width: 100, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.cif != null ? this._fm.formatMoney_decimal(params.data?.cif) : 0),
            },
            {
                headerName: this.l('THC'), headerTooltip: this.l('THC'), field: 'thc', width: 100, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.thc != null ? this._fm.formatMoney_decimal(params.data?.thc) : 0),
            },
            {
                headerName: this.l('TAX'), headerTooltip: this.l('Tax'), field: 'tax', width: 100, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.tax != null ? this._fm.formatMoney_decimal(params.data?.tax) : 0),
            },
            {
                headerName: this.l('VAT'), headerTooltip: this.l('Vat'), field: 'vat', width: 100, type: 'rightAligned',
                cellRenderer: (params) => (params.data?.vat != null ? this._fm.formatMoney_decimal(params.data?.vat) : 0),
            },
            { headerName: this.l('Currency'), headerTooltip: this.l('Currency'), field: 'currency', width: 100 }
        ];

        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }
    ngOnInit() {
        this._other.getListContForWarehouse()
            .subscribe((result) => {
                this.data = result ?? [];
            });

        this._other.getListWarehouse()
            .subscribe(result => {
                result.forEach(e => {
                    this.list.push({ value: e.storageLocation, label: e.storageLocation, address: e.addressLanguageVn })
                })
            })
    }

    show(): void {
        this.listCont = '';
        this.modal.show();

    }

    callBackDataGrid(params: GridParams) {
        this.dataParams = params;
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => ProdContainerRentalWHPlanDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new ProdContainerRentalWHPlanDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);

        this._selectrow = this.saveSelectedRow.id;

        this.listCont = '';
        if(params.api.getSelectedRows().length) {
            for(var i = 0; i < params.api.getSelectedRows().length; i++){
                if(i != params.api.getSelectedRows().length - 1){
                    this.listCont += params.api.getSelectedRows()[i].id + ',';
                }else{
                    this.listCont += params.api.getSelectedRows()[i].id;
                }
            }
        }
    }

    save(): void {
        if(this._goodsReceived == null || this._goodsReceived == ''){
            this.notify.warn('Goods Received Note No is Required!');
            return;
        }
        if(this._receiveDate == undefined){
            this.notify.warn('Receive Date is Required!');
            return;
        }
        if(this._warehouse == null || this._warehouse == ''){
            this.notify.warn('Warehouse is Required!');
            return;
        }
        if(!this.isExcel && !this.isPdf){
            this.notify.warn('Export is Required!');
            return;
        }
        if(this.listCont.length < 1){
            this.notify.warn('Need to choose at least 1 Container!');
            return;
        }

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

    onChangeToExcel(event){
        if (event && this.isPdf){
            this.isExcel = event;
            this.isPdf = !event;
        }else{
            this.isExcel = event;
        }
    }

    onChangeToPdf(event){
        if (event && this.isExcel){
            this.isPdf = event;
            this.isExcel = !event;
        }else{
            this.isPdf = event;
        }
    }

    close(): void {
        this.modal.hide();
        this.modalClose.emit(null);
    }
}
