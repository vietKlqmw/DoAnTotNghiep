import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { StockReceivingRoutingModule } from './stock-receiving-routing.module';
import { StockReceivingComponent } from './stock-receiving.component';
import { ViewMaterialModule } from '@app/main/master/other/view-material/view-material.module';
import { AddPurchaseOrderModalComponent } from './order-stock-receiving-modal.component';
import { AddGdnStockModalComponent } from './add-gdn-stock-modal.component';
import { UpdateOrderStockModalComponent } from './update-order-stock-modal.component';

@NgModule({
    imports: [
        AppSharedModule,
        StockReceivingRoutingModule,
        ViewMaterialModule
    ],
    declarations: [
        StockReceivingComponent,
        AddPurchaseOrderModalComponent,
        AddGdnStockModalComponent,
        UpdateOrderStockModalComponent
    ]
})
export class StockReceivingModule { }
