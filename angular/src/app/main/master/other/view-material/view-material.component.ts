import { formatDate } from "@angular/common";
import { Component, Injector, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { MasterMaterialDto, ProdOthersServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from "ngx-bootstrap/modal";

@Component({
    selector: 'viewMaterial',
    templateUrl: './view-material.component.html'
})
export class ViewMaterialComponent extends AppComponentBase {
    @ViewChild('viewMaterial', { static: true }) modal: ModalDirective;

    rowData: MasterMaterialDto = new MasterMaterialDto();

    _effectiveDateFrom: any;
    _effectiveDateTo: any;

    constructor(
        injector: Injector,
        private _other: ProdOthersServiceProxy
    ) {
        super(injector);
    }

    show(materialId?: number): void {
        this._other.getDataMaterialbyId(materialId)
        .subscribe(result => {
            this.rowData = result[0];
        })

        this._effectiveDateFrom = this.rowData.effectiveDateFrom ? formatDate(new Date(this.rowData.effectiveDateFrom.toString()), 'dd/MM/yyyy', 'en-US') : null;
        this._effectiveDateTo = this.rowData.effectiveDateTo ? formatDate(new Date(this.rowData.effectiveDateTo.toString()), 'dd/MM/yyyy', 'en-US') : null;

        this.modal.show();
    }

    close(): void {
        this.modal.hide();
    }
}
