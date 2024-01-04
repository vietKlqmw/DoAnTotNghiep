import { Component, Injector, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { MasterStorageLocationDto, MasterStorageLocationServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from "ngx-bootstrap/modal";
import { finalize } from "rxjs/operators";
import { StorageLocationComponent } from "./storage-location.component";

@Component({
    selector: 'editModal',
    templateUrl: './edit-storage-location-modal.component.html'
})
export class EditWarehouseModalComponent extends AppComponentBase {
    @ViewChild('editModal', { static: true }) modal: ModalDirective;

    rowData: MasterStorageLocationDto = new MasterStorageLocationDto();
    saving = false;
    isEdit: boolean = false;
    header: string = '';
    _address: string = '';
    _category: string = '';
    _type: string = '';
    _maxStock = 0;
    list = [
        { value: '', label: "" },
        { value: 'Miền Bắc', label: "Miền Bắc" },
        { value: 'Miền Trung', label: "Miền Trung" },
        { value: 'Miền Nam', label: "Miền Nam" }
    ];

    constructor(
        private injector: Injector,
        private _service: MasterStorageLocationServiceProxy,
        private _component: StorageLocationComponent
    ) {
        super(injector);
    }

    ngOnInit() { }

    show(type, material: MasterStorageLocationDto): void {
        this._address = '';
        this._category = 'Raw Material';
        this._maxStock = 0;
        this._type = '';
        this.header = type;
        if (type == 'Edit') {
            this.rowData = material;
            this._address = this.rowData.addressLanguageVn;
            this._category = this.rowData.category;
            this._maxStock = this.rowData.maxStock;
            this._type = 'Miền Bắc';
        }
        else if (type == 'Add') {
            this.rowData = new MasterStorageLocationDto();
        }

        this.modal.show();
    }

    save(): void {
        if (this._address == null || this._address == '') {
            this.notify.warn('Address is Required!');
            return;
        }
        if (this._category == null || this._category == '') {
            this.notify.warn('Category is Required!');
            return;
        }
        if (this._type == null || this._type == '') {
            this.notify.warn('Transport is Required!');
            return;
        }
        if (this._maxStock == undefined || this._maxStock == 0) {
            this.notify.warn('MaxStock is Required!');
            return;
        }
        if (this.rowData.inventory > this._maxStock) {
            this.notify.warn('MaxStock cannot less than Inventory!');
            return;
        }

        this.rowData.addressLanguageVn = this._address;
        this.rowData.category = this._category;
        this.rowData.type = this._type;
        this.rowData.maxStock = Number(this._maxStock);

        this.saving = true;
        this._service.editWH(this.rowData)
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
