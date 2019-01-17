namespace Team7ADProject.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class LogicDB : DbContext
    {
        public LogicDB()
            : base("name=LogicDB")
        {
        }

        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<CollectionPoint> CollectionPoint { get; set; }
        public virtual DbSet<DelegationOfAuthority> DelegationOfAuthority { get; set; }
        public virtual DbSet<DeliveryOrder> DeliveryOrder { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Disbursement> Disbursement { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<PurchaseOrder> PurchaseOrder { get; set; }
        public virtual DbSet<Stationery> Stationery { get; set; }
        public virtual DbSet<StationeryRequest> StationeryRequest { get; set; }
        public virtual DbSet<StationeryRetrieval> StationeryRetrieval { get; set; }
        public virtual DbSet<StockAdjustment> StockAdjustment { get; set; }
        public virtual DbSet<Supplier> Supplier { get; set; }
        public virtual DbSet<TransactionDetail> TransactionDetail { get; set; }
        public virtual DbSet<RequestByDeptView> RequestByDeptView { get; set; }
        public virtual DbSet<RequestByItemView> RequestByItemView { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoles>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.DelegationOfAuthority)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.DelegatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.DelegationOfAuthority1)
                .WithRequired(e => e.AspNetUsers1)
                .HasForeignKey(e => e.DelegatedTo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.DeliveryOrder)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.AcceptedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.Department1)
                .WithRequired(e => e.AspNetUsers1)
                .HasForeignKey(e => e.DepartmentRepId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.Department2)
                .WithRequired(e => e.AspNetUsers2)
                .HasForeignKey(e => e.DepartmentHeadId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.Disbursement)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.DisbursedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.Disbursement1)
                .WithRequired(e => e.AspNetUsers1)
                .HasForeignKey(e => e.AcknowledgedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.PurchaseOrder)
                .WithOptional(e => e.AspNetUsers)
                .HasForeignKey(e => e.ApprovedBy);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.PurchaseOrder1)
                .WithRequired(e => e.AspNetUsers1)
                .HasForeignKey(e => e.OrderedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.StationeryRequest)
                .WithOptional(e => e.AspNetUsers)
                .HasForeignKey(e => e.ApprovedBy);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.StationeryRequest1)
                .WithRequired(e => e.AspNetUsers1)
                .HasForeignKey(e => e.RequestedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.StationeryRetrieval)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.RetrievedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.StockAdjustment)
                .WithOptional(e => e.AspNetUsers)
                .HasForeignKey(e => e.ApprovedBy);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.StockAdjustment1)
                .WithRequired(e => e.AspNetUsers1)
                .HasForeignKey(e => e.PreparedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CollectionPoint>()
                .HasMany(e => e.Department)
                .WithRequired(e => e.CollectionPoint)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DeliveryOrder>()
                .HasMany(e => e.Invoice)
                .WithRequired(e => e.DeliveryOrder)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DeliveryOrder>()
                .HasMany(e => e.TransactionDetail)
                .WithRequired(e => e.DeliveryOrder)
                .HasForeignKey(e => e.TransactionRef)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Department>()
                .HasMany(e => e.AspNetUsers)
                .WithRequired(e => e.Department)
                .HasForeignKey(e => e.DepartmentId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Department>()
                .HasMany(e => e.DelegationOfAuthority)
                .WithRequired(e => e.Department)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Department>()
                .HasMany(e => e.Disbursement)
                .WithRequired(e => e.Department)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Department>()
                .HasMany(e => e.StationeryRequest)
                .WithRequired(e => e.Department)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Disbursement>()
                .HasMany(e => e.TransactionDetail)
                .WithRequired(e => e.Disbursement)
                .HasForeignKey(e => e.TransactionRef)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Invoice>()
                .HasMany(e => e.TransactionDetail)
                .WithRequired(e => e.Invoice)
                .HasForeignKey(e => e.TransactionRef)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PurchaseOrder>()
                .HasMany(e => e.DeliveryOrder)
                .WithRequired(e => e.PurchaseOrder)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PurchaseOrder>()
                .HasMany(e => e.TransactionDetail)
                .WithRequired(e => e.PurchaseOrder)
                .HasForeignKey(e => e.TransactionRef)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Stationery>()
                .HasMany(e => e.TransactionDetail)
                .WithRequired(e => e.Stationery)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StationeryRequest>()
                .HasMany(e => e.Disbursement)
                .WithRequired(e => e.StationeryRequest)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StationeryRequest>()
                .HasMany(e => e.TransactionDetail)
                .WithRequired(e => e.StationeryRequest)
                .HasForeignKey(e => e.TransactionRef)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StationeryRetrieval>()
                .HasMany(e => e.TransactionDetail)
                .WithRequired(e => e.StationeryRetrieval)
                .HasForeignKey(e => e.TransactionRef)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StockAdjustment>()
                .HasMany(e => e.TransactionDetail)
                .WithRequired(e => e.StockAdjustment)
                .HasForeignKey(e => e.TransactionRef)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Supplier>()
                .HasMany(e => e.DeliveryOrder)
                .WithRequired(e => e.Supplier)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Supplier>()
                .HasMany(e => e.Invoice)
                .WithRequired(e => e.Supplier)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Supplier>()
                .HasMany(e => e.PurchaseOrder)
                .WithRequired(e => e.Supplier)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Supplier>()
                .HasMany(e => e.Stationery)
                .WithRequired(e => e.Supplier)
                .HasForeignKey(e => e.FirstSupplierId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Supplier>()
                .HasMany(e => e.Stationery1)
                .WithRequired(e => e.Supplier1)
                .HasForeignKey(e => e.SecondSupplierId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Supplier>()
                .HasMany(e => e.Stationery2)
                .WithRequired(e => e.Supplier2)
                .HasForeignKey(e => e.ThirdSupplierId)
                .WillCascadeOnDelete(false);
        }
    }
}
