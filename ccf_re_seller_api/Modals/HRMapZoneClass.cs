using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ccf_re_seller_api.Models;

namespace ccf_re_seller_api.Modals
{
    [Table("ccfmapzone")]
    public class HRMapZoneClass
    {
        //private readonly HRContext _context;

        //public HRMapZoneClass(HRContext context)
        //{
        //    _context = context;
        //}

        [Key]
        public string zoneid { get; set; }
        [ForeignKey("ccfbranch")]
        public string braid { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string remark { get; set; }
        public DateTime datecreate { get; set; }
        public string cby { get; set; }


        public virtual HRBranchClass ccfbranch { get; set; }

        //public string branchname
        //{
        //    get
        //    {
        //        var name = _context.mapZoneClass.SingleOrDefault(cur => cur.braid == this.braid);

        //        if (name != null)
        //        {
        //            return name.branchname;
        //        }
        //        return "";
        //    }
        //}
    }
}
