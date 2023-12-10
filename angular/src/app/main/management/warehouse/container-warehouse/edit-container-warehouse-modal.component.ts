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

    rowData: ProdContainerRentalWHPlanDto = new ProdContainerRentalWHPlanDto();

    saving = false;

    isEdit: boolean = false;
    header: string = '';
    _requestDate: any;
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

        this.modal.show();
    }

    save(): void {
        this.rowData.requestDate = moment(this._requestDate);

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
