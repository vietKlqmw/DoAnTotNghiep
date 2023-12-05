import { DatePipe } from '@angular/common';
import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MasterMaterialServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'listErrorImportMaterial',
    templateUrl: './list-error-import-material-modal.component.html'
})
export class ListErrorImportMaterialModalComponent extends AppComponentBase {
    @ViewChild('listErrorImportMaterial', { static: true }) modal: ModalDirective;
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
        private _service: MasterMaterialServiceProxy
    ) {
        super(injector);

        this.viewColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 100, pinned: 'left' },
            {
                headerName: this.l('Material Type'),
                headerTooltip: this.l('Material Type'),
                field: 'materialType',
                flex: 1,
                pinned: 'left'
            },
            {
                headerName: this.l('Material Code'),
                headerTooltip: this.l('Material Code'),
                field: 'materialCode',
                flex: 1,
                pinned: 'left'
            },
            {
                headerName: this.l('Description'),
                headerTooltip: this.l('Description'),
                field: 'description',
                flex: 1
            },
            {
                headerName: this.l('Material Group'),
                headerTooltip: this.l('Material Group'),
                field: 'materialGroup',
                flex: 1
            },
            {
                headerName: this.l('Base Unit Of Measure'),
                headerTooltip: this.l('Base Unit Of Measure'),
                field: 'baseUnitOfMeasure',
                flex: 1
            },
            {
                headerName: this.l('Storage Location'),
                headerTooltip: this.l('Storage Location'),
                field: 'storageLocation',
                flex: 1
            },
            {
                headerName: this.l('Production Type'),
                headerTooltip: this.l('Production Type'),
                field: 'productionType',
                flex: 1
            },
            {
                headerName: this.l('Standard Price'),
                headerTooltip: this.l('Standard Price'),
                field: 'standardPrice',
                flex: 1
            },
            {
                headerName: this.l('Moving Price'),
                headerTooltip: this.l('Moving Price'),
                field: 'movingPrice',
                flex: 1
            },
            {
                headerName: this.l('Material Origin'),
                headerTooltip: this.l('Material Origin'),
                field: 'materialOrigin',
                cellClass: ['text-center'],
                flex: 1
            },
            {
                headerName: this.l('Effective Date From'),
                headerTooltip: this.l('Effective Date From'),
                field: 'effectiveDateFrom',
                valueGetter: (params) => this.pipe.transform(params.data?.effectiveDateFrom, 'dd/MM/yyyy'),
                flex: 1
            },
            {
                headerName: this.l('Effective Date To'),
                headerTooltip: this.l('Effective Date To'),
                field: 'effectiveDateTo',
                valueGetter: (params) => this.pipe.transform(params.data?.effectiveDateTo, 'dd/MM/yyyy'),
                flex: 1
            },
            {
                headerName: this.l('Error Description'),
                headerTooltip: this.l('Error Description'),
                field: 'errorDescription',
                flex: 1,
            }
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
