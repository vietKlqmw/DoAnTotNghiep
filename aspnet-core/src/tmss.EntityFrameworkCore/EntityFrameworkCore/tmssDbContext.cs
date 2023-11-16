using Abp.IdentityServer4;
using Abp.Organizations;
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
using tmss.Master.VehicleCBU;
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
        public virtual DbSet<MasterVehicleCBU> MasterVehicleCBUs { get; set; }

        public virtual DbSet<MasterCustomsStatus> MasterCustomsStatuss { get; set; }

        public virtual DbSet<MasterInvoiceStatus> MasterInvoiceStatuss { get; set; }

        public virtual DbSet<MasterContainerStatus> MasterContainerStatuss { get; set; }

        public virtual DbSet<MasterFactory> MasterFactorys { get; set; }

        public virtual DbSet<MasterStorageLocation> MasterStorageLocations { get; set; }

        public virtual DbSet<MasterMaterialGroup> MasterMaterialGroups { get; set; }

        public virtual DbSet<MasterProductGroup> MasterProductGroups { get; set; }

        public virtual DbSet<MasterProductType> MasterProductTypes { get; set; }

        public virtual DbSet<MasterMaterialType> MasterMaterialTypes { get; set; }

        public virtual DbSet<MasterMaterial> MasterMaterials { get; set; }

        public virtual DbSet<MasterUnitOfMeasure> MasterUnitOfMeasures { get; set; }

        public virtual DbSet<MasterFuelType> MasterFuelTypes { get; set; }

        public virtual DbSet<MasterCarfamily> MasterCarfamilys { get; set; }

        public virtual DbSet<MasterVehicleCKD> MasterVehicleCKDs { get; set; }

        public virtual DbSet<MasterEngine> MasterEngines { get; set; }

        public virtual DbSet<MasterCarSeries> MasterCarSeriess { get; set; }

        public virtual DbSet<MasterEngineModel> MasterEngineModels { get; set; }

        public virtual DbSet<MasterEngineType> MasterEngineTypes { get; set; }

        public virtual DbSet<MasterTransmissionType> MasterTransmissionTypes { get; set; }

        public virtual DbSet<MasterMaterial_T> MasterMaterial_Ts { get; set; }

        public virtual DbSet<MasterEngine_T> MasterEngine_Ts { get; set; }

        public virtual DbSet<MasterVehicleCKD_T> MasterVehicleCKD_Ts { get; set; }

        #endregion

        #region MaterialManagement

        public virtual DbSet<ProdShipment> ProdShipments { get; set; }

        public virtual DbSet<ProdBillOfLading> ProdBillOfLadings { get; set; }

        public virtual DbSet<ProdInvoice> ProdInvoices { get; set; }

        public virtual DbSet<ProdInvoiceDetails> ProdInvoiceDetailss { get; set; }

        public virtual DbSet<ProdContainerInvoice> ProdContainerInvoices { get; set; }

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
