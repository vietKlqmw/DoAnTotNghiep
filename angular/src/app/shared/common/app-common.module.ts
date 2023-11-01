import { CommonModule } from '@angular/common';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppLocalizationService } from '@app/shared/common/localization/app-localization.service';
import { AppNavigationService } from '@app/shared/layout/nav/app-navigation.service';
import { tmssCommonModule } from '@shared/common/common.module';
import { UtilsModule } from '@shared/utils/utils.module';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BsDatepickerModule, BsDatepickerConfig, BsDaterangepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { PaginatorModule } from 'primeng/paginator';
import { TableModule } from 'primeng/table';
import { AppAuthService } from './auth/app-auth.service';
import { AppRouteGuard } from './auth/auth-route-guard';
import { CommonLookupModalComponent } from './lookup/common-lookup-modal.component';
import { EntityTypeHistoryModalComponent } from './entityHistory/entity-type-history-modal.component';
import { EntityChangeDetailModalComponent } from './entityHistory/entity-change-detail-modal.component';
import { DateRangePickerInitialValueSetterDirective } from './timing/date-range-picker-initial-value.directive';
import { DatePickerInitialValueSetterDirective } from './timing/date-picker-initial-value.directive';
import { DateTimeService } from './timing/date-time.service';
import { TimeZoneComboComponent } from './timing/timezone-combo.component';
import { CustomizableDashboardComponent } from './customizable-dashboard/customizable-dashboard.component';
import { WidgetGeneralStatsComponent } from './customizable-dashboard/widgets/widget-general-stats/widget-general-stats.component';
import { DashboardViewConfigurationService } from './customizable-dashboard/dashboard-view-configuration.service';
import { GridsterModule } from 'angular-gridster2';
import { WidgetDailySalesComponent } from './customizable-dashboard/widgets/widget-daily-sales/widget-daily-sales.component';
import { WidgetEditionStatisticsComponent } from './customizable-dashboard/widgets/widget-edition-statistics/widget-edition-statistics.component';
import { WidgetHostTopStatsComponent } from './customizable-dashboard/widgets/widget-host-top-stats/widget-host-top-stats.component';
import { WidgetIncomeStatisticsComponent } from './customizable-dashboard/widgets/widget-income-statistics/widget-income-statistics.component';
import { WidgetMemberActivityComponent } from './customizable-dashboard/widgets/widget-member-activity/widget-member-activity.component';
import { WidgetProfitShareComponent } from './customizable-dashboard/widgets/widget-profit-share/widget-profit-share.component';
import { WidgetRecentTenantsComponent } from './customizable-dashboard/widgets/widget-recent-tenants/widget-recent-tenants.component';
import { WidgetRegionalStatsComponent } from './customizable-dashboard/widgets/widget-regional-stats/widget-regional-stats.component';
import { WidgetSalesSummaryComponent } from './customizable-dashboard/widgets/widget-sales-summary/widget-sales-summary.component';
import { WidgetSubscriptionExpiringTenantsComponent } from './customizable-dashboard/widgets/widget-subscription-expiring-tenants/widget-subscription-expiring-tenants.component';
import { WidgetTopStatsComponent } from './customizable-dashboard/widgets/widget-top-stats/widget-top-stats.component';
import { FilterDateRangePickerComponent } from './customizable-dashboard/filters/filter-date-range-picker/filter-date-range-picker.component';
import { AddWidgetModalComponent } from './customizable-dashboard/add-widget-modal/add-widget-modal.component';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { NgxBootstrapDatePickerConfigService } from 'assets/ngx-bootstrap/ngx-bootstrap-datepicker-config.service';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { CountoModule } from 'angular2-counto';
import { AppBsModalModule } from '@shared/common/appBsModal/app-bs-modal.module';
import { SingleLineStringInputTypeComponent } from './input-types/single-line-string-input-type/single-line-string-input-type.component';
import { ComboboxInputTypeComponent } from './input-types/combobox-input-type/combobox-input-type.component';
import { CheckboxInputTypeComponent } from './input-types/checkbox-input-type/checkbox-input-type.component';
import { MultipleSelectComboboxInputTypeComponent } from './input-types/multiple-select-combobox-input-type/multiple-select-combobox-input-type.component';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { AgGridModule } from '@ag-grid-community/angular';
import { InputMaskModule } from 'primeng';
import { FormValidationModule } from './form-validation/form-validation.module';
import { HttpClientModule } from '@angular/common/http';
import { GridTableComponent } from './ag-grid-custom/grid-table/grid-table.component';
import { SimpleAgGridComponent } from './ag-grid-custom/simple-ag-grid/simple-ag-grid.component';
import { GridPaginationComponent } from './ag-grid-custom/grid-pagination/grid-pagination.component';
import { TmssDatepickerComponent } from './input-types/tmss-datepicker/tmss-datepicker.component';
import { TmssDatepickerrangerComponent } from './input-types/tmss-datepickerranger/tmss-datepickerranger.component';
import { TmssComboboxComponent } from './input-types/tmss-combobox/tmss-combobox.component';
import { ProductionComboboxComponent } from './input-types/production-combobox/production-combobox.component';
import { TmssCheckboxComponent } from './input-types/tmss-checkbox/tmss-checkbox.component';
import { AddonWidthDirective } from '@shared/utils/addon-width.directive';
import { TmssTextareaComponent } from './input-types/tmss-textarea/tmss-textarea.component';
import { TmssTextInputComponent } from './input-types/tmss-text-input/tmss-text-input.component';
import { TmssSearchInputComponent } from './input-types/tmss-search-input/tmss-search-input.component';
import { TmssSelectGridModalComponent } from './grid/tmss-select-grid-modal/tmss-select-grid-modal.component';
import { TmssMultiColumnDropdownComponent } from './grid/tmss-multi-column-dropdown/tmss-multi-column-dropdown.component';
import { AgFilterGridComponent } from './grid/ag-filter-grid/ag-filter-grid.component';
import { AgDropdownRendererComponent } from './grid/ag-dropdown-renderer/ag-dropdown-renderer.component';
import { AgDatepickerRendererComponent } from './grid/ag-datepicker-renderer/ag-datepicker-renderer.component';
import { AgCheckboxRendererComponent } from './grid/ag-checkbox-renderer/ag-checkbox-renderer.component';
import { AgFloatingFilterGridComponent } from './grid/ag-floating-filter-grid/ag-floating-filter-grid.component';
import { DashboardComponent } from '@app/main/dashboard/dashboard.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        ModalModule.forRoot(),
        UtilsModule,
        tmssCommonModule,
        TableModule,
        PaginatorModule,
        GridsterModule,
        TabsModule.forRoot(),
        BsDropdownModule.forRoot(),
        NgxChartsModule,
        BsDatepickerModule.forRoot(),
        PerfectScrollbarModule,
        CountoModule,
        AppBsModalModule,
        AutoCompleteModule,
        AgGridModule.withComponents([
        ]),
        InputMaskModule,
        FormValidationModule,
        HttpClientModule,
    ],
    declarations: [
        TimeZoneComboComponent,
        CommonLookupModalComponent,
        EntityTypeHistoryModalComponent,
        EntityChangeDetailModalComponent,
        DateRangePickerInitialValueSetterDirective,
        DatePickerInitialValueSetterDirective,
        CustomizableDashboardComponent,
        WidgetGeneralStatsComponent,
        WidgetDailySalesComponent,
        WidgetEditionStatisticsComponent,
        WidgetHostTopStatsComponent,
        WidgetIncomeStatisticsComponent,
        WidgetMemberActivityComponent,
        WidgetProfitShareComponent,
        WidgetRecentTenantsComponent,
        WidgetRegionalStatsComponent,
        WidgetSalesSummaryComponent,
        WidgetSubscriptionExpiringTenantsComponent,
        WidgetTopStatsComponent,
        FilterDateRangePickerComponent,
        AddWidgetModalComponent,
        SingleLineStringInputTypeComponent,
        ComboboxInputTypeComponent,
        CheckboxInputTypeComponent,
        MultipleSelectComboboxInputTypeComponent,
        GridTableComponent,
        SimpleAgGridComponent,
        GridPaginationComponent,
        TmssDatepickerComponent,
        TmssDatepickerrangerComponent,
        TmssComboboxComponent,
        ProductionComboboxComponent,
        TmssCheckboxComponent,
        AddonWidthDirective,
        TmssTextareaComponent,
        TmssTextInputComponent,
        TmssSearchInputComponent,
        TmssSelectGridModalComponent,
        TmssMultiColumnDropdownComponent,
        AgFilterGridComponent,
        AgDropdownRendererComponent,
        AgDatepickerRendererComponent,
        AgCheckboxRendererComponent,
        AgFloatingFilterGridComponent,
    ],
    exports: [
        TimeZoneComboComponent,
        CommonLookupModalComponent,
        EntityTypeHistoryModalComponent,
        EntityChangeDetailModalComponent,
        DateRangePickerInitialValueSetterDirective,
        DatePickerInitialValueSetterDirective,
        CustomizableDashboardComponent,
        NgxChartsModule,
        FormValidationModule,
        GridTableComponent,
        SimpleAgGridComponent,
        GridPaginationComponent,
        TmssDatepickerComponent,
        TmssDatepickerrangerComponent,
        TmssComboboxComponent,
        ProductionComboboxComponent,
        TmssCheckboxComponent,
        AddonWidthDirective,
        TmssTextareaComponent,
        TmssTextInputComponent,
        TmssSearchInputComponent,
        TmssSelectGridModalComponent,
        TmssMultiColumnDropdownComponent,
        AgDropdownRendererComponent,
        AgDatepickerRendererComponent,
        AgCheckboxRendererComponent
    ],
    providers: [
        DateTimeService,
        AppLocalizationService,
        AppNavigationService,
        DashboardViewConfigurationService,
        { provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerConfig },
        { provide: BsDaterangepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDaterangepickerConfig },
        { provide: BsLocaleService, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerLocale },
    ],

    entryComponents: [
        WidgetGeneralStatsComponent,
        WidgetDailySalesComponent,
        WidgetEditionStatisticsComponent,
        WidgetHostTopStatsComponent,
        WidgetIncomeStatisticsComponent,
        WidgetMemberActivityComponent,
        WidgetProfitShareComponent,
        WidgetRecentTenantsComponent,
        WidgetRegionalStatsComponent,
        WidgetSalesSummaryComponent,
        WidgetSubscriptionExpiringTenantsComponent,
        WidgetTopStatsComponent,
        FilterDateRangePickerComponent,
        SingleLineStringInputTypeComponent,
        ComboboxInputTypeComponent,
        CheckboxInputTypeComponent,
        MultipleSelectComboboxInputTypeComponent,
        TmssDatepickerComponent,
        TmssDatepickerrangerComponent,
        TmssComboboxComponent,
        ProductionComboboxComponent,
        TmssCheckboxComponent,
        TmssTextareaComponent,
        TmssTextInputComponent,
        TmssSearchInputComponent,
        TmssSelectGridModalComponent,
        TmssMultiColumnDropdownComponent,
        AgDropdownRendererComponent,
        AgCheckboxRendererComponent
    ]
})
export class AppCommonModule {
    static forRoot(): ModuleWithProviders<AppCommonModule> {
        return {
            ngModule: AppCommonModule,
            providers: [
                AppAuthService,
                AppRouteGuard
            ]
        };
    }
}
