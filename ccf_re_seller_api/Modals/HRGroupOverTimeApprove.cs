using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ccf_re_seller_api.Models;
namespace ccf_re_seller_api.Modals
{
    
    [Table("ccfgroupoveapp")]
    public class HRGroupOverTimeApprove
    {
        private readonly HRContext _context;
        public HRGroupOverTimeApprove(HRContext context)
        {
            _context = context;
        }
        //
        [Key]
        public string groupoveapp { get; set; }

        [ForeignKey("ccfgroupovereq")]
        public string groupoveid { get; set; }
        public int applev { get; set; }
        [ForeignKey("ccfpinfo")]
        public string eid { get; set; }
        public int prio { get; set; }
        public int apstatu { get; set; }
        public string com { get; set; }
        public DateTime createdate { get; set; }


        public virtual HRGroupOverTimeRequest ccfgroupovereq { get; set; }
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
    }
}
