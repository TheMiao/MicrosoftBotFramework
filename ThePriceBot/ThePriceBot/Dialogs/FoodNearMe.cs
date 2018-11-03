using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace ThePriceBot.Dialogs
{
    [Serializable]
    public class FoodNearMe : BaseDialog, IDialog<object>
    {
        public FoodNearMe()
        {

        }

        public async Task StartAsync(IDialogContext context)
        {

        }

        public async Task MessageReceivedAsync(IDialogContext context)
        {
        }
    }
}