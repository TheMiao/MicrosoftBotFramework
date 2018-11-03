using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using ThePriceBot.Common;

namespace ThePriceBot.Dialogs
{
    [Serializable]
    public class RootDialog : BaseDialog, IDialog<object>
    {
        public RootDialog()
        {

        }

        public async Task StartAsync(IDialogContext context)
        {
            await MessageReceivedAsync(context);
        }

        public async Task MessageReceivedAsync(IDialogContext context)
        {
            await ShowOption(context);
        }

        protected async override Task ShowOption(IDialogContext context)
        {
            try
            {
                var welcomeMessage = context.MakeMessage();
                var options = new List<string>();
                options.AddRange(new List<string>()
                {
                    Constants.Function.Office365, Constants.Function.FoodNearMe
                });
                welcomeMessage.AddKeyboardCard(Constants.Messages.WelcomeMessage, options);

                // return our reply to the user
                await context.PostAsync(welcomeMessage);

                context.Wait(OnOptionSelected);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RootDialog MessageReceivedAsync through exception: {ex.Message}");
            }
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            try
            {
                var activity = await result;
                if (string.Equals(activity.Text, Constants.PromptDialogChoice.Cancel))
                {
                    context.Done(true);
                    return;
                }
                else
                {
                    // Navigate to different dialog.
                    if (string.Equals(activity.Text, Constants.Function.Office365))
                    {
                        context.Call(new Office365Dialog(), ResumeAfterOptionDialog);
                    }
                    else if (string.Equals(activity.Text, Constants.Function.FoodNearMe))
                    {
                        context.Call(new FoodNearMe(), ResumeAfterOptionDialog);
                    }
                }
            }
            catch (Exception ex)
            {
                await MessageReceivedAsync(context);
                await context.PostAsync(Constants.Messages.GenericError);
                Debug.WriteLine($"onOptionSelected from RootDialog {ex.Message}");
            }

        }

        protected override async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var message = await result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ResumeAfterOptionDialog from RootDialog {ex.Message}");
                await context.PostAsync(Constants.Messages.GenericError);

            }
            finally
            {
                context.Done((object)null);
            }
        }
    }
}