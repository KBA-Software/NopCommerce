using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Nop.Plugin.Misc.HANSAmedProductRibbon.Areas.Admin.Models
{
    /// <summary>
    /// Represents a configuration model
    /// </summary>
    public class ConfigurationModel
    {
        #region Ctor

        public ConfigurationModel()
        {
 
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Plugins.Misc.HansaProductRibbon.Enabled")]
        public bool Enabled { get; set; }
 
        [NopResourceDisplayName("Plugins.Misc.HansaProductRibbon.PBS")]
        public string ProductBoxSelector { get; set; }

        [NopResourceDisplayName("Plugins.Misc.HansaProductRibbon.PBPCS")]
        public string ProductBoxPictureContainerSelector { get; set; }

        [NopResourceDisplayName("Plugins.Misc.HansaProductRibbon.PPCS")]
        public string ProductPagePictureParentContainerSelector	 { get; set; }

        [NopResourceDisplayName("Plugins.Misc.HansaProductRibbon.PPBCS")]
        public string ProductPageBigPictureContainerSelector { get; set; }


   

        #endregion
    }
}