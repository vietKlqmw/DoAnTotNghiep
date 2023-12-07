import { DatePipe } from '@angular/common';
import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MasterPartListServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'listErrorImport',
    templateUrl: './list-error-import-part-list-modal.component.html'
})
export class ListErrorImportModalComponent extends AppComponentBase {
    @ViewChild('listErrorImport', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    data: any[] = [];
    viewColDefs: any;
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 1000000000,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    dataParams: GridParams | undefined;
    defaultColDef = {
        resizable: true,
        sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        filter: true,
        floatingFilter: true,
        suppressHorizontalScroll: true,
        textFormatter: function (r: any) {
            if (r == null) return null;
            return r.toLowerCase();
        },
        tooltip: (params) => params.value,
    };

    guid;

    constructor(injector: Injector,
        private _service: MasterPartListServiceProxy
    ) {
        super(injector);

        this.viewColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 60 },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', flex: 1 },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName', flex: 1 },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', flex: 1 },
            { headerName: this.l('Carfamily Code'), headerTooltip: this.l('Carfamily Code'), field: 'carfamilyCode', flex: 1 },
            {
                headerName: this.l('Start Production Month'), headerTooltip: this.l('Start Production Month'), field: 'startProductionMonth', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.startProductionMonth, 'dd/MM/yyyy')
            },
            {
                headerName: this.l('End Production Month'), headerTooltip: this.l('End Production Month'), field: 'endProductionMonth', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.endProductionMonth, 'dd/MM/yyyy')
            },
            { headerName: this.l('Remark'), headerTooltip: this.l('Remark'), field: 'remark', flex: 1 },
            { headerName: this.l('Error Description'), headerTooltip: this.l('Error Description'), field: 'errorDescription', flex: 1 }
        ];

        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }
    ngOnInit() { }

    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "stt") {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView() {

        setTimeout(() => {
            this.dataParams.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSizeAll();
        }, 200)
    }

    show(guid): void {
        this.guid = guid;
        this._service.getListErrorImport(guid)
            .subscribe((result) => {
                this.data = result.items ?? [];
                this.resetGridView();
            });

        this.modal.show();
    }

    callBackDataGrid(params: GridParams) {
        this.dataParams = params;
        this.resetGridView();
    }

    close(): void {
        this.modal.hide();
        this.modalClose.emit(null);
    }
}
