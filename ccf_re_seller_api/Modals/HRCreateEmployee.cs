using System;
namespace ccf_re_seller_api.Models
{
    public class HRCreateEmployee
    {
        public string? eid { get; set; }
        public string? ecard { get; set; }
        public string? fname { get; set; }
        public string? lname { get; set; }
        public string? bcode { get; set; }
        public string? dname { get; set; }
        public DateTime dob { get; set; }
        public string? mstatus { get; set; }
        public string? gender { get; set; }
        public string? nat { get; set; }
        public string? blo { get; set; }
        public string? reg { get; set; }
        public string? pba { get; set; }
        public string? etn { get; set; }
        public string? hnum { get; set; }
        public string? rnum { get; set; }
        public string? village { get; set; }
        public string? district { get; set; }
        public string? commune { get; set; }
        public string? province { get; set; }
        public string? dadd { get; set; }
        public string? pnum { get; set; }
        public string? fnum { get; set; }
        public string? email { get; set; }
        public DateTime rdate { get; set; }
        public string? estatus { get; set; }
        public string? photo { get; set; }
        public int elevel { get; set; }
        public string? isapprover { get; set; }

        

        //Employee Join Info

        public string? jonid { get; set; }
        public DateTime jdate { get; set; }
        public string? site { get; set; }
        public string? dep { get; set; }
        public string? pos { get; set; }
        public string? sup { get; set; }
        public decimal pmsal { get; set; }
        public double msal { get; set; }
        public string? stype { get; set; }
        public string? con { get; set; }
        public string? etype { get; set; }
        public int npm { get; set; }
        public DateTime edate { get; set; }
        public DateTime ecdate { get; set; }
        public string? status { get; set; }
        public string? remark { get; set; }
        public string? changepassword { get; set; }
        public string? incharge { get; set; }

    }
}
