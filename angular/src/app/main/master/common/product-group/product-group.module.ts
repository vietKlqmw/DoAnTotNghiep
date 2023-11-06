import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { ProductGroupRoutingModule } from './product-group-routing.module';
import { ProductGroupComponent } from './product-group.component';

@NgModule({
    imports: [
        AppSharedModule,
        ProductGroupRoutingModule
    ],
    declarations: [ProductGroupComponent]
})
export class ProductGroupModule { }
