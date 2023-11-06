import { NgModule } from '@angular/core';
import { MaterialTypeComponent } from './material-type.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { MaterialTypeRoutingModule } from './material-type-routing.module';

@NgModule({
    imports: [
        AppSharedModule,
        MaterialTypeRoutingModule
    ],
    declarations: [MaterialTypeComponent]
})
export class MaterialTypeModule { }
