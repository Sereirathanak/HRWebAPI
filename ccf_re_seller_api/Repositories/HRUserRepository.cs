////using System;
////namespace ccf_hr_management_api.Repositories
////{
////    public class UserRepository
////    {
////        public UserRepository()
////        {
////        }
////    }
////}

//using FirebaseAdmin;
//using FirebaseAdmin.Messaging;
//using Google.Apis.Auth.OAuth2;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Globalization;
//using System.Web.Http.Cors;
//using ccf_re_seller_api.Modals;
//using ccf_re_seller_api.Models;

//namespace ccf_re_seller_api.Repositories
//{
//    [EnableCors("*", "*", "*")]
//    public class HRUserRepository
//    {

//        private readonly HRContext _context;
//        private readonly IWebHostEnvironment _env;

//        public HRUserRepository(HRContext context, IWebHostEnvironment env)
//        {
//            _context = context;
//            _env = env;
//        }

       
//        //

//        public virtual async Task<dynamic> SendNotificationUser(string uid, string bcode, string title, string body,  string lcode, string rcode)
//        {

//            return "";
//            //try
//            //{

//            //    var userLevelC = await _context.ccfUserClass
//            //                              .Where(u => u.uid != uid)
//            //                              .Where(u => u.ustatus == "A")
//            //                              .AsQueryable()
//            //                              .ToListAsync();


//            //    // Check C Level
//            //    if (userLevelC != null)
//            //    {
//            //        List<string> tokenCList = new List<string>();
//            //        List<string> userCList = new List<string>();

//            //        foreach (var user in userLevelC)
//            //        {
//            //            Console.WriteLine(user.mtoken);

//            //            if (user.mtoken != null && user.mtoken != "" && user.mtoken != "null")
//            //            {
//            //                Console.WriteLine(user.mtoken);

//            //                tokenCList.Add(user.mtoken);
//            //                userCList.Add(user.uid);
//            //            }
//            //        }

//            //        if (tokenCList.Count > 0)
//            //        {
//            //            // Remove duplicated token & ucode
//            //            tokenCList = tokenCList.Distinct().ToList();
//            //            userCList = userCList.Distinct().ToList();
//            //            //await SendNotification(tokenCList, title, body);
//            //            //await SaveMessage(userCList, bcode,  title, body, lcode, rcode );

//            //        }
//            //    }
//            //}
//            //catch
//            //{
//            //    throw;
//            //}

//            //// Final Return
//            //return "";
//        }

        
//    }
//}
