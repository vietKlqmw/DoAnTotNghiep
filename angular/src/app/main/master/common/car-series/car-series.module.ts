import { NgModule } from '@angular/core';
import { CarSeriesComponent } from './car-series.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { CarSeriesRoutingModule } from './car-series-routing.module';

@NgModule({
    imports: [
        AppSharedModule,
        CarSeriesRoutingModule
    ],
    declarations: [CarSeriesComponent]
})
export class CarSeriesModule { }
