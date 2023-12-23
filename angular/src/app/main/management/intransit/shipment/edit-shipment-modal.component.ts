import { Component, Injector, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { ProdOthersServiceProxy, ProdShipmentDto, ProdShipmentServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from "ngx-bootstrap/modal";
import { finalize } from "rxjs/operators";
import { BsDatepickerDirective } from "ngx-bootstrap/datepicker";
import * as moment from 'moment';
import { ShipmentComponent } from "./shipment.component";
import { formatDate } from "@angular/common";
import { FrameworkComponent, GridParams, PaginationParamsModel } from "@app/shared/common/models/base.model";
import { AgCellButtonRendererComponent } from "@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component";
import { DataFormatService } from "@app/shared/common/services/data-format.service";

@Component({
    selector: 'editModal',
    templateUrl: './edit-shipment-modal.component.html'
})
export class EditShipmentModalComponent extends AppComponentBase {
    @ViewChild('editModal', { static: true }) modal: ModalDirective;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('datepicker2', { static: false }) datepicker2!: BsDatepickerDirective;
    @ViewChild('datepicker3', { static: false }) datepicker3!: BsDatepickerDirective;

    rowData: ProdShipmentDto = new ProdShipmentDto();
    saving = false;
    rowSelection = 'multiple';
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
    frameworkComponents: FrameworkComponent;
    dataParams: GridParams | undefined;
    selectedRow: ProdShipmentDto = new ProdShipmentDto();
    saveSelectedRow: ProdShipmentDto = new ProdShipmentDto();

    isEdit: boolean = false;
    header: string = '';
    _shipmentDate: any;
    _etd: any;
    _eta: any;
    _forwarder;
    listSupplier = [{ label: '', value: '' }];
    listForwarder = [{ label: '', value: '' }];
    list = [
        { value: 'NEW', label: "NEW" },
        { value: 'PENDING', label: "PENDING" },
        { value: 'ORDERED', label: "ORDERED" }
    ];
    isOrder: boolean = false;
    listCont = '';

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
        private injector: Injector,
        private _service: ProdShipmentServiceProxy,
        private _other: ProdOthersServiceProxy,
        private _component: ShipmentComponent,
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
            { headerName: this.l('Container No'), headerTooltip: this.l('Container No'), field: 'containerNo', flex: 1 },
            { headerName: this.l('Cfc'), headerTooltip: this.l('cfc'), field: 'carfamilyCode', flex: 1 },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', flex: 1 },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName', width: 310 },
            {
                headerName: this.l('Qty'), headerTooltip: this.l('Qty'), field: 'usageQty', width: 90, type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.usageQty)
            }
        ];

        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent
        };
    }

    ngOnInit() {
        this._other.getListSupplier().subscribe(result => {
            result.forEach(e => {
                this.listSupplier.push({ label: e.supplierName, value: e.supplierNo })
            })
        });
    }

    show(type, material?: ProdShipmentDto): void {
        if (type == 'Edit') this.isEdit = true;
        else this.isEdit = false;
        this.header = type;
        if (material) this.rowData = material;
        else this.rowData = new ProdShipmentDto();

        const dateValue = this.rowData.shipmentDate ? new Date(this.rowData.shipmentDate?.toString()) : new Date();
        this.datepicker?.bsValueChange.emit(dateValue);
        const dateValue2 = this.rowData.etd ? new Date(this.rowData.etd?.toString()) : undefined;
        this.datepicker2?.bsValueChange.emit(dateValue2);
        const dateValue3 = this.rowData.eta ? new Date(this.rowData.eta?.toString()) : undefined;
        this.datepicker3?.bsValueChange.emit(dateValue3);

        this.listForwarder = [{ label: '', value: '' }];
        this._forwarder = '';
        this._other.getListForwarder(this.rowData.supplierNo).subscribe(result => {
            result.forEach(e => {
                this.listForwarder.push({ label: e.name, value: e.code })
                this._forwarder = this.rowData.forwarder;
            })
        });

        this._other.getListContainerForShipment(this.rowData.supplierNo)
            .subscribe(result => {
                this.data = result ?? [];
            })

        if (type != 'Edit') this.rowData.shipmentNo = 'S' + formatDate(new Date(), 'HH', 'en-US') + 'H' + formatDate(new Date(), 'mm', 'en-US') + 'I' + formatDate(new Date(), 'ss', 'en-US') + 'P' + formatDate(new Date(), 'yyMMdd', 'en-US');

        if (type == 'Edit') {
            this._other.getListContWhenEditShipment(this.rowData.id).subscribe(res => {
                this.listCont = res;
            })
        } else {
            this.listCont = '';
        }

        setTimeout(() => {
            this.modal.show();
        }, 300)
    }

    save(): void {
        this.rowData.shipmentDate = this._shipmentDate ? moment(this._shipmentDate) : undefined;
        this.rowData.etd = this._etd ? moment(this._etd) : undefined;
        this.rowData.eta = this._eta ? moment(this._eta) : undefined;
        this.rowData.forwarder = this._forwarder;
        this.rowData.listCont = this.listCont;

        if (this.rowData.supplierNo == null) {
            this.notify.warn('SupplierNo is Required!');
            return;
        }
        if (this.rowData.fromPort == null) {
            this.notify.warn('From Port is Required!');
            return;
        }
        if (this.rowData.forwarder == null || this.rowData.forwarder == '') {
            this.notify.warn('Forwarder is Required!');
            return;
        }
        if (this.rowData.status != 'NEW' && this.rowData.shipmentDate == undefined) {
            this.notify.warn('ShipmentDate is Required!');
            return;
        }
        if (this.rowData.status == 'ORDERED' && this.listCont.length == 0) {
            this.notify.warn('Shipment does not contain any containers!');
            return;
        }

        if (this.rowData.status == 'ORDERED') {
            this.message.confirm(
                this.l('Shipment will not be editable!'),
                this.l('AreYouSure'),
                isConfirmed => {
                    if (isConfirmed) {
                        this.saving = true;
                        this._service.editShipment(this.rowData)
                            .pipe(finalize(() => { this.saving = false; }))
                            .subscribe(() => {
                                this.notify.info(this.l('SavedSuccessfully'));
                                this._component.searchDatas();
                                this.close();
                            });
                    }
                }
            );
        }
    }

    changeSupplier(event) {
        this._other.getListContainerForShipment(event)
            .subscribe(result => {
                this.data = result ?? [];
            })

        this.listForwarder = [{ label: '', value: '' }];
        this._other.getListForwarder(event).subscribe(result => {
            result.forEach(e => {
                this.listForwarder.push({ label: e.name, value: e.code })
            })
        });
    }

    callBackDataGrid(params: GridParams) {
        this.dataParams = params;
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => ProdShipmentDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new ProdShipmentDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);

        this.listCont = '';
        if (params.api.getSelectedRows().length) {
            for (var i = 0; i < params.api.getSelectedRows().length; i++) {
                if (i != params.api.getSelectedRows().length - 1) {
                    this.listCont += params.api.getSelectedRows()[i].id + ',';
                } else {
                    this.listCont += params.api.getSelectedRows()[i].id;
                }
            }
        }
    }

    // selectAll() {
    //     this.dataParams.api.forEachNode((e, idx) => {
    //         this.dataParams.api.getRowNode(`${e.rowIndex}`)?.setSelected(true);
    //         this.dataParams.api.setFocusedCell(e.rowIndex,
    //             this.dataParams.api.getColumnDefs()[0]['checked']);
    //         this.dataParams.api.redrawRows();
    //     });
    // }

    close(): void {
        this.modal.hide();
    }
}
