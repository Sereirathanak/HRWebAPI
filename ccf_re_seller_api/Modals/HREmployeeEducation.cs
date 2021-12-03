using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ccf_re_seller_api.Models
{
    [Table("eedu")]
    public class HREmployeeEducation
    {
        [Key]
        public string eduid { get; set; }
        [ForeignKey("pinfo")]
        public string eid { get; set; }
        public string inst { get; set; }
        public string sub { get; set; }
        public DateTime sdate { get; set; }
        public DateTime edate { get; set; }
        public byte[] certyfi { get; set; }
        public string remark { get; set; }
        public virtual HREmployee pinfo { get; set; }


    }

    public class HRValidateEmployeeEducation
    {
        [Key]
        public string eduid { get; set; }

        public string eid { get; set; }

        public string inst { get; set; }

        public string sub { get; set; }

        public DateTime sdate { get; set; }

        public DateTime edate { get; set; }

        public byte[] certyfi { get; set; }

        public string remark { get; set; }

    }

    public class ReturnHRValidateEmployeeEducation
    {
        public string eduid { get; set; }
        public string eid { get; set; }
        public string inst { get; set; }
        public string sub { get; set; }
        public DateTime sdate { get; set; }
        public DateTime edate { get; set; }
        public string remark { get; set; }
        public string filepath { get; set; }
    }
}
