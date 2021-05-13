using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ccf_re_seller_api.Modals
{
    [Table("ccfducment")]
    public class Document
    {
        [Key]
        public string dcode { get; set; }
        public string description { get; set; }
        //[Required]
        [DataType(DataType.Upload)]
        public byte[] filepath { get; set; }
        public DateTime datecreate { get; set; }

        [ForeignKey("CcfuserRe")]
        public string ucode { get; set; }
        public string minetype { get; set; }
        public string type { get; set; }

        public string u1 { get; set; }
        public string u2 { get; set; }
        public string u3 { get; set; }
        public string u4 { get; set; }
        public string u5 { get; set; }

        public virtual CcfuserRe CcfuserRe { get; set; }

        public string base64FilePath
        {
            get
            {
                string fileBase64 = Convert.ToBase64String(this.filepath);
                return string.Format("data:image/jpg;base64,{0}", fileBase64);
            }
        }

    }

    public class ValidateUserDocument
    {
        [Key]
        public string dcode { get; set; }

        public string type { get; set; }

        public string ucode { get; set; }

        public string description { get; set; }

        public string typeaccountbank { get; set; }
        public string typeaccountnumber { get; set; }
        public string idtype { get; set; }
        public string idnumber { get; set; }

        public string dob { get; set; }
        public string verifystatus { get; set; }
        public string phone { get; set; }
        public string username { get; set; }
        public string gender { get; set; }

        public string status { get; set; }

        //[Required]
        public byte[] kyc { get; set; }
        public byte[] employee { get; set; }
        public byte[] businessOwnership { get; set; }
        public byte[] collateral { get; set; }
    }


    public class ReturnDocumentByLoan
    {
        public string dcode { get; set; }
        public string type { get; set; }
        public string uid { get; set; }
        public string description { get; set; }
        public string filepath { get; set; }
    }

   


}
