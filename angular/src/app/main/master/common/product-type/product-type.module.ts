import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { ProductTypeRoutingModule } from './product-type-routing.module';
import { ProductTypeComponent } from './product-type.component';

@NgModule({
    imports: [
        AppSharedModule,
        ProductTypeRoutingModule
    ],
    declarations: [ProductTypeComponent]
})
export class ProductTypeModule { }
