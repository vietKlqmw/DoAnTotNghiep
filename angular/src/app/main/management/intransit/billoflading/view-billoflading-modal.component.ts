import { Component, Injector, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { ProdOthersServiceProxy, ProdShipmentDto, ProdShipmentServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from "ngx-bootstrap/modal";
import { DataFormatService } from "@app/shared/common/services/data-format.service";

@Component({
    selector: 'viewModal',
    templateUrl: './view-billoflading-modal.component.html'
})
export class ViewBillOfLadingModalComponent extends AppComponentBase {
    @ViewChild('viewModal', { static: true }) modal: ModalDirective;

    rowData: ProdShipmentDto = new ProdShipmentDto();
    saving = false;

    constructor(
        private injector: Injector,
        private _service: ProdShipmentServiceProxy,
        private _other: ProdOthersServiceProxy,
        private _fm: DataFormatService
    ) {
        super(injector);
    }

    ngOnInit() { }

    show(material?: ProdShipmentDto): void {
        this.rowData = material;

        this.modal.show();
    }

    save(): void {
    }

    close(): void {
        this.modal.hide();
    }
}
