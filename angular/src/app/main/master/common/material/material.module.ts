import { NgModule } from '@angular/core';
import { MaterialComponent } from './material.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { MaterialRoutingModule } from './material-routing.module';

@NgModule({
    imports: [
        AppSharedModule,
        MaterialRoutingModule
    ],
    declarations: [MaterialComponent]
})
export class MaterialModule { }
