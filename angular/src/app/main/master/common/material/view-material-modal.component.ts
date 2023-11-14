import { formatDate } from "@angular/common";
import { Component, Injector, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { MasterMaterialDto } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from "ngx-bootstrap/modal";

@Component({
    selector: 'viewMaterialModal',
    templateUrl: './view-material-modal.component.html'
})
export class ViewMaterialModalComponent extends AppComponentBase {
    @ViewChild('viewMaterialModal', { static: true }) modal: ModalDirective;

    rowData: MasterMaterialDto = new MasterMaterialDto();

    _effectiveDateFrom: any;
    _effectiveDateTo: any;

    constructor(
        injector: Injector
    ) {
        super(injector);
    }

    show(material?: MasterMaterialDto): void {
        this.rowData = material;

        this._effectiveDateFrom = this.rowData.effectiveDateFrom ? formatDate(new Date(this.rowData.effectiveDateFrom.toString()), 'dd/MM/yyyy', 'en-US') : null;
        this._effectiveDateTo = this.rowData.effectiveDateTo ? formatDate(new Date(this.rowData.effectiveDateTo.toString()), 'dd/MM/yyyy', 'en-US') : null;

        this.modal.show();
    }

    close(): void {
        this.modal.hide();
    }
}
