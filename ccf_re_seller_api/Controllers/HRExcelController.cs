using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using ccf_re_seller_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.IO;
using ccf_re_seller_api.Modals;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;

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
        [HttpPost("hr/excel2")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

            var path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Ok(path);
            return RedirectToAction("Files");
        }

        //
        [HttpPost("hr/excel1")]
        public async Task<IActionResult> TestAsync([FromForm] HRExcelImport _timeClock)
        {
            string currFileExtension = string.Empty;
            string currFilePath = string.Empty;

            string fileName = HttpContext.Request.Form.Files[0].FileName;
            string tempPath = System.IO.Path.GetTempPath(); //Get Temporary File Path  
            fileName = System.IO.Path.GetFileName(fileName); //Get File Name (not including path)  
            currFileExtension = System.IO.Path.GetExtension(fileName); //Get File Extension  
            currFilePath = tempPath + fileName; //Get File Path after Uploading and Record to Former Declared Global Variable  
            //return Ok(file);


            //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(currFilePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        while (reader.Read()) //Each ROW
                        {
                            for (int column = 0; column < reader.FieldCount; column++)
                            {
                                //Console.WriteLine(reader.GetString(column));//Will blow up if the value is decimal etc. 
                                Console.WriteLine(reader.GetValue(column));//Get Value returns object
                            }
                        }
                    } while (reader.NextResult()); //Move to NEXT SHEET
                }
            }
            //long size = file.Sum(f => f.Length);

            // full path to file in temp location
            //long size = files.Sum(f => f.Length);
            //var filePath = Path.GetTempFileName();


            //foreach (var formFile in files)
            //{
            //    if (formFile.Length > 0)
            //    {
            //        using (var stream = new FileStream(filePath, FileMode.Create))
            //        {
            //            await formFile.CopyToAsync(stream);
            //        }
            //    }
            //}

            return Ok(currFilePath);
        }

        //
        [HttpPost("hr/excel")]
        public async Task<ActionResult> Post(Microsoft.AspNetCore.Http.IFormFile file, HRExcelImport _timeClock)
        {


            return Ok(file.FileName);


            string allowExtensions = ".xls|.xlsx|.png|.gif";
            string fileEx = "";
            string mineType = "";
            string fileName = "";
            string errEduId = "";
            string errEduIdBank = "";
            string errEduIdselfie = "";
            IExcelDataReader reader = null;


            if (file != null)
            {
                //
                fileName = file.FileName;
                mineType = file.ContentType;
                fileEx = Path.GetExtension(fileName);

                //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);

                    // Validate File Size 10M
                    if (memoryStream.Length > 10485760)
                    {
                        errEduId = "The document size cannot bigger than 10M.";
                    }

                    if (!allowExtensions.Contains(fileEx))
                    {
                        errEduId = "The document type is not allow.";
                    }

                    if (errEduId == "")
                    {

                        using (var readertes = ExcelReaderFactory.CreateReader(memoryStream))
                        {


                            while (readertes.Read()) //Each row of the file
                            {
                                for (int column = 0; column < readertes.FieldCount; column++)
                                {
                                    //Console.WriteLine(reader.GetString(column));//Will blow up if the value is decimal etc. 
                                    Console.WriteLine(readertes.GetValue(column));//Get Value returns object
                                    return Ok(readertes.GetValue(column));

                                }


                                //return Ok(reader.GetValue(0).ToString());
                                //users.Add(new UserModel { Name = reader.GetValue(0).ToString(), City = reader.GetValue(1).ToString() });
                            }
                        }

                    }

                    memoryStream.Close();
                    memoryStream.Dispose();
                }
            }
            else
            {
                return Ok("else");

            }


            //var fileName = "/Users/vatsopheap/Desktop/test.xlsx";
            //List<dynamic> termsLst = new List<dynamic>();
            //// For .net core, the next line requires the NuGet package, 
            //// System.Text.Encoding.CodePages
            //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            //using (var stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read))
            //{
            //    using (var reader = ExcelReaderFactory.CreateReader(stream))
            //    {

            //        while (reader.Read()) //Each row of the file
            //        {
            //            //termsLst.Add(reader.GetValue(0).ToString());
            //            return Ok(reader.GetValue(0).ToString());

            //        }
            //    }
            //}

            return Ok(_timeClock);
        }

    }
}
