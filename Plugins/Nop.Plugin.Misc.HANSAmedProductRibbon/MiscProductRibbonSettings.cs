using Nop.Core.Configuration;

namespace Nop.Plugin.Misc.HANSAmedProductRibbon
{
    public class MiscProductRibbonSettings : ISettings
    {
        /// <summary>
        /// Gets or sets a if posting is enabled
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to put in debug mode (verbose logging etc.)
        /// </summary>
        public bool DebugMode { get; set; }

        /// <summary>
        /// Gets or sets a URL to post to
        /// </summary>
        //public string PostURL { get; set; }

        /// <summary>
        /// Gets or sets a URL to post to
        /// </summary>
        //public string NewUserPostURL { get; set; }
        /// <summary>
        /// User name for posting
        /// </summary>
        //public string User { get; set; }

        /// <summary>
        /// User name for posting
        /// </summary>
        //public string Password { get; set; }


        /// <summary>
        /// Turn On Asynchronous posting
        /// </summary>
        /// 
        //public bool UseAsynchPosting { get; set; }
//
        /// <summary>
        /// Address(s) to send error email
        /// </summary>
        //public string ErrorEmailAddress { get; set; }
    }
}
