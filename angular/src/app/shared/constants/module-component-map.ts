import { TABS } from '../constants/tab-keys';

export const MODULE_PATHS = {
    APP_ADMIN_ADMIN_MODULE: 'app/admin/admin.module',

    // Master bts
    MASTER_BTS_SYSTEM_PARAM: 'app/main/master/system-param/system-param.module',
    MASTER_BTS_COUNTRY_DESTINATION: 'app/main/master/country-destination/country-destination.module',
    MASTER_BTS_TITLE_GROUP: 'app/main/master/title-group/title-group.module',
    MASTER_BTS_BUSINESS_TRIP_POLICY: 'app/main/master/business-trip-policy/business-trip-policy.module',
    MASTER_BTS_BUDGET: 'app/main/master/budget/budget.module',
    MASTER_BTS_INVOICING: 'app/main/master/invoicing/invoicing.module',
    MASTER_BTS_AIR_TICKET: 'app/main/master/air-ticket/air-ticket.module',
    MASTER_BTS_HOTEL: 'app/main/master/hotel/mst-hotel.module',
    MASTER_BTS_ORACLE_INVOICE_BATCH_NAME: 'app/main/master/oracle-invoice-batch-name/oracle-invoice-batch-name.module',
    MASTER_BTS_DAILY_EXCHANGE_RATES: 'app/main/master/daily-exchange-rates/daily-exchange-rates.module',
    MASTER_BTS_VISA: 'app/main/master/visa/visa.module',
    MASTER_BTS_TIME_KEEPER: 'app/main/master/time-keeper/time-keeper.module',

    //INF_FQF3BT01: 'app/main/inf/inf-fqf3bt01/inf-fqf3bt01.module',
    INF_FQF3BT03: 'app/main/inf/inf-fqf3bt03/inf-fqf3bt03.module',

    // One day
    ONEDAY_APPROVAL: 'app/main/one-day/bt-approval/bt-approval.module',
    ONEDAY_APPROVAL_DETAIL: 'app/main/one-day/bt-approval-detail/oneday-bt-approval-detail.module',
    ONEDAY_EXPENSE_DECLARATION: 'app/main/one-day/bt-expense-declaration/oneday-bt-expense-declaration.module',
    ONEDAY_EXPENSE_DECLARATION_DETAIL: 'app/main/one-day/bt-expense-declaration-detail/oneday-bt-expense-declaration-detail.module',

    //Overnight
    OVERNIGHT_APPROVAL: 'app/main/overnight/bt-approval/bt-approval.module',
    OVERNIGHT_EXPENSE_DECLARATION: 'app/main/overnight/bt-expense-declaration/overnight-bt-expense-declaration.module',
    OVERNIGHT_EXPENSE_DECLARATION_DETAIL: 'app/main/overnight/bt-expense-declaration-detail/overnight-bt-expense-declaration-detail.module',
    OVERNIGHT_EXPENSE_LATE: 'app/main/overnight/bt-expense-late/overnight-bt-expense-late.module',
    OVERNIGHT_WIFI_DEVICE_PRE_REQUEST: 'app/main/overnight/wifi-device-pre-request/wifi-device-pre-request.module',
    OVERNIGHT_APPROVAL_DETAIL: 'app/main/overnight/bt-approval-detail/overnight-bt-approval-detail.module',

    //FIN
    FIN_ADVANCE_PAYMENT: 'app/main/fin/advance-payment/advance-payment.module',
    FIN_AIR_TICKET: 'app/main/fin/air-ticket/air-ticket.module',
    FIN_HOTEL: 'app/main/fin/hotel/hotel.module',
    FIN_BUDGET_CHECKING: 'app/main/fin/budget-checking/budget-checking.module',
    FIN_EXPENSE_PAYMENT: 'app/main/fin/expense-payment/expense-payment.module',
    FIN_INVOICING: 'app/main/fin/invoicing/invoicing.module',
    FIN_IMPORT_TO_HRM : 'app/main/fin/import-to-hrm/import-to-hrm.module',
    //GA
    //GA_AIR_TICKET: 'app/main/ga/air-ticket/air-ticket.module',
    GA_AIR_TICKET_REQUEST: 'app/main/ga/air-ticket-request/air-ticket-request.module',
    GA_INFO: 'app/main/ga/ga-info/ga-info.module',
    GA_EXPECTED_AIR_TICKET: 'app/main/ga/expected-air-ticket/expected-air-ticket.module',
    GA_HOTEL: 'app/main/ga/hotel/hotel.module',

    //HR
    HR_AIR_TICKET_ICT: 'app/main/hr/air-ticket-ict/air-ticket-ict.module',

    //IT
    IT_WIFI_DEVICE_REQUEST: 'app/main/it-mngt/wifi-device-request/wifi-device-request.module',
    WIFI_DEVICE_REQUEST: 'app/main/it-mngt/wifi-device-request/wifi-device-request.module',

    //DOWLOAD
    DOWLOAD: 'app/main/dowload/dowload.module',

    //FAQ
    FAQ: 'app/main/faq/faq.module',

    //REPORT
    REPORT: 'app/main/report/report.module',

    // TIMER KEEPER
    TIMER_KEEPER: 'app/main/timer-keeper/timer-keeper.module',

    //Interface
    FQF3BT01: 'app/main/interface/fqf3bt01/fqf3bt01.module',
    FQF3BTLV2: 'app/main/interface/fqf3btlv2/fqf3btlv2.module',

    //DashBoard
    DASHBOARD: 'app/main/dashboard/dashboard.module',
    //
    FIN_DASHBOARDFIN: 'app/main/dashboardFin/dashboardfin.module',

};

