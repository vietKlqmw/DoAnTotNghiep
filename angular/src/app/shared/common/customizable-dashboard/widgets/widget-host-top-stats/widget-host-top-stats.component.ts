import { Component, Injector, OnInit, OnDestroy } from '@angular/core';
import { GetDataDashboardTop, HostDashboardServiceProxy, ProdOthersServiceProxy, TopStatsData } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import { WidgetComponentBase } from '../widget-component-base';

@Component({
    selector: 'app-widget-host-top-stats',
    templateUrl: './widget-host-top-stats.component.html',
    styleUrls: ['./widget-host-top-stats.component.css']
})
export class WidgetHostTopStatsComponent extends WidgetComponentBase implements OnInit, OnDestroy {

    selectedDateRange: moment.Moment[] = [moment().add(-7, 'days').startOf('day'), moment().endOf('day')];
    loading = true;
    //topStatsData: TopStatsData;
    topStatsData: GetDataDashboardTop;
    selectedWarehouse = 'A1';

    constructor(
        injector: Injector,
        private _hostDashboardServiceProxy: HostDashboardServiceProxy,
        private _other: ProdOthersServiceProxy
        ) {

        super(injector);
    }

    ngOnInit(): void {
        this.subDateRangeFilter();
        this.runDelayed(this.loadHostTopStatsData);
    }

    loadHostTopStatsData = () => {
        // this._hostDashboardServiceProxy.getTopStatsData(this.selectedDateRange[0], this.selectedDateRange[1]).subscribe((data) => {
        //     this.topStatsData = data;
        //     this.loading = false;
        // });
        this._other.getDataForDashboardTop(this.selectedWarehouse).subscribe((data) => {
            this.topStatsData = data;
            this.loading = false;
        });
    }

    onDateRangeFilterChange = (dateRange) => {
        if (!dateRange || dateRange.length !== 2 || (this.selectedDateRange[0] === dateRange[0] && this.selectedDateRange[1] === dateRange[1])) {
            return;
        }

        this.selectedDateRange[0] = dateRange[0];
        this.selectedDateRange[1] = dateRange[1];
        this.runDelayed(this.loadHostTopStatsData);
    }

    warehouseChange(interval) {
        if (this.selectedWarehouse === interval) {
            return;
        }

        this.selectedWarehouse = interval;
        this.loadHostTopStatsData();
    }

    subDateRangeFilter() {
        abp.event.on('app.dashboardFilters.dateRangePicker.onDateChange', this.onDateRangeFilterChange);
    }

    unSubDateRangeFilter() {
        abp.event.off('app.dashboardFilters.dateRangePicker.onDateChange', this.onDateRangeFilterChange);
    }

    ngOnDestroy(): void {
        this.unSubDateRangeFilter();
    }
}
