using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

#nullable disable

namespace ccf_re_seller_api.Modals
{
    [Table("ccfreferal_cus")]
    public partial class CcfreferalCu
    {
        private readonly ReSellerAPIContext _context;

        public CcfreferalCu(ReSellerAPIContext context)
        {
            _context = context;
        }

        [Key]
        public string cid { get; set; }

        [ForeignKey("CcfreferalRe")]
        public string refcode { get; set; }

        [ForeignKey("CcfuserRe")]
        public string uid { get; set; }
        public string cname { get; set; }
        public string phone { get; set; }
        public int? lamount { get; set; }
        public string? lpourpose { get; set; }
        public DateTime refdate { get; set; }
        public string u1 { get; set; }
        public string u2 { get; set; }
        public string u3 { get; set; }
        public string u4 { get; set; }
        public string u5 { get; set; }
        public string status { get; set; }

        public string province { get; set; }
        public string district { get; set; }
        public string commune { get; set; }
        public string village { get; set; }
        public string curcode { get; set; }


        public string Currency
        {
            get
            {
                var currency = _context?.Currency.SingleOrDefault(cur => cur.curcode == this.curcode);
                if (currency != null)
                {
                    return currency.curname;
                }
                return "";
            }
        }

        public string ProvinceName
        {
            get
            {
                var province = _context?.Addresses.Where(a => a.procode == this.province).FirstOrDefault();

                if (province == null)
                {
                    return "";
                }
                return province.prodes;

            }
        }

        public string DistrictName
        {
            get
            {
                var province = _context?.Addresses.Where(a => a.discode == this.district).FirstOrDefault();

                if (province == null)
                {
                    return "";
                }
                return province.disdes;
            }
        }

        public string CommuneName
        {
            get
            {
                var province = _context?.Addresses.Where(a => a.comcode == this.commune).FirstOrDefault();

                if (province == null)
                {
                    return "";
                }
                return province.comdes;
            }
        }

        public string VillageName
        {
            get
            {
                var province = _context?.Addresses.Where(a => a.vilcode == this.village).FirstOrDefault();

                if (province == null)
                {
                    return "";
                }
                return province.vildes;
            }
        }



        public virtual CcfreferalRe CcfreferalRe { get; set; }
        public virtual CcfuserRe CcfuserRe { get; set; }
        public virtual ICollection<CcfreferalCusUp> ccfreferalCusUp { get; set; }

    }

    
}
