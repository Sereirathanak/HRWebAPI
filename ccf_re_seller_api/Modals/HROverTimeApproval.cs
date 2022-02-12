using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ccf_re_seller_api.Models;
namespace ccf_re_seller_api.Modals
{
    [Table("ccfoved")]
    public class HROverTimeApproval
    {
        private readonly HRContext _context;
        public HROverTimeApproval(HRContext context)
        {
            _context = context;
        }
        [Key]
        public string ovedid { get; set; }

        [ForeignKey("ccfove")]
        public string oveid { get; set; }
        public int applev { get; set; }
        [ForeignKey("ccfpinfo")]
        public string eid { get; set; }
        public int prio { get; set; }
        public int apstatu { get; set; }
        public string com { get; set; }

        public virtual HROverTimeRequest ccfove { get; set; }
        public virtual HREmployee ccfpinfo { get; set; }

        public string employeename
        {
            get
            {
                var checkLeaveRequest = _context?.overTimeRequest.SingleOrDefault(cur => cur.oveid == this.oveid);
                var name = _context?.employee.SingleOrDefault(cur => cur.eid == checkLeaveRequest.eid);

                if (name != null)
                {
                    return name.dname;
                }
                return "";
            }
        }

        public string leavereason
        {
            get
            {
                var checkLeaveRequest = _context?.overTimeRequest.SingleOrDefault(cur => cur.oveid == this.oveid);


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
                var checkLeaveRequest = _context?.overTimeRequest.SingleOrDefault(cur => cur.oveid == this.oveid);


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
                var checkLeaveRequest = _context?.overTimeRequest.SingleOrDefault(cur => cur.oveid == this.oveid);


                if (checkLeaveRequest != null)
                {
                    return checkLeaveRequest.todat;
                }
                return dat1;
            }
        }

        public String overtimetimein
        {
            get
            {
                var checkLeaveRequest = _context?.overTimeRequest.SingleOrDefault(cur => cur.oveid == this.oveid);


                if (checkLeaveRequest != null)
                {
                    return checkLeaveRequest.timin;
                }
                return "";
            }
        }

        public String overtimetimeout
        {
            get
            {
                var checkLeaveRequest = _context?.overTimeRequest.SingleOrDefault(cur => cur.oveid == this.oveid);

                if (checkLeaveRequest != null)
                {
                    return checkLeaveRequest.timout;
                }
                return "";
            }
        }

        public String overtimeoverper
        {
            get
            {
                var checkLeaveRequest = _context?.overTimeRequest.SingleOrDefault(cur => cur.oveid == this.oveid);

                if (checkLeaveRequest != null)
                {
                    return checkLeaveRequest.overper;
                }
                return "";
            }
        }
    }
}
