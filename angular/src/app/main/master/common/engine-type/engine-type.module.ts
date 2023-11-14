import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { EngineTypeRoutingModule } from './engine-type-routing.module';
import { EngineTypeComponent } from './engine-type.component';

@NgModule({
    imports: [
        AppSharedModule,
        EngineTypeRoutingModule
    ],
    declarations: [EngineTypeComponent]
})
export class EngineTypeModule { }
