import { NgModule } from '@angular/core';
import { CommonDeclareModule } from '@app/shared/common-declare.module';
import { ViewMaterialComponent } from './view-material.component';

@NgModule({
    imports: [
        CommonDeclareModule
    ],
    declarations: [
        ViewMaterialComponent
    ],
    exports: [
        ViewMaterialComponent
    ]
})
export class ViewMaterialModule { }
