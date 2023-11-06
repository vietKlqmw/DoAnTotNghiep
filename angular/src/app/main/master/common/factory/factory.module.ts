import { NgModule } from '@angular/core';
import { FactoryComponent } from './factory.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { FactoryRoutingModule } from './factory-routing.module';

@NgModule({
    imports: [
        AppSharedModule,
        FactoryRoutingModule
    ],
    declarations: [FactoryComponent]
})
export class FactoryModule { }
