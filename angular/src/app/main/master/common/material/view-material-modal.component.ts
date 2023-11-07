import { Component, Injector, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { MasterMaterialDto } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from "ngx-bootstrap/modal";

@Component({
    selector: 'viewMaterialModal',
    templateUrl: './view-material-modal.component.html'
})
export class ViewMaterialModalComponent extends AppComponentBase {
    @ViewChild('viewMaterialModal', {static: true}) modal: ModalDirective;

    rowData: MasterMaterialDto = new MasterMaterialDto();

    constructor(
        injector: Injector
    ) {
        super(injector);
    }

    show(material?: MasterMaterialDto): void {
        this.rowData = material;
        this.modal.show();
    }

    close(): void {
        this.modal.hide();
    }
}
