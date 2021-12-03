using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
namespace ccf_re_seller_api.Models
{
    public partial class HRContext : DbContext
        {
            public HRContext()
            {
            }

            public HRContext(DbContextOptions<HRContext> options)
                : base(options)
            {
            }

            public virtual DbSet<HRCcfUserClass> ccfUserClass { get; set; }
            public virtual DbSet<HREmployee> employee { get; set; }
            public virtual DbSet<HREmployeeJoinInfo> employeeJoinInfo { get; set; }
            public virtual DbSet<HRBranchClass> branchClass { get; set; }
            public virtual DbSet<HRCcfassign> ccfassign { get; set; }
            public virtual DbSet<HRCcfrole> ccfrole { get; set; }
            public virtual DbSet<HRCcfulog> ccfulog { get; set; }
            public virtual DbSet<HREmployeeDocument> employeeDocument { get; set; }
            public virtual DbSet<HREmployeeFamily> employeeFamily { get; set; }
            public virtual DbSet<HREmployeeHistory> employeeHistory { get; set; }
            public virtual DbSet<HREmployeeEducation> employeeEducation { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                OnModelCreatingPartial(modelBuilder);

                modelBuilder.HasDefaultSchema("ccfhrmanagement");
            }
            partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        }
}
