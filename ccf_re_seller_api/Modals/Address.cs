using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ccf_re_seller_api.Modals
{
    [Table("ccfaddress")]
    public class Address
    {
        [Key]
        public string acode { get; set; }
        public string procode { get; set; }
        public string prodes { get; set; }
        public string discode { get; set; }
        public string disdes { get; set; }
        public string comcode { get; set; }
        public string comdes { get; set; }
        public string vilcode { get; set; }
        public string vildes { get; set; }
        public string u1 { get; set; }
        public string u2 { get; set; }
        public string u3 { get; set; }
        public string u4 { get; set; }
        public string u5 { get; set; }
    }

    public class ReturnProvince
    {
        public string procode { get; set; }
        public string prodes { get; set; }
    }

    public class ReturnDistrict
    {
        public string discode { get; set; }
        public string disdes { get; set; }
    }

    public class ReturnCommune
    {
        public string comcode { get; set; }
        public string comdes { get; set; }
    }

    public class ReturnVillage
    {
        public string vilcode { get; set; }
        public string vildes { get; set; }
    }
}
