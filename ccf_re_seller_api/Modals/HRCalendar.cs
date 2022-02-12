using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ccf_re_seller_api.Modals
{
    [Table("ccfcal")]
    public class HRCalendar
    {
        [Key]
        public string calid { get; set; }
        [ForeignKey("ccforg")]
        public string orgid { get; set; }
        public int mon { get; set; }
        public int tue { get; set; }
        public int wen { get; set; }
        public int thu { get; set; }
        public int fri { get; set; }
        public int sat { get; set; }
        public int sun { get; set; }
        public virtual HROrganizationClass ccforg { get; set; }

    }
}
