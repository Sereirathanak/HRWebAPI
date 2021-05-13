using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ccf_re_seller_api.Modals
{
   
    [Table("transition")]
    public class Transition
    {

        private readonly ReSellerAPIContext _context;

        public Transition(ReSellerAPIContext context)
        {
            _context = context;
        }

        [Key]
        public string tid { get; set; }
        public string cid { get; set; }
        public string refcode { get; set; }
        public string transitiontype { get; set; }
        public string approvecode { get; set; }
        public DateTime datecreate { get; set; }
        public string status { get; set; }
        public string typeaccountbank { get; set; }
        public string accountbank { get; set; }
        public int amount { get; set; }
        public string currencycode { get; set; }
        public string u1 { get; set; }
        public string u2 { get; set; }
        public string u3 { get; set; }
        public string u4 { get; set; }
        public string u5 { get; set; }


        public string Currency
        {
            get
            {
                var currency = _context?.Currency.SingleOrDefault(cur => cur.curcode == this.currencycode);
                if (currency != null)
                {
                    return currency.curname;
                }
                return "";
            }
        }

    }
}
