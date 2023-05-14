using chat.Data;
using chat.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography.Pkcs;

namespace chat.Controllers
{

    [Authorize]
    public class ChatController : Controller
    {
        private readonly ChatContext context;

        public ChatController(ChatContext context)
        {
            this.context = context;
        }

        public IActionResult Index(int? Id)
        {
            var LoggedInUser = GetUserDetail();
            TempData["LUser"] = LoggedInUser;

            var users = context.TblUser.ToList();

            if (Id == null)
            {
                Id = LoggedInUser.UserId;
            }

            var SelectedUser = context.TblUser.FirstOrDefault(x => x.UserId == Id);
            TempData["SelectedUser"] = SelectedUser;

            var SelectedUserChat = context.TblChat.Where(x => x.SenderId == LoggedInUser.UserId && x.ReceiverId == Id || x.SenderId == Id && x.ReceiverId == LoggedInUser.UserId).ToList();
            TempData["SelectedUserChat"] = SelectedUserChat;

            return View(users);
        }






        [HttpPost("/chat/sendmessage")]
        public async Task<IActionResult> SendMessage(string? messageData, int rId)
        {
            var memberId = GetUserDetail();

            if (messageData != null)
            {

                var chat = new TblChat()
                {
                    Message = messageData,
                    SenderId = memberId.UserId,
                    ReceiverId = rId,
                    MediaType = "",
                    MediaUrl = ""
                };

                context.TblChat.Add(chat);
                var result = await context.SaveChangesAsync();

            }


            var SelectedUser = context.TblUser.FirstOrDefault(x => x.UserId == rId);

            var SelectedUserChat = context.TblChat.Where(x => x.SenderId == memberId.UserId && x.ReceiverId == rId || x.SenderId == rId && x.ReceiverId == memberId.UserId).ToList();




            string content = "";

            foreach (var chatString in SelectedUserChat)
            {
                if (chatString.SenderId == memberId.UserId)
                {
                    content = content + " <div class='media mb-4 justify-content-end align-items-start'> <div class='message-sent'> <div> <p class='mb-2'>" +
                                        chatString.Message +
                                   " </p> <span class='fs-14'>" + chatString.MessageTime.ToString("hh:mm tt") + "</span> </div> </div> <div class='dz-media ms-2'> <img src='/public/user.png" + "' alt=''> </div>  </div>";

                }
                else
                {
                    content = content + "<div class='media my-4'> <div class='dz-media'><img src = '/public/user.png' alt=''> </div> <div class='message-received w-auto'> <div> <p class='mb-2'> " +
                        chatString.Message +
                        "</p> <span class='fs-14'>" + chatString.MessageTime.ToString("hh:mm tt") + "</span> </div>  </div> </div>";

                }
            }


            return Json(new { success = true, content = content });



        }







        protected TblUser GetUserDetail()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = context.TblUser.FirstOrDefault(x => x.UserId == Int16.Parse(userId));

            return user;
        }





    }
}


