using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ccf_re_seller_api.Models;

namespace ccf_re_seller_api.Modals
{
    [Table("ccflre")]
    public class HRLeaveRequest
    {
        private readonly HRContext _context;

        public HRLeaveRequest(HRContext context)
        {
            _context = context;
        }

        [Key]
        public string lreid { get; set; }

        [ForeignKey("ccforg")]
        public string orgid { get; set; }

        [ForeignKey("ccfpinfo")]
        public string eid { get; set; }

        [ForeignKey("ccflea")]
        public string leaid { get; set; }

        public DateTime frdat { get; set; }
        public DateTime todat { get; set; }
        public string numleav { get; set; }
        public string lfor { get; set; }
        public string lnot { get; set; }
        public string reason { get; set; }
        public string remark { get; set; }
        public byte[] file { get; set; }
        public DateTime createdate { get; set; }
        public string statu { get; set; }



        public virtual HROrganizationClass ccforg { get; set; }
        public virtual HRLeaveType ccflea { get; set; }
        public virtual HREmployee ccfpinfo { get; set; }

        public virtual HRleaveApprovalRequest ccfleaveApproval { get; set; }


        public String leavenoted
        {
            get
            {
                var checkLeaveRequest = _context?.leaveType.SingleOrDefault(cur => cur.leaid == this.leaid);
                if (checkLeaveRequest != null)
                {
                    return checkLeaveRequest.ltyp;
                }
                return "";
            }
        }

        public String leaverequestname
        {
            get
            {
                var checkLeaveRequest = _context?.employee.SingleOrDefault(cur => cur.eid == this.eid);
                if (checkLeaveRequest != null)
                {
                    return checkLeaveRequest.dname;
                }
                return "";
            }
        }

        public String leaverequestposition
        {
            get
            {
                var checkLeaveRequest = _context?.employeeJoinInfo.SingleOrDefault(cur => cur.eid == this.eid);
                if (checkLeaveRequest != null)
                {
                    return checkLeaveRequest.positionName;
                }
                return "";
            }
        }


        public object[] leaveapproverby
        {
            get
            {
                var checkLeaveRequest = _context?.leaveApprovalRequest.Where(cur => cur.lreid == this.lreid).ToList();

                if (checkLeaveRequest != null)
                {
                    return checkLeaveRequest.ToArray();
                }

                return null;
            }
        }

    }

   


    public class ValidateLeaveDocument
    {
        [Key]
        public string lreid { get; set; }

        public string orgid { get; set; }

        [ForeignKey("ccfpinfo")]
        public string eid { get; set; }

        public string leaid { get; set; }

        public DateTime frdat { get; set; }
        public DateTime todat { get; set; }
        public string numleav { get; set; }

        public string lfor { get; set; }
        public string lnot { get; set; }
        public string reason { get; set; }
        public string remark { get; set; }

        //[Required]
        public byte[] file { get; set; }

        public DateTime createdate { get; set; }
        public string statu { get; set; }


    }

   
}
