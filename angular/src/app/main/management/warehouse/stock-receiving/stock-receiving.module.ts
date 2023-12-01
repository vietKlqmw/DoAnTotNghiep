import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { StockReceivingRoutingModule } from './stock-receiving-routing.module';
import { StockReceivingComponent } from './stock-receiving.component';

@NgModule({
    imports: [
        AppSharedModule,
        StockReceivingRoutingModule
    ],
    declarations: [StockReceivingComponent]
})
export class StockReceivingModule { }
