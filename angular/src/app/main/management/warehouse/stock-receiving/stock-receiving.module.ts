import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { StockReceivingRoutingModule } from './stock-receiving-routing.module';
import { StockReceivingComponent } from './stock-receiving.component';
import { ViewMaterialModule } from '@app/main/master/other/view-material/view-material.module';

@NgModule({
    imports: [
        AppSharedModule,
        StockReceivingRoutingModule,
        ViewMaterialModule
    ],
    declarations: [StockReceivingComponent]
})
export class StockReceivingModule { }
