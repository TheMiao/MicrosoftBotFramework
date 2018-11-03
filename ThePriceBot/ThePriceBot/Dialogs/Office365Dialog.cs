using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ThePriceBot.Dialogs
{
    [Serializable]
    public class Office365Dialog : BaseDialog, IDialog<object>
    {
        public Office365Dialog()
        {

        }

        public async override Task StartAsync(IDialogContext context)
        {
            await MessageReceivedAsync(context);
        }

        public async Task MessageReceivedAsync(IDialogContext context)
        {
            await context.PostAsync("This is Office 365 Dialog");
        }

        protected override Task ShowOption(IDialogContext context)
        {
            return base.ShowOption(context);
        }
    }
}