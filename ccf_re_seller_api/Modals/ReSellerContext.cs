using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ccf_re_seller_api.Modals
{
    public partial class ReSellerAPIContext : DbContext
    {
        public ReSellerAPIContext()
        {
        }

        public ReSellerAPIContext(DbContextOptions<ReSellerAPIContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CcfassignRe> CcfassignRes { get; set; }
        public virtual DbSet<CcfcustApr> CcfcustAprs { get; set; }
        public virtual DbSet<CcfcustAsig> CcfcustAsigs { get; set; }
        public virtual DbSet<CcflogRe> CcflogRes { get; set; }
        public virtual DbSet<CcfmessagesRe> CcfmessagesRes { get; set; }
        public virtual DbSet<CcfreferalCu> CcfreferalCus { get; set; }
        public virtual DbSet<CcfreferalCusUp> CcfreferalCusUps { get; set; }
        public virtual DbSet<CcfreferalRe> CcfreferalRes { get; set; }
        public virtual DbSet<CcfroleRe> CcfroleRes { get; set; }
        public virtual DbSet<CcfuserRe> CcfuserRes { get; set; }
        public virtual DbSet<BranchClass> BranchClass { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Currency> Currency { get; set; }
        public virtual DbSet<Transition> Transition { get; set; }
        public virtual DbSet<ExchangeRate> ExchangeRate { get; set; }
        public virtual DbSet<Document> Document { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);

            modelBuilder.HasDefaultSchema("ccfreseller");
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
