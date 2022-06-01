
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using ccf_re_seller_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.IO;
using ccf_re_seller_api.Modals;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ccf_re_seller_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("*", "*", "*")]
    public class HRExcelController : Controller
    {
        public IConfiguration _configuration;
        private readonly HRContext _context;

        public HRExcelController(IConfiguration config, HRContext context)
        {
            _configuration = config;
            _context = context;
        }
        //
        [HttpPost("hr/excel")]
        public async Task<ActionResult> Post([FromForm] HRExcelImport _timeClock)
        {
            //string allowExtensions = ".xls|.xlsx|.png|.gif";
            //string fileEx = "";
            //string mineType = "";
            //string fileName = "";
            //string errEduId = "";
            //string errEduIdBank = "";
            //string errEduIdselfie = "";
            //IExcelDataReader reader = null;


            //if (HttpContext.Request.Form.Files["exl[101]"] != null)
            //{
            //    //
            //    fileName = HttpContext.Request.Form.Files["exl[101]"].FileName;
            //    mineType = HttpContext.Request.Form.Files["exl[101]"].ContentType;
            //    fileEx = Path.GetExtension(fileName);

            //    //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            //    using (var memoryStream = new MemoryStream())
            //    {
            //        await HttpContext.Request.Form.Files["exl[101]"].CopyToAsync(memoryStream);

            //        // Validate File Size 10M
            //        if (memoryStream.Length > 10485760)
            //        {
            //            errEduId = "The document size cannot bigger than 10M.";
            //        }

            //        if (!allowExtensions.Contains(fileEx))
            //        {
            //            errEduId = "The document type is not allow.";
            //        }

            //        if (errEduId == "")
            //        {

            //            using (var readertes = ExcelReaderFactory.CreateReader(memoryStream))
            //            {

            //                while (readertes.Read()) //Each row of the file
            //                {
            //                    return Ok(reader.ResultsCount);


            //                    //return Ok(reader.GetValue(0).ToString());
            //                    //users.Add(new UserModel { Name = reader.GetValue(0).ToString(), City = reader.GetValue(1).ToString() });
            //                }
            //            }

            //        }

            //        memoryStream.Close();
            //        memoryStream.Dispose();
            //    }
            //}


            var fileName = "/Users/vatsopheap/Desktop/test.xlsx";
            List<dynamic> termsLst = new List<dynamic>();
            // For .net core, the next line requires the NuGet package, 
            // System.Text.Encoding.CodePages
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {

                    while (reader.Read()) //Each row of the file
                    {
                        //termsLst.Add(reader.GetValue(0).ToString());
                        return Ok(reader.GetValue(0).ToString());

                    }
                }
            }

            return Ok(termsLst);
        }

    }
}
