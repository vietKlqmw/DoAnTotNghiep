import { Component, Injector, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { MasterPartListDto, MasterPartListServiceProxy, ProdOthersServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from "ngx-bootstrap/modal";
import { finalize } from "rxjs/operators";
import { BsDatepickerDirective } from "ngx-bootstrap/datepicker";
import * as moment from 'moment';
import { PartListComponent } from "./part-list.component";

@Component({
    selector: 'editPartListModal',
    templateUrl: './edit-part-list-modal.component.html'
})
export class EditPartListModalComponent extends AppComponentBase {
    @ViewChild('editPartListModal', { static: true }) modal: ModalDirective;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('datepicker2', { static: false }) datepicker2!: BsDatepickerDirective;

    rowData: MasterPartListDto = new MasterPartListDto();

    saving = false;

    isEdit: boolean = false;
    header: string = '';
    _startProductionMonth: any;
    _endProductionMonth: any;
    listSupplier = [{ label: '', value: '' }];
    listCfc = [{ label: '', value: '' }];
    listMaterialUsage = [{ label: '', value: undefined }];

    constructor(
        private injector: Injector,
        private _service: MasterPartListServiceProxy,
        private _other: ProdOthersServiceProxy,
        private _component: PartListComponent
    ) {
        super(injector);
    }

    ngOnInit() {
        this._other.getListSupplier().subscribe(result => {
            result.forEach(e => {
                this.listSupplier.push({ label: e.supplierName, value: e.supplierNo })
            })
        });

        this._other.getListCarfamilyCode().subscribe(result => {
            result.forEach(e => {
                this.listCfc.push({ label: e.name, value: e.code })
            })
        });

        this._other.getListMaterialUsage().subscribe(result => {
            result.forEach(e => {
                this.listMaterialUsage.push({ label: e.materialCode, value: e.materialId })
            })
        });
    }

    show(type, material?: MasterPartListDto): void {
        if (type == 'Edit') this.isEdit = true;
        else this.isEdit = false;
        this.header = type;
        if (material) this.rowData = material;
        else this.rowData = new MasterPartListDto();

        const dateValue = this.rowData.startProductionMonth ? new Date(this.rowData.startProductionMonth?.toString()) : new Date();
        this.datepicker?.bsValueChange.emit(dateValue);
        const dateValue2 = this.rowData.endProductionMonth ? new Date(this.rowData.endProductionMonth?.toString()) : undefined;
        this.datepicker2?.bsValueChange.emit(dateValue2);

        this.modal.show();
    }

    save(): void {
        this.rowData.startProductionMonth = moment(this._startProductionMonth);
        this.rowData.endProductionMonth = this._endProductionMonth ? moment(this._endProductionMonth) : undefined;

        this.saving = true;
        this._service.editPartList(this.rowData)
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
