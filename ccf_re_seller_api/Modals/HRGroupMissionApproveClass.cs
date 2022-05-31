using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using ccf_re_seller_api.Models;
namespace ccf_re_seller_api.Modals
{
    [Table("ccfgroupmissionapp")]
    public class HRGroupMissionApproveClass
    {
        private readonly HRContext _context;

        public HRGroupMissionApproveClass(HRContext context)
        {
            _context = context;
        }

        [Key]
        public string groupmisappid { get; set; }
        [ForeignKey("ccfgroupMissionRequest")]
        public string gmid { get; set; }
        public int applev { get; set; }
        [ForeignKey("ccfpinfo")]
        public string eid { get; set; }
        public int prio { get; set; }
        public int apstatu { get; set; }
        public string com { get; set; }
        public string remark { get; set; }
        public DateTime createdate { get; set; }

        
        public virtual HRGroupMissionClass ccfgroupMissionRequest { get; set; }
        public virtual HREmployee ccfpinfo { get; set; }

        public string employeename
        {
            get
            {
                var name = _context?.employee.SingleOrDefault(cur => cur.eid == this.eid);
                if (name != null)
                {
                    return name.dname;
                }
                return "";
            }
        }

        //public string employeerequestid
        //{
        //    get
        //    {
        //        var checkLeaveRequest = _context?.groupMissionRequest.SingleOrDefault(cur => cur.gmid == this.gmid);
        //        var checkLeaveDetail = _context?.groupMissionDetailClass.SingleOrDefault(cur => cur.gmid == checkLeaveRequest.gmid);

        //        var name = _context?.employee.SingleOrDefault(cur => cur.eid == checkLeaveDetail.eid);

        //        if (name != null)
        //        {
        //            return name.eid;
        //        }
        //        return "";
        //    }
        //}

        //public string missonreason
        //{
        //    get
        //    {
        //        var checkLeaveRequest = _context?.groupMissionRequest.SingleOrDefault(cur => cur.gmid == this.gmid);


        //        if (checkLeaveRequest != null)
        //        {
        //            return checkLeaveRequest.reason;
        //        }
        //        return "";
        //    }
        //}

        //public DateTime missionfrdate
        //{
        //    get
        //    {
        //        var checkLeaveRequest = _context?.groupMissionRequest.SingleOrDefault(cur => cur.gmid == this.gmid);

        //        var dat1 = new DateTime();
        //        if (checkLeaveRequest != null)
        //        {
        //            return checkLeaveRequest.frdate;
        //        }
        //        return dat1;
        //    }
        //}

        //public DateTime missiontodato
        //{
        //    get
        //    {
        //        var dat1 = new DateTime();
        //        var checkLeaveRequest = _context?.groupMissionRequest.SingleOrDefault(cur => cur.gmid == this.gmid);

        //        if (checkLeaveRequest != null)
        //        {
        //            return checkLeaveRequest.todate;
        //        }
        //        return dat1;
        //    }
        //}

        //public String missiondep
        //{
        //    get
        //    {
        //        var checkLeaveRequest = _context?.groupMissionRequest.SingleOrDefault(cur => cur.gmid == this.gmid);


        //        if (checkLeaveRequest != null)
        //        {
        //            return checkLeaveRequest.dep;
        //        }
        //        return "";
        //    }
        //}

        //public String missionarr
        //{
        //    get
        //    {
        //        var checkLeaveRequest = _context?.groupMissionRequest.SingleOrDefault(cur => cur.gmid == this.gmid);


        //        if (checkLeaveRequest != null)
        //        {
        //            return checkLeaveRequest.arr;
        //        }
        //        return "";
        //    }
        //}

        //public String missionfor
        //{
        //    get
        //    {
        //        var checkLeaveRequest = _context?.groupMissionRequest.SingleOrDefault(cur => cur.gmid == this.gmid);

        //        if (checkLeaveRequest != null)
        //        {
        //            return checkLeaveRequest.mfor;
        //        }
        //        return "";
        //    }
        //}

        //public String missionnote
        //{
        //    get
        //    {
        //        var checkLeaveRequest = _context?.groupMissionRequest.SingleOrDefault(cur => cur.gmid == this.gmid);

        //        if (checkLeaveRequest != null)
        //        {
        //            return checkLeaveRequest.lnot;
        //        }
        //        return "";
        //    }
        //}
        //public String missionoverper
        //{
        //    get
        //    {
        //        var checkLeaveRequest = _context?.groupMissionRequest.SingleOrDefault(cur => cur.gmid == this.gmid);

        //        if (checkLeaveRequest != null)
        //        {
        //            return checkLeaveRequest.overper;
        //        }
        //        return "";
        //    }
        //}
        //public String missionname
        //{
        //    get
        //    {
        //        var checkLeaveRequest = _context?.groupMissionRequest.SingleOrDefault(cur => cur.gmid == this.gmid);

        //        if (checkLeaveRequest != null)
        //        {
        //            return checkLeaveRequest.missu;
        //        }
        //        return "";
        //    }
        //}

        //public String missiontypeid
        //{
        //    get
        //    {
        //        var checkLeaveRequest = _context?.groupMissionRequest.SingleOrDefault(cur => cur.gmid == this.gmid);

        //        if (checkLeaveRequest != null)
        //        {
        //            return checkLeaveRequest.misid;
        //        }
        //        return "";
        //    }
        //}
        //public String groupMissionRequestuestid
        //{
        //    get
        //    {
        //        var checkLeaveRequest = _context?.groupMissionRequest.SingleOrDefault(cur => cur.gmid == this.gmid);

        //        if (checkLeaveRequest != null)
        //        {
        //            return checkLeaveRequest.gmid;
        //        }
        //        return "";
        //    }
        //}

        //public String missiontypename
        //{
        //    get
        //    {
        //        var checkLeaveRequest = _context?.groupMissionRequest.SingleOrDefault(cur => cur.gmid == this.gmid);

        //        var missiontype = _context?.mssionType.SingleOrDefault(cur => cur.misid == checkLeaveRequest.misid);


        //        if (missiontype != null)
        //        {
        //            return missiontype.mistp;
        //        }
        //        return "";
        //    }
        //}

        //public DateTime missioncreate
        //{
        //    get
        //    {
        //        var checkLeaveRequest = _context?.groupMissionRequest.SingleOrDefault(cur => cur.gmid == this.gmid);
        //        var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //        DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));
        //        if (checkLeaveRequest != null)
        //        {
        //            return checkLeaveRequest.createdate;
        //        }
        //        return DOI;
        //    }
        //}


        //public string missionstatus
        //{
        //    get
        //    {
        //        var checkLeaveRequest = _context?.groupMissionRequest.SingleOrDefault(cur => cur.gmid == this.gmid);

        //        if (checkLeaveRequest != null)
        //        {
        //            return checkLeaveRequest.statu;
        //        }
        //        return "";
        //    }
        //}
    }
}
