using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace ThePriceBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                if (string.Equals(activity.Text, "help", StringComparison.CurrentCultureIgnoreCase))
                {
                    //var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                    //foreach (var helpLine in Common.Messages.HelpLines)
                    //{
                    //    var reply = activity.CreateReply(helpLine);
                    //    await connector.Conversations.ReplyToActivityAsync(reply);
                    //}

                    await Conversation.SendAsync(activity, () => new Dialogs.HelpDialog());
                } 
                else if (string.Equals(activity.Text, "hero", StringComparison.CurrentCultureIgnoreCase))
                {
                    await Conversation.SendAsync(activity, () => new Dialogs.HeroCardDialog());
                }
                else
                {
                    await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
                }
            }
            else
            {
                HandleSystemMessage(activity);
                // temp code for test purpose
                //if (!string.IsNullOrEmpty(activity.Name))
                //{
                //    await Conversation.SendAsync(activity, () => new Dialogs.HelpDialog());
                //}
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                message.Name = "Chris";
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}