namespace tmss.Authorization
{
    /// <summary>
    /// Defines string constants for application's permission names.
    /// <see cref="AppAuthorizationProvider"/> for permission definitions.
    /// </summary>
    public static class AppPermissions
    {
        //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

        public const string Pages = "Pages";

        public const string Pages_DemoUiComponents = "Pages.DemoUiComponents";
        public const string Pages_Administration = "Pages.Administration";

        public const string Pages_Administration_Roles = "Pages.Administration.Roles";
        public const string Pages_Administration_Roles_Create = "Pages.Administration.Roles.Create";
        public const string Pages_Administration_Roles_Edit = "Pages.Administration.Roles.Edit";
        public const string Pages_Administration_Roles_Delete = "Pages.Administration.Roles.Delete";

        public const string Pages_Administration_Users = "Pages.Administration.Users";
        public const string Pages_Administration_Users_Create = "Pages.Administration.Users.Create";
        public const string Pages_Administration_Users_Edit = "Pages.Administration.Users.Edit";
        public const string Pages_Administration_Users_Delete = "Pages.Administration.Users.Delete";
        public const string Pages_Administration_Users_ChangePermissions = "Pages.Administration.Users.ChangePermissions";
        public const string Pages_Administration_Users_Impersonation = "Pages.Administration.Users.Impersonation";
        public const string Pages_Administration_Users_Unlock = "Pages.Administration.Users.Unlock";

        public const string Pages_Administration_Languages = "Pages.Administration.Languages";
        public const string Pages_Administration_Languages_Create = "Pages.Administration.Languages.Create";
        public const string Pages_Administration_Languages_Edit = "Pages.Administration.Languages.Edit";
        public const string Pages_Administration_Languages_Delete = "Pages.Administration.Languages.Delete";
        public const string Pages_Administration_Languages_ChangeTexts = "Pages.Administration.Languages.ChangeTexts";

        public const string Pages_Administration_AuditLogs = "Pages.Administration.AuditLogs";

        public const string Pages_Administration_OrganizationUnits = "Pages.Administration.OrganizationUnits";
        public const string Pages_Administration_OrganizationUnits_ManageOrganizationTree = "Pages.Administration.OrganizationUnits.ManageOrganizationTree";
        public const string Pages_Administration_OrganizationUnits_ManageMembers = "Pages.Administration.OrganizationUnits.ManageMembers";
        public const string Pages_Administration_OrganizationUnits_ManageRoles = "Pages.Administration.OrganizationUnits.ManageRoles";

        public const string Pages_Administration_HangfireDashboard = "Pages.Administration.HangfireDashboard";

        public const string Pages_Administration_UiCustomization = "Pages.Administration.UiCustomization";

        public const string Pages_Administration_WebhookSubscription = "Pages.Administration.WebhookSubscription";
        public const string Pages_Administration_WebhookSubscription_Create = "Pages.Administration.WebhookSubscription.Create";
        public const string Pages_Administration_WebhookSubscription_Edit = "Pages.Administration.WebhookSubscription.Edit";
        public const string Pages_Administration_WebhookSubscription_ChangeActivity = "Pages.Administration.WebhookSubscription.ChangeActivity";
        public const string Pages_Administration_WebhookSubscription_Detail = "Pages.Administration.WebhookSubscription.Detail";
        public const string Pages_Administration_Webhook_ListSendAttempts = "Pages.Administration.Webhook.ListSendAttempts";
        public const string Pages_Administration_Webhook_ResendWebhook = "Pages.Administration.Webhook.ResendWebhook";

        public const string Pages_Administration_DynamicParameters = "Pages.Administration.DynamicParameters";
        public const string Pages_Administration_DynamicParameters_Create = "Pages.Administration.DynamicParameters.Create";
        public const string Pages_Administration_DynamicParameters_Edit = "Pages.Administration.DynamicParameters.Edit";
        public const string Pages_Administration_DynamicParameters_Delete = "Pages.Administration.DynamicParameters.Delete";

        public const string Pages_Administration_DynamicParameterValue = "Pages.Administration.DynamicParameterValue";
        public const string Pages_Administration_DynamicParameterValue_Create = "Pages.Administration.DynamicParameterValue.Create";
        public const string Pages_Administration_DynamicParameterValue_Edit = "Pages.Administration.DynamicParameterValue.Edit";
        public const string Pages_Administration_DynamicParameterValue_Delete = "Pages.Administration.DynamicParameterValue.Delete";

        public const string Pages_Administration_EntityDynamicParameters = "Pages.Administration.EntityDynamicParameters";
        public const string Pages_Administration_EntityDynamicParameters_Create = "Pages.Administration.EntityDynamicParameters.Create";
        public const string Pages_Administration_EntityDynamicParameters_Edit = "Pages.Administration.EntityDynamicParameters.Edit";
        public const string Pages_Administration_EntityDynamicParameters_Delete = "Pages.Administration.EntityDynamicParameters.Delete";

        public const string Pages_Administration_EntityDynamicParameterValue = "Pages.Administration.EntityDynamicParameterValue";
        public const string Pages_Administration_EntityDynamicParameterValue_Create = "Pages.Administration.EntityDynamicParameterValue.Create";
        public const string Pages_Administration_EntityDynamicParameterValue_Edit = "Pages.Administration.EntityDynamicParameterValue.Edit";
        public const string Pages_Administration_EntityDynamicParameterValue_Delete = "Pages.Administration.EntityDynamicParameterValue.Delete";
        //TENANT-SPECIFIC PERMISSIONS

        public const string Pages_Tenant_Dashboard = "Pages.Tenant.Dashboard";

