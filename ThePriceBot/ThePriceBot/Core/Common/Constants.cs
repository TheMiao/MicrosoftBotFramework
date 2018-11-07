using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThePriceBot.Common
{
    public static class Constants
    {
        public static class Function
        {
            public static string Office365 = "Office 365";
            public static string FoodNearMe = "Food Near Me";
        }

        public class Messages
        {
            public static string WelcomeMessage = "Welcome to use Bot, please select following option. You can also type help to learn how to use the bot";
            public static string PickOption = "Please tell me what you want me to do";
            public static string WhosMyDad = "Who's my dad";
            public static string GenericError = "Sorry I'm running into issue, please try me later";

            public static string[] HelpLines = { "Send me a greeting to get started.",
            "Search by cuisine: will search for restaurants based on the type of cuisine closest to your location.",
            "Search by location: will search for restaurants around a selected location.",
            "Settings: you can setup a favourite location and/or type of cuisine for quick searches" };
        }

        public class PromptDialogChoice
        {
            //WelcomeDialog
            public const string SearchByCuisine = "By cuisine";
            public const string SearchByLocation = "By location";
            public const string Settings = "Settings";
            public const string SetFavouriteLocation = "Set fav location";
            public const string SetFavouriteCuisine = "Set fav cuisine";
            public const string DeleteFavouriteLocation = "Delete fav location";
            public const string DeleteFavouriteCuisine = "Delete fav cuisine";
            public const string DeleteAllSettings = "Delete all settings";
            public const string Cancel = "Cancel";
            public const string ExitSettings = "Exit settings";
            public const string ShowMoreResults = "Show more results";
            public const string Back = "Back";
            public const string Search = "Search";
            public const string GoBack = "Go back";
            public const string More = "More";
        }
    }

}