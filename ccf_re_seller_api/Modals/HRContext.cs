using System;
using ccf_re_seller_api.Modals;
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
            public virtual DbSet<HRBranchClass> hrBranchClass { get; set; }
            public virtual DbSet<HRCcfassign> ccfassign { get; set; }
            public virtual DbSet<HRCcfrole> ccfrole { get; set; }
            public virtual DbSet<HRCcfulog> ccfulog { get; set; }
            public virtual DbSet<HREmployeeDocument> employeeDocument { get; set; }
            public virtual DbSet<HREmployeeFamily> employeeFamily { get; set; }
            public virtual DbSet<HREmployeeHistory> employeeHistory { get; set; }
            public virtual DbSet<HREmployeeEducation> employeeEducation { get; set; }
            public virtual DbSet<HRTimeLogClass> timeLogClass { get; set; }
            public virtual DbSet<HROrganizationClass> organizationClass { get; set; }
            public virtual DbSet<HRPosition> position { get; set; }
            public virtual DbSet<HRDepartment> department { get; set; }
            public virtual DbSet<HREmployeeType> employeeType { get; set; }
            public virtual DbSet<HRWorkCalendar> workCalendar { get; set; }
            public virtual DbSet<HRCalendar> calendar { get; set; }
            public virtual DbSet<HRLeaveType> leaveType { get; set; }
            public virtual DbSet<HRMapZoneClass> mapZoneClass { get; set; }
            public virtual DbSet<HRLeaveRequest> leaveRequest { get; set; }
            public virtual DbSet<HRMissionType> mssionType { get; set; }
            public virtual DbSet<HRMissionreq> missionreq { get; set; }
            public virtual DbSet<HRMissionApproval> missionApproval { get; set; }
            public virtual DbSet<HROverTimeType> overTimeType { get; set; }
            public virtual DbSet<HRleaveEnrollment> leaveEnrollment { get; set; }
            public virtual DbSet<HRleaveApprovalRequest> leaveApprovalRequest { get; set; }
            public virtual DbSet<HROverTimeRequest> overTimeRequest { get; set; }
            public virtual DbSet<HROverTimeApproval> overTimeApproval { get; set; }
            public virtual DbSet<HRMissionRequestDocument> missionRequestDocument { get; set; }
            public virtual DbSet<HRLeaveRequestDocument> leaveRequestDocument { get; set; }
            public virtual DbSet<HROvertimeRequestDocument> overtimeRequestDocument { get; set; }
            public virtual DbSet<HRImageProfile> imageProfile { get; set; }
            public virtual DbSet<HRCcfmessage> hrccfmessages { get; set; }
            public virtual DbSet<HRStructures> structures { get; set; }
            public virtual DbSet<HRGroupMissionClass> groupMissionRequest { get; set; }
            public virtual DbSet<HRGroupMissionDetailClass> groupMissionDetailClass { get; set; }
            public virtual DbSet<HRGroupMissionApproveClass> groupMissionApprove { get; set; }
            public virtual DbSet<HRGroupmMssionRequestDocumentClassClass> groupmMssionRequestDocumentClassClass { get; set; }
            public virtual DbSet<HRGroupOverTimeRequest> groupOverTimeRequest { get; set; }
            public virtual DbSet<HRGroupOverTimeDetail> groupOverTimeDetail { get; set; }
            public virtual DbSet<HRGroupOverTimeApprove> groupOverTimeApprove { get; set; }
            public virtual DbSet<HRGroupOverTimeDocument> groupOverTimeDocument { get; set; }
            public virtual DbSet<HRCcfUserBranch> UserBranches { get; set; }
        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                OnModelCreatingPartial(modelBuilder);

                modelBuilder.HasDefaultSchema("ccfhrmanagement");
            modelBuilder.Entity<HRCcfUserBranch>().ToTable("ccfuserbranch");
            modelBuilder.Entity<HRTimeLogClass>().ToTable("ccftim");

        }
            partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        }
}
