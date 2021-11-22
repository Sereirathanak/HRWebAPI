using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ccf_re_seller_api.Models
{
    [Table("efam")]
    public class HREmployeeFamily
    {
        [Key]
        public string famid { get; set; }
        [ForeignKey("pinfo")]
        public string eid { get; set; }
        public string fname { get; set; }
        public string rtype { get; set; }
        public string famstatus { get; set; }
        public string photo { get; set; }
        public string rmark { get; set; }

    }
}
