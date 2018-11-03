using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ThePriceBot.Dialogs
{
    [Serializable]
    public class HeroCardDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        public async virtual Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            var welcomeMessage = context.MakeMessage();
            welcomeMessage.Text = "Welcome to bot Hero Card Demo";

            await context.PostAsync(welcomeMessage);

            await this.DisplayHeroCard(context);
        }

        public async Task DisplayHeroCard(IDialogContext context)
        {

            var replyMessage = context.MakeMessage();
            Attachment attachment = GetProfileHeroCard(); ;
            replyMessage.Attachments = new List<Attachment> { attachment };
            await context.PostAsync(replyMessage);
            context.Done((object)null);
        }

        private static Attachment GetProfileHeroCard()
        {
            var heroCard = new HeroCard
            {
                // title of the card  
                Title = "Chris Wang",
                //subtitle of the card  
                Subtitle = "Microsoft vendor support developer",
                // navigate to page , while tab on card  
                Tap = new CardAction(ActionTypes.OpenUrl, "Learn More", value: "https://www.cnblogs.com/TheMiao/"),
                //Detail Text  
                Text = "I am a Software Developer living in Shanghai, with experience working on new and innovative technologies including Cross-Platform Applications, SharePoint and Graph API",
                // list of  Large Image  
                Images = new List<CardImage> { new CardImage("https://media.licdn.com/dms/image/C5103AQE1l8o6VCIGEw/profile-displayphoto-shrink_200_200/0?e=1546473600&v=beta&t=FXE_A3F8uF_aDI282jvzQ4QAhWFennj7DjUAgtJCznc") },
                // list of buttons   
                Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "Learn More", value: "https://www.linkedin.com/in/chrismiaowang/"), new CardAction(ActionTypes.OpenUrl, "GitHub", value: "https://github.com/TheMiao/MicrosoftBotFramework")}
            };

            return heroCard.ToAttachment();
        }



    }
}