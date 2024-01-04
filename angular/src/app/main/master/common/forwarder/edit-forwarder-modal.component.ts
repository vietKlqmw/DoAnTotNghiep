import { Component, Injector, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { MasterForwarderDto, MasterSupplierListServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from "ngx-bootstrap/modal";
import { finalize } from "rxjs/operators";
import { ForwarderComponent } from "./forwarder.component";

@Component({
    selector: 'editModal',
    templateUrl: './edit-forwarder-modal.component.html'
})
export class EditForwarderModalComponent extends AppComponentBase {
    @ViewChild('editModal', { static: true }) modal: ModalDirective;

    rowData: MasterForwarderDto = new MasterForwarderDto();
    saving = false;
    isEdit: boolean = false;
    header: string = '';
    _code: string = '';
    _name: string = '';
    _supplierNo: string = '';

    constructor(
        private injector: Injector,
        private _service: MasterSupplierListServiceProxy,
        private _component: ForwarderComponent
    ) {
        super(injector);
    }

    ngOnInit() { }

    show(type, material: MasterForwarderDto): void {
        this._code = '';
        this._name = '';
        this._supplierNo = '';
        this.header = type;
        if (type == 'Edit') {
            this.rowData = material;
            this._code = this.rowData.code;
            this._name = this.rowData.name;
            this._supplierNo = this.rowData.supplierNo;
        }
        else if (type == 'Add') {
            this.rowData = new MasterForwarderDto();
        }

        this.modal.show();
    }

    save(): void {
        if (this._code == null || this._code == '') {
            this.notify.warn('ForwarderCode is Required!');
            return;
        }
        if (this._name == null || this._name == '') {
            this.notify.warn('ForwarderName is Required!');
            return;
        }
        if (this._supplierNo == null || this._supplierNo == '') {
            this.notify.warn('Supplier No is Required!');
            return;
        }

        // this.saving = true;
        // this._service.editWH(this.rowData)
        //     .pipe(finalize(() => { this.saving = false; }))
        //     .subscribe(() => {
        //         this.notify.info(this.l('SavedSuccessfully'));
        //         this._component.searchDatas();
        //         this.close();
        //     });
    }

    close(): void {
        this.modal.hide();
    }
}
