using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace tmss.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));
            pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            var webhooks = administration.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription, L("Webhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Create, L("CreatingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Edit, L("EditingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_ChangeActivity, L("ChangingWebhookActivity"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Detail, L("DetailingSubscription"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ListSendAttempts, L("ListingSendAttempts"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ResendWebhook, L("ResendingWebhook"));

            var dynamicParameters = administration.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters, L("DynamicParameters"));
            dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters_Create, L("CreatingDynamicParameters"));
            dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters_Edit, L("EditingDynamicParameters"));
            dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters_Delete, L("DeletingDynamicParameters"));

            var dynamicParameterValues = dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue, L("DynamicParameterValue"));
            dynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue_Create, L("CreatingDynamicParameterValue"));
            dynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue_Edit, L("EditingDynamicParameterValue"));
            dynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue_Delete, L("DeletingDynamicParameterValue"));

            var entityDynamicParameters = dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters, L("EntityDynamicParameters"));
            entityDynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters_Create, L("CreatingEntityDynamicParameters"));
            entityDynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters_Edit, L("EditingEntityDynamicParameters"));
            entityDynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters_Delete, L("DeletingEntityDynamicParameters"));

            var entityDynamicParameterValues = dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue, L("EntityDynamicParameterValue"));
            entityDynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Create, L("CreatingEntityDynamicParameterValue"));
            entityDynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Edit, L("EditingEntityDynamicParameterValue"));
            entityDynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Delete, L("DeletingEntityDynamicParameterValue"));

            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);


            var master = pages.CreateChildPermission(AppPermissions.Pages_Master, L("Pages_Master"));

            var common = master.CreateChildPermission(AppPermissions.Pages_Master_Common, L("Pages_Master_Common"));


            var general = master.CreateChildPermission(AppPermissions.Pages_Master_General, L("Pages_Master_General"));

            var masterwarehouse = general.CreateChildPermission(AppPermissions.Pages_Master_General_Warehouse_View, L("Pages_Master_General_Warehouse_View"));
            masterwarehouse.CreateChildPermission(AppPermissions.Pages_Master_General_Warehouse_Edit, L("Pages_Master_General_Warehouse_Edit"));

            var material = general.CreateChildPermission(AppPermissions.Pages_Master_General_Material_View, L("Pages_Master_General_Material_View"));
            material.CreateChildPermission(AppPermissions.Pages_Master_General_Material_Edit, L("Pages_Master_General_Material_Edit"));

            var partlist = general.CreateChildPermission(AppPermissions.Pages_Master_General_PartList_View, L("Pages_Master_General_PartList_View"));
            partlist.CreateChildPermission(AppPermissions.Pages_Master_General_PartList_Edit, L("Pages_Master_General_PartList_Edit"));

            var cfc = general.CreateChildPermission(AppPermissions.Pages_Master_General_Cfc_View, L("Pages_Master_General_Cfc_View"));
            cfc.CreateChildPermission(AppPermissions.Pages_Master_General_Cfc_Edit, L("Pages_Master_General_Cfc_Edit"));

            var supplier = general.CreateChildPermission(AppPermissions.Pages_Master_General_Supplier_View, L("Pages_Master_General_Supplier_View"));
            supplier.CreateChildPermission(AppPermissions.Pages_Master_General_Supplier_Edit, L("Pages_Master_General_Supplier_Edit"));

            var forwarder = general.CreateChildPermission(AppPermissions.Pages_Master_General_Forwarder_View, L("Pages_Master_General_Forwarder_View"));
            forwarder.CreateChildPermission(AppPermissions.Pages_Master_General_Forwarder_Edit, L("Pages_Master_General_Forwarder_Edit"));



            var management = pages.CreateChildPermission(AppPermissions.Pages_Management, L("Pages_Management"));

            var intransit = management.CreateChildPermission(AppPermissions.Pages_Management_Intransit, L("Pages_Management_Intransit"));

            var orderpart = intransit.CreateChildPermission(AppPermissions.Pages_Management_Intransit_OrderPart_View, L("Pages_Management_Intransit_OrderPart_View"));
            orderpart.CreateChildPermission(AppPermissions.Pages_Management_Intransit_OrderPart_Edit, L("Pages_Management_Intransit_OrderPart_Edit"));

            var contintransit = intransit.CreateChildPermission(AppPermissions.Pages_Management_Intransit_ContainerIntransit_View, L("Pages_Management_Intransit_ContainerIntransit_View"));
            contintransit.CreateChildPermission(AppPermissions.Pages_Management_Intransit_ContainerIntransit_Edit, L("Pages_Management_Intransit_ContainerIntransit_Edit"));

            var shipment = intransit.CreateChildPermission(AppPermissions.Pages_Management_Intransit_Shipment_View, L("Pages_Management_Intransit_Shipment_View"));
            shipment.CreateChildPermission(AppPermissions.Pages_Management_Intransit_Shipment_Edit, L("Pages_Management_Intransit_Shipment_Edit"));

            var bill = intransit.CreateChildPermission(AppPermissions.Pages_Management_Intransit_Bill_View, L("Pages_Management_Intransit_Bill_View"));
            bill.CreateChildPermission(AppPermissions.Pages_Management_Intransit_Bill_Edit, L("Pages_Management_Intransit_Bill_Edit"));

            var invoice = intransit.CreateChildPermission(AppPermissions.Pages_Management_Intransit_Invoice_View, L("Pages_Management_Intransit_Invoice_View"));
            invoice.CreateChildPermission(AppPermissions.Pages_Management_Intransit_Invoice_Edit, L("Pages_Management_Intransit_Invoice_Edit"));

            var customsdeclare = intransit.CreateChildPermission(AppPermissions.Pages_Management_Intransit_CustomsDeclare_View, L("Pages_Management_Intransit_CustomsDeclare_View"));
            customsdeclare.CreateChildPermission(AppPermissions.Pages_Management_Intransit_CustomsDeclare_Edit, L("Pages_Management_Intransit_CustomsDeclare_Edit"));


            var warehouse = management.CreateChildPermission(AppPermissions.Pages_Management_Warehouse, L("Pages_Management_Warehouse"));

            var contlist = warehouse.CreateChildPermission(AppPermissions.Pages_Management_Warehouse_ContainerList_View, L("Pages_Management_Warehouse_ContainerList_View"));

            var contwarehouse = warehouse.CreateChildPermission(AppPermissions.Pages_Management_Warehouse_ContainerWarehouse_View, L("Pages_Management_Warehouse_ContainerWarehouse_View"));
            contwarehouse.CreateChildPermission(AppPermissions.Pages_Management_Warehouse_ContainerWarehouse_Edit, L("Pages_Management_Warehouse_ContainerWarehouse_Edit"));

            var stockwarehouse = warehouse.CreateChildPermission(AppPermissions.Pages_Management_Warehouse_StockWarehouse_View, L("Pages_Management_Warehouse_StockWarehouse_View"));
            stockwarehouse.CreateChildPermission(AppPermissions.Pages_Management_Warehouse_StockWarehouse_Edit, L("Pages_Management_Warehouse_StockWarehouse_Edit"));
            stockwarehouse.CreateChildPermission(AppPermissions.Pages_Management_Warehouse_StockWarehouse_Order, L("Pages_Management_Warehouse_StockWarehouse_Order"));

            var invoiceout = warehouse.CreateChildPermission(AppPermissions.Pages_Management_Warehouse_InvoiceOut_View, L("Pages_Management_Warehouse_InvoiceOut_View"));
            invoiceout.CreateChildPermission(AppPermissions.Pages_Management_Warehouse_InvoiceOut_Edit, L("Pages_Management_Warehouse_InvoiceOut_Edit"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, tmssConsts.LocalizationSourceName);
        }
    }
}
