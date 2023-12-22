import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { OrderPartRoutingModule } from './order-part-routing.module';
import { OrderPartComponent } from './order-part.component';

@NgModule({
    imports: [
        AppSharedModule,
        OrderPartRoutingModule
    ],
    declarations: [
        OrderPartComponent,
        // EditOrderPartModalComponent
    ]
})
export class OrderPartModule { }
