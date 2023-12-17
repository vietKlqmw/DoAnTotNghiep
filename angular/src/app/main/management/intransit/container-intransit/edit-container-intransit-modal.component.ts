import { Component, Injector, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { ProdOthersServiceProxy, ProdContainerIntransitDto, ProdContainerIntransitServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from "ngx-bootstrap/modal";
import { finalize } from "rxjs/operators";
import { ContainerIntransitComponent } from "./container-intransit.component";
import { formatDate } from "@angular/common";

@Component({
    selector: 'editModal',
    templateUrl: './edit-container-intransit-modal.component.html'
})
export class EditContainerIntransitModalComponent extends AppComponentBase {
    @ViewChild('editModal', { static: true }) modal: ModalDirective;

    rowData: ProdContainerIntransitDto = new ProdContainerIntransitDto();
    shipmentData = [];
    saving = false;

    isEdit: boolean = false;
    header: string = '';
    listPart = [{ label: '', value: undefined, supplier: '' }];

    constructor(
        private injector: Injector,
        private _service: ProdContainerIntransitServiceProxy,
        private _other: ProdOthersServiceProxy,
        private _component: ContainerIntransitComponent
    ) {
        super(injector);
    }

    ngOnInit() {
        this._other.getListPart().subscribe(result => {
            this.shipmentData = result;
            result.forEach(e => {
                this.listPart.push({ label: e.partName, value: e.partId, supplier: e.supplierNo })
            })
        });

    }

    show(type, material?: ProdContainerIntransitDto): void {
        if (type == 'Edit') this.isEdit = true;
        else this.isEdit = false;
        this.header = type;
        if (material) this.rowData = material;
        else this.rowData = new ProdContainerIntransitDto();

        if (type != 'Edit') this.rowData.containerNo = 'CONT' + formatDate(new Date(), 'yyMMddmmss', 'en-US');

        this.modal.show();

    }

    save(): void {
        if (this.rowData.partListId == null) {
            this.notify.warn('PartListId is Required!');
            return;
        }
        if (this.rowData.usageQty == null) {
            this.notify.warn('Usage Qty is Required!');
            return;
        }
        this.saving = true;
        this._service.editContainerIntransit(this.rowData)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this._component.searchDatas();
                this.close();
            });
    }

    changePart(ev) {
       this.rowData.supplierNo = this.listPart.filter(e => e.value == ev)[0].supplier;
    }

    close(): void {
        this.modal.hide();
    }
}
