using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ccf_re_seller_api.Models;

namespace ccf_re_seller_api.Modals
{
    [Table("ccftim")]
    public class HRTimeLogClass
    {
        private readonly HRContext _context;

        public HRTimeLogClass(HRContext context)
        {
            _context = context;
        }

        [Key]
        
        public string timid { get; set; }

        [ForeignKey("ccfpinfo")]
        public string eid { get; set; }

        [ForeignKey("ccfbranch")]
        public string braid { get; set; }

        public DateTime tdate { get; set; }
        public string tim { get; set; }
        public string sta { get; set; }
        public string cty { get; set; }

        public virtual HREmployee ccfpinfo { get; set; }
        public virtual HRBranchClass ccfbranch { get; set; }


        public string branchName
        {
            get
            {
                var branch = _context?.hrBranchClass.SingleOrDefault(b => b.braid == this.braid);
                if (branch != null)
                {
                    return branch.braname;
                }
                return "";
            }
        }
    }

    //
    public class HRExcelImport
    {
        [Key]
        public string timid { get; set; }
        public string eid { get; set; }
        public string braid { get; set; }
        public DateTime tdate { get; set; }
        public string tim { get; set; }
        public string sta { get; set; }
        public string cty { get; set; }

    }

    //
}
