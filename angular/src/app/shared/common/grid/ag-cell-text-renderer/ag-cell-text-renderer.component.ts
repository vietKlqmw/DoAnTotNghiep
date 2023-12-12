import { ICellRendererAngularComp } from '@ag-grid-community/angular';
import { Component } from '@angular/core';

@Component({
    selector: 'ag-cell-text-renderer',
    templateUrl: './ag-cell-text-renderer.component.html',
    styleUrls: ['./ag-cell-text-renderer.component.less']
})
export class AgCellTextRendererComponent implements ICellRendererAngularComp {

    public params: any;
    field: string;

    textCellDef: {
        text: string | Function, // Text display text
        iconName: string, // 'fa fa-pencil'
        className: string | Function,
        disabled: boolean | Function, // true => disable button, false => enable button
        message: string,
        function: Function,
    };

    constructor(
    ) {
    }

    agInit(params) {
        this.params = params;
        this.field = this.params.colDef.field;
        this.textCellDef = this.params.colDef.textCellDef;
    }

    get _Text() {
        if (typeof this.textCellDef.text === 'function') {
            return this.textCellDef.text(this.params);
        }
        return this.textCellDef.text
    }


    get _ClassName() {
        if (typeof this.textCellDef.className === 'function') {
            return this.textCellDef.className(this.params);
        }
        return this.textCellDef.text
    }


    get _DisableText() {
        // Execute if type is function
        if (typeof this.textCellDef.disabled === 'function') {
            return this.textCellDef.disabled(this.params);
        } else {
            return this.textCellDef.disabled;
        }
    }



    customFunction() {
        // Execute function when button is clicked
        //   if (this.disableText) {
        //     this.toastService.openWarningToast(this.buttonDef.message || 'Dữ liệu không đủ điều kiện để thực hiện thao tác');
        //     return;
        //   }
        return;
        this.textCellDef.function(this.params);
    }

    refresh(): boolean {
        return false;
    }
}
