using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ThePriceBot.Dialogs
{
    [Serializable]
    public class BaseDialog : IDialog<object>
    {
        public BaseDialog()
        {

        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected virtual async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected virtual async Task ShowOption(IDialogContext context)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var welcomeMessage = context.MakeMessage();
        }


#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task StartAsync(IDialogContext context)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {

        }
    }
}