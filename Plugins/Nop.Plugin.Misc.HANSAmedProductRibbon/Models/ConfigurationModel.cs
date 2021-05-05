using Nop.Web.Framework.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Nop.Plugin.Misc.HANSAmedProductRibbon.Models
{
    /// <summary>
    /// Represents a configuration model
    /// </summary>
    public class ConfigurationModel
    {
        #region Ctor

        public ConfigurationModel()
        {
            PictureList = new List<RibbonPictureModel>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Plugins.Misc.SAPPayment.Enabled")]
        public bool Enabled { get; set; }

        [NopResourceDisplayName("Plugins.Misc.SAPPayment.DebugMode")]
        public bool DebugMode { get; set; }

        public List<RibbonPictureModel> PictureList { get; set; }


        //[NopResourceDisplayName("Plugins.Misc.SAPPayment.PostURL")]
        //public string PostURL { get; set; }

        //[NopResourceDisplayName("Plugins.Misc.SAPPayment.User")]
        //public string User { get; set; }

        //[NopResourceDisplayName("Plugins.Misc.SAPPayment.Password")]
        //public string Password { get; set; }

        //[NopResourceDisplayName("Plugins.Misc.SAPPayment.RepostID")]
        //public int RepostID { get; set; }

        //[NopResourceDisplayName("Plugins.Misc.SAPPayment.UseAsynchPosting")]
        //public bool UseAsynchPosting { get; set; }

        ///// <summary>
        ///// Address(s) to send error email
        ///// </summary>
        //[NopResourceDisplayName("Plugins.Misc.SAPPayment.ErrorEmailAddress")]
        //public string ErrorEmailAddress { get; set; }

        #endregion
    }
}