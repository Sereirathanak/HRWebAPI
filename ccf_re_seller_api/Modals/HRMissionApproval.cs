using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using ccf_re_seller_api.Models;
namespace ccf_re_seller_api.Modals
{
    [Table("ccfmisd")]
    public class HRMissionApproval
    {
        private readonly HRContext _context;

        public HRMissionApproval(HRContext context)
        {
            _context = context;
        }

        [Key]
        public string misdid { get; set; }

        [ForeignKey("ccfmissionreq")]
        public string misnid { get; set; }
        public int applev { get; set; }
        [ForeignKey("ccfpinfo")]
        public string eid { get; set; }
        public int prio { get; set; }
        public int apstatu { get; set; }
        public string com { get; set; }
        public string remark { get; set; }

        public virtual HRMissionreq ccfmissionreq { get; set; }
        public virtual HREmployee ccfpinfo { get; set; }

        public string employeename
        {
            get
            {
                var checkLeaveRequest = _context?.missionreq.SingleOrDefault(cur => cur.misnid == this.misnid);
                var name = _context?.employee.SingleOrDefault(cur => cur.eid == checkLeaveRequest.eid);

                if (name != null)
                {
                    return name.dname;
                }
                return "";
            }
        }

        public string employeerequestid
        {
            get
            {
                var checkLeaveRequest = _context?.missionreq.SingleOrDefault(cur => cur.misnid == this.misnid);
                var name = _context?.employee.SingleOrDefault(cur => cur.eid == checkLeaveRequest.eid);

                if (name != null)
                {
                    return name.eid;
                }
                return "";
            }
        }

        public string missonreason
        {
            get
            {
                var checkLeaveRequest = _context?.missionreq.SingleOrDefault(cur => cur.misnid == this.misnid);


                if (checkLeaveRequest != null)
                {
                    return checkLeaveRequest.reason;
                }
                return "";
            }
        }

        public DateTime missionfrdate
        {
            get
            {
                var checkLeaveRequest = _context?.missionreq.SingleOrDefault(cur => cur.misnid == this.misnid);

                var dat1 = new DateTime();
                if (checkLeaveRequest != null)
                {
                    return checkLeaveRequest.frdate;
                }
                return dat1;
            }
        }

        public DateTime missiontodato
        {
            get
            {
                var dat1 = new DateTime();
                var checkLeaveRequest = _context?.missionreq.SingleOrDefault(cur => cur.misnid == this.misnid);

                if (checkLeaveRequest != null)
                {
                    return checkLeaveRequest.todate;
                }
                return dat1;
            }
        }

        public String missiondep
        {
            get
            {
                var checkLeaveRequest = _context?.missionreq.SingleOrDefault(cur => cur.misnid == this.misnid);


                if (checkLeaveRequest != null)
                {
                    return checkLeaveRequest.dep;
                }
                return "";
            }
        }

        public String missionarr
        {
            get
            {
                var checkLeaveRequest = _context?.missionreq.SingleOrDefault(cur => cur.misnid == this.misnid);


                if (checkLeaveRequest != null)
                {
                    return checkLeaveRequest.arr;
                }
                return "";
            }
        }

        public String missionfor
        {
            get
            {
                var checkLeaveRequest = _context?.missionreq.SingleOrDefault(cur => cur.misnid == this.misnid);

                if (checkLeaveRequest != null)
                {
                    return checkLeaveRequest.mfor;
                }
                return "";
            }
        }

        public String missionnote
        {
            get
            {
                var checkLeaveRequest = _context?.missionreq.SingleOrDefault(cur => cur.misnid == this.misnid);

                if (checkLeaveRequest != null)
                {
                    return checkLeaveRequest.lnot;
                }
                return "";
            }
        }
        public String missionoverper
        {
            get
            {
                var checkLeaveRequest = _context?.missionreq.SingleOrDefault(cur => cur.misnid == this.misnid);

                if (checkLeaveRequest != null)
                {
                    return checkLeaveRequest.overper;
                }
                return "";
            }
        }
        public String missionname
        {
            get
            {
                var checkLeaveRequest = _context?.missionreq.SingleOrDefault(cur => cur.misnid == this.misnid);

                if (checkLeaveRequest != null)
                {
                    return checkLeaveRequest.missu;
                }
                return "";
            }
        }

        public String missiontypeid
        {
            get
            {
                var checkLeaveRequest = _context?.missionreq.SingleOrDefault(cur => cur.misnid == this.misnid);

                if (checkLeaveRequest != null)
                {
                    return checkLeaveRequest.misid;
                }
                return "";
            }
        }
        public String missionrequestid
        {
            get
            {
                var checkLeaveRequest = _context?.missionreq.SingleOrDefault(cur => cur.misnid == this.misnid);

                if (checkLeaveRequest != null)
                {
                    return checkLeaveRequest.misnid;
                }
                return "";
            }
        }

        public String missiontypename
        {
            get
            {
                var checkLeaveRequest = _context?.missionreq.SingleOrDefault(cur => cur.misnid == this.misnid);

                var missiontype = _context?.mssionType.SingleOrDefault(cur => cur.misid == checkLeaveRequest.misid);


                if (missiontype != null)
                {
                    return missiontype.mistp;
                }
                return "";
            }
        }

        public DateTime missioncreate
        {
            get
            {
                var checkLeaveRequest = _context?.missionreq.SingleOrDefault(cur => cur.misnid == this.misnid);
                var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));
                if (checkLeaveRequest != null)
                {
                    return checkLeaveRequest.createdate;
                }
                return DOI;
            }
        }


        public string missionstatus
        {
            get
            {
                var checkLeaveRequest = _context?.missionreq.SingleOrDefault(cur => cur.misnid == this.misnid);
               
                if (checkLeaveRequest != null)
                {
                    return checkLeaveRequest.statu;
                }
                return "";
            }
        }

    }
   
}
