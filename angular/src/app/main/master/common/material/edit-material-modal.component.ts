import { Component, Injector, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { MasterMaterialDto, MasterMaterialServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from "ngx-bootstrap/modal";
import { finalize } from "rxjs/operators";
import { MaterialComponent } from "./material.component";
import { BsDatepickerDirective } from "ngx-bootstrap/datepicker";
import * as moment from 'moment';

@Component({
    selector: 'editMaterialModal',
    templateUrl: './edit-material-modal.component.html'
})
export class EditMaterialModalComponent extends AppComponentBase {
    @ViewChild('editMaterialModal', { static: true }) modal: ModalDirective;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('datepicker2', { static: false }) datepicker2!: BsDatepickerDirective;

    rowData: MasterMaterialDto = new MasterMaterialDto();

    saving = false;

    isEdit: boolean = false;
    header: string = '';
    _effectiveDateFrom: any;
    _effectiveDateTo: any;

    constructor(
        private injector: Injector,
        private _service: MasterMaterialServiceProxy,
        private _component: MaterialComponent
    ) {
        super(injector);
    }

    ngOnInit() { }

    show(type, material?: MasterMaterialDto): void {
        if (type == 'Edit') this.isEdit = true;
        else this.isEdit = false;
        this.header = type;
        if (material) this.rowData = material;
        else this.rowData = new MasterMaterialDto();

        const dateValue = this.rowData.effectiveDateFrom ? new Date(this.rowData.effectiveDateFrom?.toString()) : new Date();
        this.datepicker?.bsValueChange.emit(dateValue);
        const dateValue2 = this.rowData.effectiveDateTo ? new Date(this.rowData.effectiveDateTo?.toString()) : undefined;
        this.datepicker2?.bsValueChange.emit(dateValue2);

        this.modal.show();
    }

    save(): void {
        this.rowData.effectiveDateFrom = moment(this._effectiveDateFrom);
        this.rowData.effectiveDateTo = this._effectiveDateTo ? moment(this._effectiveDateTo) : undefined;

        this.saving = true;
        this._service.editInfoMaterial(this.rowData)
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
