import { Component, OnInit, ElementRef, ViewChild, Injector, OnDestroy } from '@angular/core';
import { HostDashboardServiceProxy, GetEditionTenantStatisticsOutput, ProdOthersServiceProxy, GetDataDashboardQtyOut } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import * as _ from 'lodash';
import { WidgetComponentBase } from '../widget-component-base';

@Component({
    selector: 'app-widget-edition-statistics',
    templateUrl: './widget-edition-statistics.component.html',
    styleUrls: ['./widget-edition-statistics.component.css']
})
export class WidgetEditionStatisticsComponent extends WidgetComponentBase implements OnInit, OnDestroy {

    @ViewChild('EditionStatisticsChart', { static: true }) editionStatisticsChart: ElementRef;

    selectedDateRange: moment.Moment[] = [moment().add(-7, 'days').startOf('day'), moment().endOf('day')];

    editionStatisticsHasData = false;
    editionStatisticsData;
    selectedType = 'Cfc';
    type = 'IN';

    constructor(
        injector: Injector,
        private _hostDashboardServiceProxy: HostDashboardServiceProxy,
        private _other: ProdOthersServiceProxy) {
        super(injector);
    }

    ngOnInit(): void {
        this.subDateRangeFilter();
        this.runDelayed(this.showChart);
    }

    showChart = () => {
        // this._hostDashboardServiceProxy.getEditionTenantStatistics(this.selectedDateRange[0], this.selectedDateRange[1])
        //     .subscribe((editionTenantStatistics) => {
        //         this.editionStatisticsData = this.normalizeEditionStatisticsData(editionTenantStatistics);
        //         this.editionStatisticsHasData = _.filter(this.editionStatisticsData, data => data.value > 0).length > 0;
        //     });
        this._other.getDataForDashboardQtyOut(this.selectedType, this.type, moment(this.selectedDateRange[0]), moment(this.selectedDateRange[1]))
            .subscribe((editionTenantStatistics) => {
                this.editionStatisticsData = this.normalizeEditionStatisticsData(editionTenantStatistics);
                this.editionStatisticsHasData = _.filter(this.editionStatisticsData, data => data.value > 0).length > 0;
            });
    }

    // normalizeEditionStatisticsData(data: GetEditionTenantStatisticsOutput): Array<any> {
    //     if (!data || !data.editionStatistics || data.editionStatistics.length === 0) {
    //         return [];
    //     }

    //     const chartData = new Array(data.editionStatistics.length);

    //     for (let i = 0; i < data.editionStatistics.length; i++) {
    //         chartData[i] = {
    //             name: data.editionStatistics[i].label,
    //             value: data.editionStatistics[i].value
    //         };
    //     }

    //     return chartData;
    // }

    normalizeEditionStatisticsData(data: GetDataDashboardQtyOut[]): Array<any> {
        if (!data || data.length === 0) {
            return [];
        }

        const chartData = new Array(data.length);

        for (let i = 0; i < data.length; i++) {
            chartData[i] = {
                name: data[i].label,
                value: data[i].qtyOut
            };
        }

        return chartData;
    }

    onDateRangeFilterChange = (dateRange) => {
        if (!dateRange || dateRange.length !== 2 || (this.selectedDateRange[0] === dateRange[0] && this.selectedDateRange[1] === dateRange[1])) {
            return;
        }

        this.selectedDateRange[0] = dateRange[0];
        this.selectedDateRange[1] = dateRange[1];
        this.runDelayed(this.showChart);
    }

    typeChange(interval) {
        if (this.selectedType === interval) {
            return;
        }

        this.selectedType = interval;
        this.showChart();
    }

    changeType(type) {
        this.type = type;
        this.showChart();
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
