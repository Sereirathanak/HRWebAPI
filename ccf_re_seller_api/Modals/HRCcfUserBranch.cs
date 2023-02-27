using System;
using System.ComponentModel.DataAnnotations;

namespace ccf_re_seller_api.Modals
{
    public class HRCcfUserBranch
    {
        public HRCcfUserBranch()
        {
        }
        public HRCcfUserBranch(string eid,string branch)
        {
            this.eid = eid;
            this.branch = branch;
        }
        [Key]
        public int id { get; set; }
        public string eid { get; set; }
        public string branch { get; set; }
    }
}

