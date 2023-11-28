import { Component, Injector, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { ProdContainerRentalWHPlanDto, ProdContainerRentalWHPlanServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from "ngx-bootstrap/modal";
import { finalize } from "rxjs/operators";
import { BsDatepickerDirective } from "ngx-bootstrap/datepicker";
import * as moment from 'moment';
import { ContainerWarehouseComponent } from "./container-warehouse.component";

@Component({
    selector: 'editContainerWarehouse',
    templateUrl: './edit-container-warehouse-modal.component.html'
})
export class EditContainerWarehouseComponent extends AppComponentBase {
    @ViewChild('editContainerWarehouse', { static: true }) modal: ModalDirective;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('datepicker2', { static: false }) datepicker2!: BsDatepickerDirective;
    @ViewChild('datepicker3', { static: false }) datepicker3!: BsDatepickerDirective;
    @ViewChild('datepicker4', { static: false }) datepicker4!: BsDatepickerDirective;
    @ViewChild('datepicker5', { static: false }) datepicker5!: BsDatepickerDirective;

    rowData: ProdContainerRentalWHPlanDto = new ProdContainerRentalWHPlanDto();

    saving = false;

    isEdit: boolean = false;
    header: string = '';
    _requestDate: any;
    _devanningDate: any;
    _actualDevanningDate: any;
    _gateInPlanTime: any;
    _gateInActualDateTime: any;
    list = [
        { value: 'R', label: "REQUESTED" },
        { value: 'P', label: "PENDING" },
        { value: 'F', label: "CONFIRM" },
        { value: 'C', label: "CANCEL" }
    ];

    constructor(
        private injector: Injector,
        private _service: ProdContainerRentalWHPlanServiceProxy,
        private _component: ContainerWarehouseComponent
    ) {
        super(injector);
    }

    ngOnInit() { }

    show(type, material?: ProdContainerRentalWHPlanDto): void {
        if (type == 'Edit') this.isEdit = true;
        else this.isEdit = false;
        this.header = type;
        if (material) this.rowData = material;
        else this.rowData = new ProdContainerRentalWHPlanDto();

        const dateValue = this.rowData.requestDate ? new Date(this.rowData.requestDate?.toString()) : new Date();
        this.datepicker?.bsValueChange.emit(dateValue);
        const dateValue2 = this.rowData.devanningDate ? new Date(this.rowData.devanningDate?.toString()) : new Date();
        this.datepicker2?.bsValueChange.emit(dateValue2);
        const dateValue3 = this.rowData.actualDevanningDate ? new Date(this.rowData.actualDevanningDate?.toString()) : new Date();
        this.datepicker3?.bsValueChange.emit(dateValue3);
        const dateValue4 = this.rowData.gateInPlanTime ? new Date(this.rowData.gateInPlanTime?.toString()) : new Date();
        this.datepicker4?.bsValueChange.emit(dateValue4);
        const dateValue5 = this.rowData.gateInActualDateTime ? new Date(this.rowData.gateInActualDateTime?.toString()) : new Date();
        this.datepicker5?.bsValueChange.emit(dateValue5);

        this.modal.show();
    }

    save(): void {
        this.rowData.requestDate = moment(this._requestDate);
        this.rowData.devanningDate = moment(this._devanningDate);
        this.rowData.actualDevanningDate = moment(this._actualDevanningDate);
        this.rowData.gateInPlanTime = moment(this._gateInPlanTime);
        this.rowData.gateInActualDateTime = moment(this._gateInActualDateTime);

        this.saving = true;
        this._service.editContainerWH(this.rowData)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this._component.searchDatas();
                this.close();
            });
    }

    close(): void {
        this.modal.hide();
    }
}
