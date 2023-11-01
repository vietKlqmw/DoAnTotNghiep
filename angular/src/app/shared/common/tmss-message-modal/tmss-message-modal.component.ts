import { Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { ModalDirective } from "ngx-bootstrap/modal";

@Component({
    selector: "tmss-message-modal",
    templateUrl: "./tmss-message-modal.component.html",
    styleUrls: ["./tmss-message-modal.component.less"],
})
export class TmssMessageModalComponent extends AppComponentBase {
    @ViewChild('modal', { static: false }) modal: ModalDirective;
    @Input() modalClass: string = 'tmss-modal-md';
    @Input() headerText: string = 'Thông báo';
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
    messageWarning: any;

    constructor(injector: Injector) {
        super(injector);
    }

    ngOnInit() {}

    show(messageWarning?: any) {
        this.messageWarning = messageWarning;
        this.modal.show();
    }
    onCancelBtn() {
        this.modal.hide();
        this.modalClose.emit();
    }
}
