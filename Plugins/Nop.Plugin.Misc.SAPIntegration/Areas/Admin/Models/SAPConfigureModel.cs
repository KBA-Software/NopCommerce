using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.SAPIntegration.Areas.Admin.Models
{
    public class SAPConfigureModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("SAPIntegration.SAPConfigure.SapAPI")]
        public string SapAPI { get; set; }
        public bool SapAPI_OverrideForStore { get; set; }

        [NopResourceDisplayName("SAPIntegration.SAPConfigure.UserId")]
        public string UserId { get; set; }
        public bool UserId_OverrideForStore { get; set; }

        [NopResourceDisplayName("SAPIntegration.SAPConfigure.Password")]
        public string Password { get; set; }
        public bool Password_OverrideForStore { get; set; }
    }
}
