import { Component, Injector, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { MasterSupplierListDto, MasterSupplierListServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from "ngx-bootstrap/modal";
import { finalize } from "rxjs/operators";
import { SupplierListComponent } from "./supplier-list.component";

@Component({
    selector: 'editModal',
    templateUrl: './edit-supplier-modal.component.html'
})
export class EditSupplierModalComponent extends AppComponentBase {
    @ViewChild('editModal', { static: true }) modal: ModalDirective;

    rowData: MasterSupplierListDto = new MasterSupplierListDto();
    saving = false;
    isEdit: boolean = false;
    header: string = '';
    _supplierNo: string = '';
    _supplierName: string = '';
    _remarks: string = '';
    _supplierType: string = '';
    _supplierNameVn: string = '';
    _exporter: string = '';

    constructor(
        private injector: Injector,
        private _service: MasterSupplierListServiceProxy,
        private _component: SupplierListComponent
    ) {
        super(injector);
    }

    ngOnInit() { }

    show(type, material: MasterSupplierListDto): void {
        this._supplierNo = '';
        this._supplierName = '';
        this._remarks = '';
        this._supplierType = '';
        this._supplierNameVn = '';
        this._exporter = '';
        this.header = type;
        if (type == 'Edit') {
            this.rowData = material;
            this._supplierNo = this.rowData.supplierNo;
            this._supplierName = this.rowData.supplierName;
            this._remarks = this.rowData.remarks;
            this._supplierType = this.rowData.supplierType;
            this._supplierNameVn = this.rowData.supplierNameVn;
            this._exporter = this.rowData.exporter;
        }
        else if (type == 'Add') {
            this.rowData = new MasterSupplierListDto();
        }

        this.modal.show();
    }

    save(): void {
        if (this._supplierNo == null || this._supplierNo == '') {
            this.notify.warn('SupplierNo is Required!');
            return;
        }
        if (this._supplierName == null || this._supplierName == '') {
            this.notify.warn('SupplierName is Required!');
            return;
        }
        if (this._supplierNameVn == null || this._supplierNameVn == '') {
            this.notify.warn('SupplierNameVn is Required!');
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