        public const string Pages_Administration_Tenant_Settings = "Pages.Administration.Tenant.Settings";

        public const string Pages_Administration_Tenant_SubscriptionManagement = "Pages.Administration.Tenant.SubscriptionManagement";

        //HOST-SPECIFIC PERMISSIONS

        public const string Pages_Editions = "Pages.Editions";
        public const string Pages_Editions_Create = "Pages.Editions.Create";
        public const string Pages_Editions_Edit = "Pages.Editions.Edit";
        public const string Pages_Editions_Delete = "Pages.Editions.Delete";
        public const string Pages_Editions_MoveTenantsToAnotherEdition = "Pages.Editions.MoveTenantsToAnotherEdition";

        public const string Pages_Tenants = "Pages.Tenants";
        public const string Pages_Tenants_Create = "Pages.Tenants.Create";
        public const string Pages_Tenants_Edit = "Pages.Tenants.Edit";
        public const string Pages_Tenants_ChangeFeatures = "Pages.Tenants.ChangeFeatures";
        public const string Pages_Tenants_Delete = "Pages.Tenants.Delete";
        public const string Pages_Tenants_Impersonation = "Pages.Tenants.Impersonation";

        public const string Pages_Administration_Host_Maintenance = "Pages.Administration.Host.Maintenance";
        public const string Pages_Administration_Host_Settings = "Pages.Administration.Host.Settings";
        public const string Pages_Administration_Host_Dashboard = "Pages.Administration.Host.Dashboard";


        #region MyApp

        public const string Pages_Master = "Pages.Master";

        public const string Pages_Master_Common = "Pages.Master.Common";

        public const string Pages_Master_General = "Pages.Master.General";

        public const string Pages_Master_General_Warehouse_View = "Pages.Master.General.Warehouse.View";
        public const string Pages_Master_General_Warehouse_Edit = "Pages.Master.General.Warehouse.Edit";

        public const string Pages_Master_General_Material_View = "Pages.Master.General.Material.View";
        public const string Pages_Master_General_Material_Edit = "Pages.Master.General.Material.Edit";

        public const string Pages_Master_General_PartList_View = "Pages.Master.General.PartList.View";
        public const string Pages_Master_General_PartList_Edit = "Pages.Master.General.PartList.Edit";

        public const string Pages_Master_General_Cfc_View = "Pages.Master.General.Cfc.View";
        public const string Pages_Master_General_Cfc_Edit = "Pages.Master.General.Cfc.Edit";

        public const string Pages_Master_General_Supplier_View = "Pages.Master.General.Supplier.View";
        public const string Pages_Master_General_Supplier_Edit = "Pages.Master.General.Supplier.Edit";

        public const string Pages_Master_General_Forwarder_View = "Pages.Master.General.Forwarder.View";
        public const string Pages_Master_General_Forwarder_Edit = "Pages.Master.General.Forwarder.Edit";



        public const string Pages_Management = "Pages.Management";

        public const string Pages_Management_Intransit = "Pages.Management.Intransit";

        public const string Pages_Management_Intransit_OrderPart_View = "Pages.Management.Intransit.OrderPart.View";
        public const string Pages_Management_Intransit_OrderPart_Edit = "Pages.Management.Intransit.OrderPart.Edit";

        public const string Pages_Management_Intransit_ContainerIntransit_View = "Pages.Management.Intransit.ContainerIntransit.View";
        public const string Pages_Management_Intransit_ContainerIntransit_Edit = "Pages.Management.Intransit.ContainerIntransit.Edit";

        public const string Pages_Management_Intransit_Shipment_View = "Pages.Management.Intransit.Shipment.View";
        public const string Pages_Management_Intransit_Shipment_Edit = "Pages.Management.Intransit.Shipment.Edit";

        public const string Pages_Management_Intransit_Bill_View = "Pages.Management.Intransit.Bill.View";
        public const string Pages_Management_Intransit_Bill_Edit = "Pages.Management.Intransit.Bill.Edit";

        public const string Pages_Management_Intransit_Invoice_View = "Pages.Management.Intransit.Invoice.View";
        public const string Pages_Management_Intransit_Invoice_Edit = "Pages.Management.Intransit.Invoice.Edit";

        public const string Pages_Management_Intransit_CustomsDeclare_View = "Pages.Management.Intransit.CustomsDeclare.View";
        public const string Pages_Management_Intransit_CustomsDeclare_Edit = "Pages.Management.Intransit.CustomsDeclare.Edit";

        public const string Pages_Management_Warehouse = "Pages.Management.Warehouse";

        public const string Pages_Management_Warehouse_ContainerList_View = "Pages.Management.Warehouse.ContainerList.View";

        public const string Pages_Management_Warehouse_ContainerWarehouse_View = "Pages.Management.Warehouse.ContainerWarehouse.View";
        public const string Pages_Management_Warehouse_ContainerWarehouse_Edit = "Pages.Management.Warehouse.ContainerWarehouse.Edit";

        public const string Pages_Management_Warehouse_StockWarehouse_View = "Pages.Management.Warehouse.StockWarehouse.View";
        public const string Pages_Management_Warehouse_StockWarehouse_Edit = "Pages.Management.Warehouse.StockWarehouse.Edit";
        public const string Pages_Management_Warehouse_StockWarehouse_Order = "Pages.Management.Warehouse.StockWarehouse.Order";

        public const string Pages_Management_Warehouse_InvoiceOut_View = "Pages.Management.Warehouse.InvoiceOut.View";
        public const string Pages_Management_Warehouse_InvoiceOut_Edit = "Pages.Management.Warehouse.InvoiceOut.Edit";

        #endregion
    }
}