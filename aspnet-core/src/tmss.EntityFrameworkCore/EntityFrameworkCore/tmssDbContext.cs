using Abp.IdentityServer4;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using tmss.Authorization.Delegation;
using tmss.Authorization.Roles;
using tmss.Authorization.Users;
using tmss.Chat;
using tmss.Editions;
using tmss.Friendships;
using tmss.Master;
using tmss.Master.CustomsStatus;
using tmss.Master.InvoiceStatus;
using tmss.Master.Material;
using tmss.MaterialManagement;
using tmss.MultiTenancy;
using tmss.MultiTenancy.Accounting;
using tmss.MultiTenancy.Payments;
using tmss.Storage;

namespace tmss.EntityFrameworkCore
{
    public class tmssDbContext : AbpZeroDbContext<Tenant, Role, User, tmssDbContext>, IAbpPersistedGrantDbContext
    {
        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        #region MASTER

        public virtual DbSet<MasterCustomsStatus> MasterCustomsStatuss { get; set; }

        public virtual DbSet<MasterInvoiceStatus> MasterInvoiceStatuss { get; set; }

        public virtual DbSet<MasterContainerStatus> MasterContainerStatuss { get; set; }

        public virtual DbSet<MasterStorageLocation> MasterStorageLocations { get; set; }

        public virtual DbSet<MasterMaterialGroup> MasterMaterialGroups { get; set; }

        public virtual DbSet<MasterProductType> MasterProductTypes { get; set; }

        public virtual DbSet<MasterMaterialType> MasterMaterialTypes { get; set; }

        public virtual DbSet<MasterMaterial> MasterMaterials { get; set; }

        public virtual DbSet<MasterUnitOfMeasure> MasterUnitOfMeasures { get; set; }

        public virtual DbSet<MasterCarfamily> MasterCarfamilys { get; set; }

        public virtual DbSet<MasterMaterial_T> MasterMaterial_Ts { get; set; }

        public virtual DbSet<MasterSupplierList> MasterSupplierLists { get; set; }

        public virtual DbSet<MasterForwarder> MasterForwarders { get; set; }

        public virtual DbSet<MasterPartList> MasterPartLists { get; set; }

        public virtual DbSet<MasterPartList_T> MasterPartList_Ts { get; set; }

        #endregion

        #region MaterialManagement

        public virtual DbSet<ProdStockReceiving> ProdStockReceivings { get; set; }

        public virtual DbSet<ProdShipment> ProdShipments { get; set; }

        public virtual DbSet<ProdBillOfLading> ProdBillOfLadings { get; set; }

        public virtual DbSet<ProdInvoice> ProdInvoices { get; set; }

        public virtual DbSet<ProdInvoiceDetails> ProdInvoiceDetailss { get; set; }

        public virtual DbSet<ProdContainerInvoice> ProdContainerInvoices { get; set; }

        public virtual DbSet<ProdContainerList> ProdContainerLists { get; set; }

        public virtual DbSet<ProdContainerIntransit> ProdContainerIntransits { get; set; }

        public virtual DbSet<ProdContainerRentalWHPlan> ProdContainerRentalWHPlans { get; set; }

        public virtual DbSet<ProdContainerRentalWHPlan_T> ProdContainerRentalWHPlan_Ts { get; set; }

        public virtual DbSet<ProdContainerTransitPortPlan> ProdContainerTransitPortPlans { get; set; }

        public virtual DbSet<ProdContainerTransitPortPlan_T> ProdContainerTransitPortPlan_Ts { get; set; }

        public virtual DbSet<ProdCustomsDeclare> ProdCustomsDeclares { get; set; }

        #endregion

        public tmssDbContext(DbContextOptions<tmssDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique();
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}
