import { Component, Injector, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { MasterMaterialDto, MasterMaterialServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from "ngx-bootstrap/modal";
import { finalize } from "rxjs/operators";

@Component({
    selector: 'editMaterialModal',
    templateUrl: './edit-material-modal.component.html'
})
export class EditMaterialModalComponent extends AppComponentBase {
    @ViewChild('editMaterialModal', {static: true}) modal: ModalDirective;

    rowData: MasterMaterialDto = new MasterMaterialDto();

    saving = false;

    isEdit: boolean = false;
    header: string = '';

    constructor(
        private injector: Injector,
        private _service: MasterMaterialServiceProxy
    ) {
        super(injector);
    }

    ngOnInit(){}

    show(type, material?: MasterMaterialDto): void {
        if(type == 'Edit') this.isEdit = true;
        else this.isEdit = false;
        this.header = type;
        this.rowData = material;
        this.modal.show();
    }

    save(): void {
        this.saving = true;
        this._service.editInfoMaterial(this.rowData)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
            });
    }

    close(): void {
        this.modal.hide();
    }
}
