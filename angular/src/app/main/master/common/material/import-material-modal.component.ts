
import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MasterMaterialServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FileUpload } from 'primeng/fileupload';
import { finalize } from 'rxjs/operators';
import { MaterialComponent } from './material.component';

@Component({

    selector: 'import-material',
    templateUrl: './import-material-modal.component.html',
    styleUrls: ['../../../import-modal.less'],
})
export class ImportMaterialComponent extends AppComponentBase {
    @ViewChild('imgInput', { static: false }) InputVar: ElementRef;
    @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;
    @ViewChild('importExcelModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    disabled = false;
    vissbleFileName;
    vissibleProgess;
    vissbleInputName;
    fileName: string = '';
    isLoading: boolean = false;
    formData: FormData = new FormData();
    uploadUrl: string;

    constructor(
        injector: Injector,
        private _httpClient: HttpClient,
        private _service: MasterMaterialServiceProxy,
        private _component: MaterialComponent
    ) {
        super(injector);
        this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/Product/ImportMaterialMasterFromExcel';
        { }
    }
    ngOnInit() {

    }

    show(): void {
        this.progressBarHidden();
        this.fileName = '';
        this.disabled = false;
        this.modal.show();
    }

    close(): void {
        this.InputVar.nativeElement.value = '';
        this.fileName = '';
        this.formData = new FormData();
        let viewName = document.getElementById('viewNameFileImport');
        if (viewName != null) {
            viewName.innerHTML = '';
        }
        this.modal.hide();
        this.modalClose.emit(null);
        this.isLoading = false;
    }

    onUpload(data: { target: { files: Array<any> } }): void {
        if (data?.target?.files.length > 0) {
            this.formData = new FormData();
            const formData: FormData = new FormData();
            const file = data?.target?.files[0];
            this.fileName = file?.name;
            formData.append('file', file, file.name);
            this.formData = formData;
            this.disabled = false;
            this.vissbleInputName = 'vissible';
            let viewName = document.getElementById("viewNameFileImport");
            if (viewName != null) {
                viewName.innerHTML = this.fileName.toString();
            }
        }
    }


    upload() {
        if (this.fileName != '') {
            this.progressBarVisible();
            this.disabled = true;
            this._httpClient
                .post<any>(this.uploadUrl, this.formData)
                .pipe(finalize(() => {
                    this.excelFileUpload?.clear();
                }))
                .subscribe(response => {
                    if (response.success) {
                        if (response.result.material.length == 0) {
                            this.notify.error('Dữ liệu không hợp lệ!');
                            this.progressBarHidden();
                            this.close();
                        }
                        else {
                            this.notify.success('Vào check db đi babe!');
                            //this.mergeData(response.result.material[0].guid);
                        }
                    }
                    else if (response.error) {
                        this.notify.error('Dữ liệu không hợp lệ!');
                        this.progressBarHidden();
                        this.close();
                    }
                },
                    error => {
                        this.notify.error('File không hợp lệ!');
                        this.progressBarHidden();
                        this.close();
                    });
        }
        else {
            this.notify.warn('Vui lòng chọn file');
        }
    }

    mergeData(guid) {
        // this.isLoading = true;
        // this._service.mergeDataSmqdOrderNormal(guid)
        //     .pipe(
        //         finalize(() => {
        //             this.isLoading = false;
        //         })
        //     )
        //     .subscribe(() => {
        //         this.showMessImport(guid);
        //     });
    }

    showMessImport(guid) {
        // this._service.getMessageErrorImport(guid)
        //     .subscribe((result) => {
        //         if (result.items.length > 0) {
        //             this.errorListModal.show(guid)
        //         }
        //         else {
        //             this.notify.info('Lưu thành công');
        //             this.modal.hide();
        //             this.modalClose.emit(null);
        //             this._component.searchDatas();
        //             this.disabled = false;
        //             this.close();
        //         }
        //     });
    }


    progressBarVisible() {
        this.vissbleInputName = 'vissible';
        this.vissibleProgess = 'vissible active';

    }
    progressBarHidden() {
        this.vissibleProgess = '';
        this.vissbleInputName = '';
    }

}
