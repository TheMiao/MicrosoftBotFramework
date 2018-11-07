using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Security.Claims;
using System.Web;
using System.Threading.Tasks;

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

        public async Task RequestUserLogin(IDialogContext context)
        {
            Request.GetOwinContext().Authentication.Challenge( new AuthenticationProperties { RedirectUri = "/" }, OpenIdConnectAuthenticationDefaults.AuthenticationType);
        }

        protected override Task ShowOption(IDialogContext context)
        {
            return base.ShowOption(context);
        }
    }
}