using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ccf_re_seller_api.Models
{
    [Table("edoc")]
    public class HREmployeeDocument
    {
        [Key]
        public string docid { get; set; }
        [ForeignKey("pinfo")]
        public string eid { get; set; }
        public string doctype { get; set; }
        public string docnum { get; set; }
        public string edate { get; set; }
        public string docatt { get; set; }
        public string rmark { get; set; }

    }
}
