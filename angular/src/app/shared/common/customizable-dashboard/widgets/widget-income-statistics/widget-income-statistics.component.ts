import { Component, OnInit, OnDestroy, Injector } from '@angular/core';
import { ChartDateInterval, HostDashboardServiceProxy, ProdOthersServiceProxy } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import * as _ from 'lodash';
import { WidgetComponentBase } from '../widget-component-base';

@Component({
    selector: 'app-widget-income-statistics',
    templateUrl: './widget-income-statistics.component.html',
    styleUrls: ['./widget-income-statistics.component.css']
})
export class WidgetIncomeStatisticsComponent extends WidgetComponentBase implements OnInit, OnDestroy {

    selectedIncomeStatisticsDateInterval = ChartDateInterval.Daily;
    loadingIncomeStatistics = true;

    //selectedDateRange: moment.Moment[] = [moment().add(-7, 'days').startOf('day'), moment().endOf('day')];
    selectedDateRange: moment.Moment[] = [moment().add(-30, 'days').startOf('day'), moment().endOf('day')];
    incomeStatisticsData: any = [];
    incomeStatisticsHasData = false;
    appIncomeStatisticsDateInterval = ChartDateInterval;
    type = 'IN';

    constructor(injector: Injector,
        private _hostDashboardServiceProxy: HostDashboardServiceProxy,
        private _other: ProdOthersServiceProxy
    ) {
        super(injector);
    }

    ngOnInit() {
        this.subDateRangeFilter();
        this.runDelayed(this.loadIncomeStatisticsData);
    }

    incomeStatisticsDateIntervalChange(interval: number) {
        if (this.selectedIncomeStatisticsDateInterval === interval) {
            return;
        }

        this.selectedIncomeStatisticsDateInterval = interval;
        this.loadIncomeStatisticsData();
    }

    loadIncomeStatisticsData = () => {
        // this.loadingIncomeStatistics = true;
        // this._hostDashboardServiceProxy.getIncomeStatistics(
        //     this.selectedIncomeStatisticsDateInterval,
        //     moment(this.selectedDateRange[0]),
        //     moment(this.selectedDateRange[1]))
        //     .subscribe(result => {
        //         this.incomeStatisticsData = this.normalizeIncomeStatisticsData(result.incomeStatistics);
        //         this.incomeStatisticsHasData = _.filter(this.incomeStatisticsData[0].series, data => data.value > 0).length > 0;
        //         this.loadingIncomeStatistics = false;
        //     });
        this.loadingIncomeStatistics = true;
        this._other.getDataForDashboardInvoiceStatistics(
            moment(this.selectedDateRange[0]),
            moment(this.selectedDateRange[1]),
            this.type)
            .subscribe(result => {
                this.incomeStatisticsData = this.normalizeIncomeStatisticsData(result);
                this.incomeStatisticsHasData = this.incomeStatisticsData ? true : false;
                this.loadingIncomeStatistics = false;
            });
    }

    // normalizeIncomeStatisticsData(data): any {
    //     const chartData = [];
    //     for (let i = 0; i < data.length; i++) {
    //         chartData.push({
    //             'name': moment(moment(data[i].date).utc().valueOf()).format('L'),
    //             'value': data[i].amount
    //         });
    //     }

    //     return [{
    //         name: '',
    //         series: chartData
    //     }];
    // }

    normalizeIncomeStatisticsData(data): any {
        const chartDataA1 = [];
        const chartDataA2 = [];
        const chartDataB1 = [];
        const chartDataC1 = [];
        const chartDataC2 = [];
        for (let i = 0; i < data.length; i++) {
            if (this.type == 'OUT') {
                switch (data[i].warehouse) {
                    case 'A1':
                        chartDataA1.push({
                            'name': moment(moment(data[i].invoiceDate).utc().valueOf()).format('L'),
                            'value': data[i].amountOut
                        });
                        break;
                    case 'A2':
                        chartDataA2.push({
                            'name': moment(moment(data[i].invoiceDate).utc().valueOf()).format('L'),
                            'value': data[i].amountOut
                        });
                        break;
                    case 'B1':
                        chartDataB1.push({
                            'name': moment(moment(data[i].invoiceDate).utc().valueOf()).format('L'),
                            'value': data[i].amountOut
                        });
                        break;
                    case 'C1':
                        chartDataC1.push({
                            'name': moment(moment(data[i].invoiceDate).utc().valueOf()).format('L'),
                            'value': data[i].amountOut
                        });
                        break;
                    case 'C2':
                        chartDataC2.push({
                            'name': moment(moment(data[i].invoiceDate).utc().valueOf()).format('L'),
                            'value': data[i].amountOut
                        });
                        break;
                }
            }else{
                switch (data[i].warehouse) {
                    case 'A1':
                        chartDataA1.push({
                            'name': moment(moment(data[i].invoiceDate).utc().valueOf()).format('L'),
                            'value': data[i].cif + data[i].tax + data[i].vat
                        });
                        break;
                    case 'A2':
                        chartDataA2.push({
                            'name': moment(moment(data[i].invoiceDate).utc().valueOf()).format('L'),
                            'value': data[i].cif + data[i].tax + data[i].vat
                        });
                        break;
                    case 'B1':
                        chartDataB1.push({
                            'name': moment(moment(data[i].invoiceDate).utc().valueOf()).format('L'),
                            'value': data[i].cif + data[i].tax + data[i].vat
                        });
                        break;
                    case 'C1':
                        chartDataC1.push({
                            'name': moment(moment(data[i].invoiceDate).utc().valueOf()).format('L'),
                            'value': data[i].cif + data[i].tax + data[i].vat
                        });
                        break;
                    case 'C2':
                        chartDataC2.push({
                            'name': moment(moment(data[i].invoiceDate).utc().valueOf()).format('L'),
                            'value': data[i].cif + data[i].tax + data[i].vat
                        });
                        break;
                }
            }
        }

        return [
            {
                name: 'A1',
                series: chartDataA1
            },
            {
                name: 'A2',
                series: chartDataA2
            },
            {
                name: 'B1',
                series: chartDataB1
            },
            {
                name: 'C1',
                series: chartDataC1
            },
            {
                name: 'C2',
                series: chartDataC2
            }
        ];
    }

    onDateRangeFilterChange = (dateRange) => {
        if (!dateRange || dateRange.length !== 2 || (this.selectedDateRange[0] === dateRange[0] && this.selectedDateRange[1] === dateRange[1])) {
            return;
        }

        this.selectedDateRange[0] = dateRange[0];
        this.selectedDateRange[1] = dateRange[1];
        this.runDelayed(this.loadIncomeStatisticsData);
    }

    changeType(type) {
        this.type = type;
        this.loadIncomeStatisticsData();
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