export const MODULE_COMPONENT_MAP = {
  [MODULE_PATHS.APP_ADMIN_ADMIN_MODULE]: [
    TABS.ADMIN_ORGANIZATION_UNITS,
    TABS.ADMIN_USERS,
    TABS.ADMIN_ROLES,
    TABS.ADMIN_AUDIT_LOGS,
    TABS.ADMIN_LANGUAGE,
    TABS.ADMIN_UI_CUSTOMIZATION,
    TABS.ADMIN_HOST_SETTINGS,
    TABS.ADMIN_TENANT_SETTINGS,
  ],

  // Master
  [MODULE_PATHS.MASTER_BTS_SYSTEM_PARAM]: [TABS.MASTER_BTS_SYSTEM_PARAM],
  [MODULE_PATHS.MASTER_BTS_COUNTRY_DESTINATION]: [TABS.MASTER_BTS_COUNTRY_DESTINATION],
  [MODULE_PATHS.MASTER_BTS_TITLE_GROUP]: [TABS.MASTER_BTS_TITLE_GROUP],
  [MODULE_PATHS.MASTER_BTS_BUSINESS_TRIP_POLICY]: [TABS.MASTER_BTS_BUSINESS_TRIP_POLICY],
  [MODULE_PATHS.MASTER_BTS_BUDGET]: [TABS.MASTER_BTS_BUDGET],
  [MODULE_PATHS.MASTER_BTS_INVOICING]: [TABS.MASTER_BTS_INVOICING],
  [MODULE_PATHS.MASTER_BTS_AIR_TICKET]: [TABS.MASTER_BTS_AIR_TICKET],
  [MODULE_PATHS.MASTER_BTS_HOTEL]: [TABS.MASTER_BTS_HOTEL],
  [MODULE_PATHS.MASTER_BTS_ORACLE_INVOICE_BATCH_NAME]: [TABS.MASTER_BTS_ORACLE_INVOICE_BATCH_NAME],
  [MODULE_PATHS.MASTER_BTS_DAILY_EXCHANGE_RATES]: [TABS.MASTER_BTS_DAILY_EXCHANGE_RATES],
  [MODULE_PATHS.MASTER_BTS_VISA]: [TABS.MASTER_BTS_VISA],
  [MODULE_PATHS.MASTER_BTS_TIME_KEEPER]: [TABS.MASTER_BTS_TIME_KEEPER],

  //[MODULE_PATHS.INF_FQF3BT01]: [TABS.INF_FQF3BT01],
  [MODULE_PATHS.INF_FQF3BT03]: [TABS.INF_FQF3BT03],

  // One day
  [MODULE_PATHS.ONEDAY_APPROVAL]: [TABS.ONEDAY_APPROVAL],
  [MODULE_PATHS.ONEDAY_APPROVAL_DETAIL]: [TABS.ONEDAY_APPROVAL_DETAIL],
  [MODULE_PATHS.ONEDAY_EXPENSE_DECLARATION]: [TABS.ONEDAY_EXPENSE_DECLARATION],
  [MODULE_PATHS.ONEDAY_EXPENSE_DECLARATION_DETAIL]: [TABS.ONEDAY_EXPENSE_DECLARATION_DETAIL],

  //Overnight
  [MODULE_PATHS.OVERNIGHT_APPROVAL]: [TABS.OVERNIGHT_APPROVAL],
  [MODULE_PATHS.OVERNIGHT_EXPENSE_DECLARATION]: [TABS.OVERNIGHT_EXPENSE_DECLARATION],
  [MODULE_PATHS.OVERNIGHT_EXPENSE_DECLARATION_DETAIL]: [TABS.OVERNIGHT_EXPENSE_DECLARATION_DETAIL],
  [MODULE_PATHS.OVERNIGHT_EXPENSE_LATE]: [TABS.OVERNIGHT_EXPENSE_LATE],
  [MODULE_PATHS.OVERNIGHT_WIFI_DEVICE_PRE_REQUEST]: [TABS.OVERNIGHT_WIFI_DEVICE_PRE_REQUEST],
  [MODULE_PATHS.OVERNIGHT_APPROVAL_DETAIL]: [TABS.OVERNIGHT_APPROVAL_DETAIL],

  //FIN
  [MODULE_PATHS.FIN_ADVANCE_PAYMENT]: [TABS.FIN_ADVANCE_PAYMENT],
  [MODULE_PATHS.FIN_AIR_TICKET]: [TABS.FIN_AIR_TICKET],
  [MODULE_PATHS.FIN_BUDGET_CHECKING]: [TABS.FIN_BUDGET_CHECKING],
  [MODULE_PATHS.FIN_EXPENSE_PAYMENT]: [TABS.FIN_EXPENSE_PAYMENT],
  [MODULE_PATHS.FIN_INVOICING]: [TABS.FIN_INVOICING],
  [MODULE_PATHS.FIN_HOTEL]: [TABS.FIN_HOTEL],
  [MODULE_PATHS.FIN_IMPORT_TO_HRM]: [TABS.FIN_IMPORT_TO_HRM],

  //GA
  //[MODULE_PATHS.GA_AIR_TICKET]: [TABS.GA_AIR_TICKET],
  [MODULE_PATHS.GA_AIR_TICKET_REQUEST]: [TABS.GA_AIR_TICKET_REQUEST],
  [MODULE_PATHS.GA_INFO]: [TABS.GA_INFO],
  [MODULE_PATHS.GA_EXPECTED_AIR_TICKET]: [TABS.GA_EXPECTED_AIR_TICKET],
  [MODULE_PATHS.GA_HOTEL]: [TABS.GA_HOTEL],

  //HR
  [MODULE_PATHS.HR_AIR_TICKET_ICT]: [TABS.HR_AIR_TICKET_ICT],

  //IT
  [MODULE_PATHS.IT_WIFI_DEVICE_REQUEST]: [TABS.IT_WIFI_DEVICE_REQUEST],
  [MODULE_PATHS.WIFI_DEVICE_REQUEST]: [TABS.WIFI_DEVICE_REQUEST],

  //DOWLOAD
  [MODULE_PATHS.DOWLOAD]: [TABS.DOWLOAD],

  //FAQ
  [MODULE_PATHS.FAQ]: [TABS.FAQ],

  //REPORT
  [MODULE_PATHS.REPORT]: [TABS.REPORT],

  [MODULE_PATHS.TIMER_KEEPER]: [TABS.TIMER_KEEPER],

  //Interface
  [MODULE_PATHS.FQF3BT01]: [TABS.FQF3BT01],
  [MODULE_PATHS.FQF3BTLV2]: [TABS.FQF3BTLV2],

  //Dashboard
  [MODULE_PATHS.DASHBOARD]: [TABS.DASHBOARD],
  [MODULE_PATHS.FIN_DASHBOARDFIN]: [TABS.FIN_DASHBOARDFIN],

};
