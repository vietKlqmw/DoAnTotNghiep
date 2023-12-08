import { Component, Injector, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { ProdCustomsDeclareDto, ProdCustomsDeclareServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from "ngx-bootstrap/modal";
import { finalize } from "rxjs/operators";
import { BsDatepickerDirective } from "ngx-bootstrap/datepicker";
import * as moment from 'moment';
import { CustomsDeclareComponent } from "./customs-declare.component";

@Component({
    selector: 'editModal',
    templateUrl: './edit-customs-declare-modal.component.html'
})
export class EditCustomsDeclareModalComponent extends AppComponentBase {
    @ViewChild('editModal', { static: true }) modal: ModalDirective;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    rowData: ProdCustomsDeclareDto = new ProdCustomsDeclareDto();
    saving = false;
    _billDate: any;
    _declareDate: any;
    list = [{ value: '', label: '' }];

    constructor(
        private injector: Injector,
        private _service: ProdCustomsDeclareServiceProxy,
        private _component: CustomsDeclareComponent
    ) {
        super(injector);
    }

    ngOnInit() { }

    show(material: ProdCustomsDeclareDto): void {
        this.rowData = material;

        const dateValue = this.rowData.declareDate ? new Date(this.rowData.declareDate?.toString()) : new Date();
        this.datepicker?.bsValueChange.emit(dateValue);

        this.modal.show();
    }

    save(): void {
        this.rowData.declareDate = this._declareDate ? moment(this._declareDate) : undefined;

        this.saving = true;
        this._service.updateCustomsDeclare(this.rowData)
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
