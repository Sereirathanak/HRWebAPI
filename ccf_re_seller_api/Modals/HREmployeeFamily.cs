using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ccf_re_seller_api.Modals;

namespace ccf_re_seller_api.Models
{
    [Table("ccfefam")]
    public class HREmployeeFamily
    {
        [Key]
        public string famid { get; set; }
        [ForeignKey("ccfpinfo")]
        public string eid { get; set; }
        public string fname { get; set; }
        public string rtype { get; set; }
        public string famstatus { get; set; }

        public byte[] photo { get; set; }
        public string rmark { get; set; }
        public virtual HRTimeLogClass ccfpinfo { get; set; }

    }

    public class HRValidateEmployeeFamily
    {
        [Key]
        public string famid { get; set; }
        public string eid { get; set; }
        public string fname { get; set; }
        public string rtype { get; set; }
        public string famstatus { get; set; }
        public byte[] photo { get; set; }
        public string rmark { get; set; }

    }

    public class ReturnDocumentByEmployeeFamily
    {
        public string famid { get; set; }
        public string eid { get; set; }
        public string fname { get; set; }
        public string rtype { get; set; }
        public string famstatus { get; set; }
        public string rmark { get; set; }
        public string filepath { get; set; }
    }
}
