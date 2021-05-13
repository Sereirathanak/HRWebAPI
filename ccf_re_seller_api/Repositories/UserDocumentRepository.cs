using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ccf_re_seller_api.Modals;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ccf_re_seller_api.Repositories
{
    public class UserDocumentRepository
    {
        private readonly ReSellerAPIContext _context;
        private readonly UserRepository _userRepository;

        public UserDocumentRepository(ReSellerAPIContext context, IWebHostEnvironment env)
        {
            _context = context;
            _context = context;
            this._userRepository = new UserRepository(_context, env);
        }
        //

        public virtual async Task<string[]> UploadDocument([FromForm] IFormFile file,
                                                                  string ucode,
                                                                  string dcode
                                                                 )
        {
            try
            {
                string fileName = "";
                string mineType = "";
                string fileEx = "";
                string allowExtensions = ".jpg|.jpeg|.png|.gif";
                string errMsg = "";
                string err10000 = "";
                string err50000 = "";

                if (file != null)
                {
                    fileName = file.FileName;
                    mineType = file.ContentType;
                    fileEx = Path.GetExtension(fileName);

                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);

                        // Validate File Size 10M
                        if (memoryStream.Length > 10485760)
                        {
                            errMsg = "The document size cannot bigger than 10M.";
                        }

                        if (!allowExtensions.Contains(fileEx))
                        {
                            errMsg = "The document type is not allow.";
                        }

                        if (errMsg == "")
                        {

                            var id = _context.Document.Max(c => c.dcode);
                            int convertInt = 0;
                            if (id == null)
                            {
                                convertInt = 10000;
                            }
                            else
                            {
                                convertInt = int.Parse(id) + 1;

                            }

                            var oldDocument101 = _context.Document.SingleOrDefault(u => u.ucode == ucode);
                            if (oldDocument101 == null)
                            {
                                var document101 = new Document()
                                {
                                    dcode = convertInt.ToString(),
                                    minetype = mineType,
                                    filepath = memoryStream.ToArray(),
                                    description = "",
                                    ucode = ucode
                                };
                                _context.Document.Add(document101);
                            }
                            else
                            {
                                oldDocument101.description = "";
                                oldDocument101.filepath = memoryStream.ToArray();
                            }

                        }

                        memoryStream.Close();
                        memoryStream.Dispose();
                    }
                }
                else
                {
                    errMsg = $"The document is required.";
                   
                }

                string[] results = new string[] { errMsg, err10000, err50000 };

                return results;

            }
            catch (Exception ex)
            {
                return new string[] { ex.Message.ToString() };
            }
        }
    }
}
