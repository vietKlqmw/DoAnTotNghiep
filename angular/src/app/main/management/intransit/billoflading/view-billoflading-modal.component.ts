import { Component, Injector, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { ProdBillOfLadingDto, ProdBillOfLadingServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from "ngx-bootstrap/modal";
import { formatDate } from "@angular/common";

@Component({
    selector: 'viewModal',
    templateUrl: './view-billoflading-modal.component.html'
})
export class ViewBillOfLadingModalComponent extends AppComponentBase {
    @ViewChild('viewModal', { static: true }) modal: ModalDirective;

    rowData: ProdBillOfLadingDto = new ProdBillOfLadingDto();
    saving = false;
    _billDate: any;
    _consignee = 'CT TNHH Toyota VN';

    constructor(
        private injector: Injector,
        private _service: ProdBillOfLadingServiceProxy,
    ) {
        super(injector);
    }

    ngOnInit() { }

    show(BillId): void {
        this._service.viewBillOfLading(BillId)
        .subscribe(result => {
            this.rowData = result;
            this._billDate = this.rowData.billDate ? formatDate(this.rowData.billDate?.toString(), 'dd/MM/yyyy', 'en-US') : undefined;
        })

        this.modal.show();
    }

    save(): void {
    }

    close(): void {
        this.modal.hide();
    }
}
