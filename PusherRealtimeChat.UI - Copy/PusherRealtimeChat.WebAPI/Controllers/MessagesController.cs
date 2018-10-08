using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PusherRealtimeChat.WebAPI.Models;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using PusherServer;
using System.Web.Http.Cors;

namespace PusherRealtimeChat.WebAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    public class MessagesController : ApiController
    {
        private static List<ChatMessage> messages =
            new List<ChatMessage>()
            {
                new ChatMessage
                {
                    AuthorTwitterHandle = "Pusher",
                    Text = "Hi there! ?"
                },
                new ChatMessage
                {
                    AuthorTwitterHandle = "Pusher",
                    Text = "Welcome to your chat app"
                }
            };

        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(
                HttpStatusCode.OK,
                messages);
        }

        public HttpResponseMessage Post(ChatMessage message)
        {
            if (message == null || !ModelState.IsValid)
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest,
                    "Invalid input");
            }
            messages.Add(message);

            var pusher = new Pusher(
                 "617667",
                  "ee4e414a35c27f28612f",
                  "d8039c1c356c3769dae5",

                //"YOUR APP ID",
                //"YOUR APP KEY",
                //"YOUR APP SECRET",
                   new PusherOptions
                   {
                       Cluster = "mt1",
                       //Cluster = "YOUR CLUSTER"
                   });
            pusher.TriggerAsync(
                channelName: "messages",
                eventName: "new_message",
                data: new
                {
                    AuthorTwitterHandle = message.AuthorTwitterHandle,
                    Text = message.Text
                });

            return Request.CreateResponse(HttpStatusCode.Created);
        }
    }
}