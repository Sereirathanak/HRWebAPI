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
using CCFReSeller;
using System.Globalization;
using System.Web.Http.Cors;
using ccf_re_seller_api.Models;
using ccf_re_seller_api.Modals;

namespace ccf_re_seller_api.Repositories
{
    [EnableCors("*", "*", "*")]
    public class HRRepository
    {
        private readonly IWebHostEnvironment _env;

        private readonly HRContext _context;

        public HRRepository(HRContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        //funtion user login
        public virtual async Task<dynamic> SendNotificationUser(string uid, string bcode, string title, string body, string lcode, string rcode, string status)
        {

            try
            {

                var userLevelC = await _context.ccfUserClass
                                          .Where(u => u.uid == uid)
                                          .Where(u => u.ustatus == "A")
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
                        await SaveMessage(userCList, bcode,  title, body, lcode, rcode, status );

                    }
                }
            }
            catch
            {
                throw;
            }

            // Final Return
            return "";
        }
        //

         public virtual async Task<dynamic> SendNotification(List<string> clientToken, string title, string body)
        {
            var curDate = DateTime.Now.ToString();
            var Mins = UniqueId.CreateRandomId().ToString();
            var appName = title + curDate + Mins;
            var registeredToken = clientToken;
            // Google Firebase
            var googleCredential = _env.ContentRootPath;
            //mac
            //googleCredential = googleCredential + "//ccf-hr-management-da23a-firebase-adminsdk-s5vqr-2d85b13be4.json";

            //window
            googleCredential = googleCredential + "\\ccf-hr-management-da23a-firebase-adminsdk-s5vqr-2d85b13be4.json";

            // I suggest to create by specifying a name for instance while we have multiple projects to setup.
            var app = FirebaseApp.Create(new AppOptions
            {
                ProjectId = "870231910336",
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
        //save message
        public virtual async Task<dynamic> SaveMessage(List<string> userList, string bcode, string title, string body, string lcode, string rcode, string status)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime DOI = DateTime.ParseExact((datetime).Trim(), "yyyy-MM-dd HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"));

            // Save Message to Database
            if (userList.Count() > 0)
            {
                var NextId = await this.GetNextMessageID();
                IList<HRCcfmessage> newMessage = new List<HRCcfmessage>();
                for (int i = 0; i < userList.Count(); i++)
                {
                    newMessage.Add(new HRCcfmessage()
                    {
                        id = NextId.ToString(),
                        ucode = userList[i],
                        bcode = bcode,
                        title = title,
                        body = body,
                        mstatus = 0,
                        createdate = DOI,
                        lcode = lcode,
                        rcode = rcode,
                        imgurl = "",
                        status= status
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
            var messageObj = await _context.hrccfmessages
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
