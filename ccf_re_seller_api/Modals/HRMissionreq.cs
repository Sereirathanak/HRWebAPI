using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ccf_re_seller_api.Models;


namespace ccf_re_seller_api.Modals
{
    [Table("ccfmissionreq")]
    public class HRMissionreq
    {
        private readonly HRContext _context;

        public HRMissionreq(HRContext context)
        {
            _context = context;
        }

        [Key]
        public string misnid { get; set; }

        [ForeignKey("ccforg")]
        public string orgid { get; set; }

        [ForeignKey("ccfpinfo")]
        public string eid { get; set; }

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

        
        public virtual HROrganizationClass ccforg { get; set; }
        public virtual HREmployee ccfpinfo { get; set; }
        public virtual HRMissionType ccfmis { get; set; }

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

    public class ValidateMiossionDocument
    {
        [Key]
        public string misnid { get; set; }

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

        //[Required]
        public byte[] file { get; set; }
   
    }


   
}
