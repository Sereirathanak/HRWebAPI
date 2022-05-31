using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ccf_re_seller_api.Models;

namespace ccf_re_seller_api.Modals
{
    [Table("ccfgroupmissionreq")]
    public class HRGroupMissionClass
    {
        private readonly HRContext _context;

        public HRGroupMissionClass(HRContext context)
        {
            _context = context;
        }

        [Key]
        public string gmid { get; set; }

        [ForeignKey("ccforg")]
        public string orgid { get; set; }

        [ForeignKey("ccfmis")]
        public string misid { get; set; }

        public string missu { get; set; }
        public DateTime frdate { get; set; }
        public DateTime todate { get; set; }
        public string dep { get; set; }
        public string arr { get; set; }
        public string mfor { get; set; }
        public string lnot { get; set; }
        public string overper { get; set; }
        public string reason { get; set; }
        public string remark { get; set; }
        public byte[] file { get; set; }
        public DateTime createdate { get; set; }
        public string statu { get; set; }

        public virtual ICollection<HRGroupMissionDetailClass> groupMissionDetail { get; set; }
        public virtual ICollection<HRGroupMissionApproveClass> groupMissionApprove { get; set; }




        public virtual HROrganizationClass ccforg { get; set; }
        public virtual HRMissionType ccfmis { get; set; }

        //public string employeeName
        //{
        //    get
        //    {

        //        var nameEmployee = _context?.groupMissionDetailClass.Where(cur => cur.gmid == this.gmid);
        //        foreach (var detail in nameEmployee)
        //        {
        //            if (detail.employeename != null)
        //            {
        //                return detail.employeename;
        //            }
        //        }

        //        return "";
        //    }
        //}

        public string missionTypeName
        {
            get
            {
                var name = _context?.mssionType.SingleOrDefault(cur => cur.misid == this.misid);
                if (name != null)
                {
                    return name.mistp;
                }
                return "";
            }
        }
    }

    public class ValidateGroupMiossionDocument
    {
        [Key]
        public string gmid { get; set; }

        public string orgid { get; set; }

        [ForeignKey("ccfpinfo")]
        public string eid { get; set; }

        public string misid { get; set; }

        public string missu { get; set; }
        public DateTime frdate { get; set; }
        public DateTime todate { get; set; }
        public string dep { get; set; }

        public string arr { get; set; }
        public string mfor { get; set; }
        public string lnot { get; set; }
        public string overper { get; set; }
        public string reason { get; set; }

        public string remark { get; set; }
        public DateTime createdate { get; set; }
        public string statu { get; set; }

        //[Required]
        public byte[] file { get; set; }
        public virtual ICollection<HRGroupMissionDetailClass> groupMissionDetail { get; set; }
        public virtual ICollection<HRGroupMissionApproveClass> groupMissionApprove { get; set; }


    }
}
