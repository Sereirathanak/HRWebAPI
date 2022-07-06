using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ccf_re_seller_api.Models;
namespace ccf_re_seller_api.Modals
{
    [Table("ccflred")]
    public class HRleaveApprovalRequest
    {
        private readonly HRContext _context;

        public HRleaveApprovalRequest(HRContext context)
        {
            _context = context;
        }

        [Key]
        public string lredid { get; set; }

        [ForeignKey("ccflre")]
        public string lreid { get; set; }
        public int applev { get; set; }
        [ForeignKey("ccfpinfo")]
        public string eid { get; set; }
        public int prio { get; set; }
        public int apstatu { get; set; }
        public string com { get; set; }
        public string remark { get; set; }

        public virtual HRLeaveRequest ccflre { get; set; }
        public virtual HREmployee ccfpinfo { get; set; }

        public string employeename
        {
            get
            {
                var checkLeaveRequest= _context?.leaveRequest.SingleOrDefault(cur => cur.lreid == this.lreid);


                if (checkLeaveRequest != null)
                {
                    var name = _context?.employee.SingleOrDefault(cur => cur.eid == checkLeaveRequest.eid);

                    return checkLeaveRequest.eid;
                }
                return "";
            }
        }


        public string approvername
        {
            get
            {
                var checkLeaveRequest = _context?.employee.SingleOrDefault(cur => cur.eid == this.eid);

                if (checkLeaveRequest != null)
                {
                    //var name = _context?.employee.SingleOrDefault(cur => cur.eid == checkLeaveRequest.eid);

                    return checkLeaveRequest.dname;
                }
                return "";
            }
        }

        public string leavereason
        {
            get
            {
                var checkLeaveRequest = _context?.leaveRequest.SingleOrDefault(cur => cur.lreid == this.lreid);

                if (checkLeaveRequest != null)
                {
                    return checkLeaveRequest.reason;
                }
                return "";
            }
        }

        public DateTime leavefrdate
        {
            get
            {
                var checkLeaveRequest = _context?.leaveRequest.SingleOrDefault(cur => cur.lreid == this.lreid);

                var dat1 = new DateTime();
                if (checkLeaveRequest != null)
                {
                    return checkLeaveRequest.frdat;
                }
                return dat1;
            }
        }

        public DateTime leavetodato
        {
            get
            {
                var dat1 = new DateTime();
                var checkLeaveRequest = _context?.leaveRequest.SingleOrDefault(cur => cur.lreid == this.lreid);

                if (checkLeaveRequest != null)
                {
                    return checkLeaveRequest.todat;
                }
                return dat1;
            }
        }

        public String leavenumber
        {
            get
            {
                var checkLeaveRequest = _context?.leaveRequest.SingleOrDefault(cur => cur.lreid == this.lreid);

                if (checkLeaveRequest != null)
                {
                    return checkLeaveRequest.numleav;
                }
                return "";
            }
        }

        public String leavefor
        {
            get
            {
                var checkLeaveRequest = _context?.leaveRequest.SingleOrDefault(cur => cur.lreid == this.lreid);

                if (checkLeaveRequest != null)
                {
                    return checkLeaveRequest.lfor;
                }
                return "";
            }
        }

        public String leavenoted
        {
            get
            {
                var checkLeaveRequest = _context?.leaveRequest.SingleOrDefault(cur => cur.lreid == this.lreid);
                if (checkLeaveRequest != null)
                {
                    return checkLeaveRequest.lnot;
                }
                return "";
            }
        }

    }
}
