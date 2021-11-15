using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ccf_re_seller_api.Modals;
using CCFReSeller;
using System.Globalization;
using System.Web.Http.Cors;

namespace ccf_re_seller_api.Repositories
{
    [EnableCors("*", "*", "*")]
    public class UserRepository
    {
        private readonly ReSellerAPIContext _context;

        private readonly IWebHostEnvironment _env;

        public UserRepository(ReSellerAPIContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        //public virtual bool UserExists(string id)
        //{
        //    return _context.Users.Any(e => e.ucode == id);
        //}

        //public virtual bool RoleExists(string id)
        //{
        //    return _context.Roles.Any(e => e.rcode == id);
        //}

        //public virtual int GetNextAssignId()
        //{
        //    var assignedRole = _context.AssignUserRoles.OrderByDescending(u => u.id).FirstOrDefault();

        //    if (assignedRole == null)
        //    {
        //        return 1;
        //    }
        //    var nextId = assignedRole.id + 1;
        //    return nextId;
        //}

        public virtual async Task<List<CcfuserRe>> GetUserObj(CustomerFilter filter)
        {
            var users = _context.CcfuserRes.AsQueryable();
            return await users.ToListAsync();
        }

        //public virtual List<string> GetRoleObjByUser(string id)
        //{
        //    try
        //    {
        //        List<string> roleList = new List<string>();
        //        var assignUserRole = _context.AssignUserRoles
        //                                     .Where(r => r.ucode == id.ToString())
        //                                     .AsQueryable();

        //        if (assignUserRole == null)
        //        {
        //            roleList.Add("0");
        //            return roleList;
        //        }

        //        var results = assignUserRole.Select(r => new ReturnAssign()
        //        {
        //            rcode = r.rcode
        //        })
        //        .ToList();

        //        for (int i = 0; i < results.Count; i++)
        //        {
        //            roleList.Add(results[i].rcode);
        //        }

        //        return roleList;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public virtual async Task<dynamic> SendNotification(List<string> clientToken, string title, string body)
        {
            var curDate = DateTime.Now.ToString();
            var Mins = UniqueId.CreateRandomId().ToString();
            var appName = title + curDate + Mins;
            var registeredToken = clientToken;
            // Google Firebase
            var googleCredential = _env.ContentRootPath;
            //mac
            //googleCredential = googleCredential + "//ccf-re-seller-app-firebase-adminsdk-khxlw-681bd6ea44.json";

            //window
            googleCredential = googleCredential + "\\ccf-re-seller-app-firebase-adminsdk-khxlw-681bd6ea44.json";

            // I suggest to create by specifying a name for instance while we have multiple projects to setup.
            var app = FirebaseApp.Create(new AppOptions
            {
                ProjectId = "73510770668",
                Credential = GoogleCredential.FromFile(googleCredential)
            }, appName);

            var message = new MulticastMessage
            {
                // payload data, some additional information for device
                Data = new Dictionary<string, string>
                {
                    { "additional_data", "" },
                    { "another_data", "" }
                },
                // the notification itself
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Tokens = registeredToken,
            };
            var firebaseMessagingInstance = FirebaseMessaging.GetMessaging(app);
            await firebaseMessagingInstance.SendMulticastAsync(message).ConfigureAwait(false);
            return true;
        }

        public virtual async Task<dynamic> SendNotificationInternal(List<string> clientToken, string title, string body)
        {
            var curDate = DateTime.Now.ToString();
            var Mins = UniqueId.CreateRandomId().ToString();
            var appName = title + curDate + Mins;
            var registeredToken = clientToken;
            Console.WriteLine(clientToken);
            // Google Firebase
            var googleCredential = _env.ContentRootPath;
            //mac
            //googleCredential = googleCredential + "//chokchey-reseller-app-5a4ab-firebase-adminsdk-rejho-48b8595ed9.json";

            //window
            googleCredential = googleCredential + "\\chokchey-reseller-app-5a4ab-firebase-adminsdk-rejho-48b8595ed9.json";

            // I suggest to create by specifying a name for instance while we have multiple projects to setup.
            var app = FirebaseApp.Create(new AppOptions
            {
                ProjectId = "chokchey-reseller-app-5a4ab",
                Credential = GoogleCredential.FromFile(googleCredential)
            }, appName);

            var message = new MulticastMessage
            {
                // payload data, some additional information for device
                Data = new Dictionary<string, string>
                {
                    { "additional_data", "" },
                    { "another_data", "" }
                },
                // the notification itself
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Tokens = registeredToken,
            };
            var firebaseMessagingInstance = FirebaseMessaging.GetMessaging(app);
            await firebaseMessagingInstance.SendMulticastAsync(message);
            return true;
        }

        public virtual async Task<dynamic> SendNotificationInternalWebApplication(List<string> clientToken, string title, string body)
        {
            var curDate = DateTime.Now.ToString();
            var Mins = UniqueId.CreateRandomId().ToString();
            var appName = title + curDate + Mins;
            var registeredToken = clientToken;
            // Google Firebase
            var googleCredential = _env.ContentRootPath;
            //mac
            //googleCredential = googleCredential + "//ccf-reseller-web-app-firebase-adminsdk-cjlin-7306caa621.json";

            //window
            googleCredential = googleCredential + "\\ccf-reseller-web-app-firebase-adminsdk-cjlin-7306caa621.json";

            // I suggest to create by specifying a name for instance while we have multiple projects to setup.
            var app = FirebaseApp.Create(new AppOptions
            {
                ProjectId = "808239604896",
                Credential = GoogleCredential.FromFile(googleCredential)
            }, appName);

            var message = new MulticastMessage
            {
                // payload data, some additional information for device
                Data = new Dictionary<string, string>
                {
                    { "additional_data", "" },
                    { "another_data", "" }
                },
                // the notification itself
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Tokens = registeredToken,
            };
            var firebaseMessagingInstance = FirebaseMessaging.GetMessaging(app);
            await firebaseMessagingInstance.SendMulticastAsync(message).ConfigureAwait(false);

            return true;
        }

        //

        public virtual async Task<dynamic> SendNotificationAssignedUser(string title, string body, string uid, string cid, string cname, string lamount, DateTime refdate, string refcode, string phone, string postion)
        {

            var listReferer = _context.CcfreferalRes.SingleOrDefault(u => u.refcode == refcode);


            var userLevelC = await _context.CcfuserRes
                                    .Where(u => u.uid != uid)
                                    .Where(u => u.ustatus == Constant.ACTIVE)
                                    .Where(u => u.uid == listReferer.uid || u.level > 0)
                                    .AsQueryable()
                                    .ToListAsync();

            // Check C Level
            if (userLevelC != null)
            {
                List<string> tokenCList = new List<string>();
                List<string> userCList = new List<string>();

                foreach (var user in userLevelC)
                {
                    if (user.mtoken != null && user.mtoken != "")
                    {
                        tokenCList.Add(user.mtoken);
                        userCList.Add(user.uid);
                    }
                }

                if (tokenCList.Count > 0)
                {
                    // Remove duplicated token & ucode
                    tokenCList = tokenCList.Distinct().ToList();
                    userCList = userCList.Distinct().ToList();

                    await SendNotificationInternal(tokenCList, title, body);
                    await SendNotification(tokenCList, title, body);
                    await SendNotificationInternalWebApplication(tokenCList, title, body);


                    await SaveMessage(userCList, title, body, cid, cname, lamount, refdate, phone, postion);
                }
            }

            // Final Return
            return "";
        }
        //
        public virtual async Task<dynamic> SendNotificationCreateReferer(string title, string body, string uid, string cid, string cname, string lamount, DateTime refdate, string phone, string postion)
        {
           
            try {

                var userLevelC = await _context.CcfuserRes
                                          .Where(u => u.uid != uid)
                                          .Where(u => u.ustatus == Constant.ACTIVE)
                                          .Where(u => u.level == 4)
                                          .AsQueryable()
                                          .ToListAsync();


                    // Check C Level
               if (userLevelC != null)
                {
                    List<string> tokenCList = new List<string>();
                    List<string> userCList = new List<string>();

                    foreach (var user in userLevelC)
                    {
                        Console.WriteLine(user.mtoken);

                        if (user.mtoken != null && user.mtoken != "" && user.mtoken != "null")
                        {
                            Console.WriteLine(user.mtoken);

                            tokenCList.Add(user.mtoken);
                            userCList.Add(user.uid);
                        }
                    }

                    if (tokenCList.Count > 0)
                    {
                        // Remove duplicated token & ucode
                        tokenCList = tokenCList.Distinct().ToList();
                        userCList = userCList.Distinct().ToList();
                        await SendNotification(tokenCList, title, body);
                        await SendNotificationInternal(tokenCList, title, body);
                        await SendNotificationInternalWebApplication(tokenCList, title, body);
                        await SaveMessage(userCList, title, body, cid, cname, lamount, refdate, phone, postion);
                    }
                }
            } catch {
                throw;
            }



            // Final Return
            return "";
        }

        public virtual async Task<dynamic> SaveMessage(List<string> userList, string title, string body, string cid, string cname, string lamount, DateTime refdate, string phone, string postion)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            // Save Message to Database
            if (userList.Count() > 0)
            {
                var dateNow = DateTime.Now.ToString("dd MMM h:mm tt");
                var NextId = await this.GetNextMessageID();
                var convertStartDate = refdate.ToString("yyyy-MM-dd");
                IList<CcfmessagesRe> newMessage = new List<CcfmessagesRe>();
                for (int i = 0; i < userList.Count(); i++)
                {
                    newMessage.Add(new CcfmessagesRe()
                    {
                        id = NextId.ToString(),
                        ucode = userList[i],
                        cid = cid,
                        title = title,
                        body = body,
                        cname = cname,
                        lamount = lamount,
                        mstatus = 0,
                        startdate = DOI,
                        date = dateNow,
                        phone = phone,
                        imgurl = "",
                        postion = postion
                    });

                    int NewID = int.Parse(NextId) + 1;
                    NextId = NewID.ToString();
                }
                _context.AddRange(newMessage);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public virtual async Task<string> GetNextMessageID()
        {
            var messageObj = await _context.CcfmessagesRes
                                        .OrderByDescending(m => m.id)
                                        .FirstOrDefaultAsync();

            if (messageObj == null)
            {
                return "1000000";
            }
            var nextId = int.Parse(messageObj.id) + 1;
            return nextId.ToString();
        }
    }
}
