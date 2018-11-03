using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace ThePriceBot.Dialogs
{
    [Serializable]
    public class HelpDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }


        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var activity = await result as Activity;

                // calculate something for us to return
                int length = (activity.Text ?? string.Empty).Length;

                // return our reply to the user
                await context.PostAsync($"Morning {activity.Name}, how can I help you today?");

                context.Wait(MessageReceivedAsync);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RootDialog MessageReceivedAsync through exception: {ex.Message}");
            }
            finally
            {
                context.Done((object)null);
            }
        }
    }
}