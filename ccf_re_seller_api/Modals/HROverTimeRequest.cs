using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ccf_re_seller_api.Models;
namespace ccf_re_seller_api.Modals
{
    [Table("ccfove")]
    public class HROverTimeRequest
    {
        private readonly HRContext _context;

        public HROverTimeRequest(HRContext context)
        {
            _context = context;
        }

        [Key]
        public string oveid { get; set; }

        [ForeignKey("ccforg")]
        public string orgid { get; set; }

        [ForeignKey("ccfpinfo")]
        public string eid { get; set; }

        [ForeignKey("ccfovty")]
        public string ovtyid { get; set; }

        public DateTime frdat { get; set; }
        public DateTime todat { get; set; }
        public string timin { get; set; }
        public string timout { get; set; }
        public string overper { get; set; }
        public string reason { get; set; }
        public string remark { get; set; }
        public byte[] file { get; set; }
        public DateTime createdate { get; set; }
        public string statu { get; set; }


        public virtual HROrganizationClass ccforg { get; set; }
        public virtual HROverTimeType ccfovty { get; set; }
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
        public string overTimeTypeName
        {
            get
            {
                var name = _context?.overTimeType.SingleOrDefault(cur => cur.ovtyid == this.ovtyid);
                if (name != null)
                {
                    return name.vtyp;
                }
                return "";
            }
        }
    }

    public class ValidateOverDocument
    {
        [Key]
        public string oveid { get; set; }

        public string orgid { get; set; }

        [ForeignKey("ccfpinfo")]
        public string eid { get; set; }

        public string ovtyid { get; set; }

        public DateTime frdate { get; set; }
        public DateTime todate { get; set; }
        public string timin { get; set; }
        public string timout { get; set; }

        public string overper { get; set; }
        public string reason { get; set; }
        public string remark { get; set; }

        //[Required]
        public byte[] file { get; set; }

        public DateTime createdate { get; set; }
        public string statu { get; set; }


    }
}
