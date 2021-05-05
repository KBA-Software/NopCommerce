using Nop.Core;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.HANSAmedProductRibbon.Models
{
    public partial class RibbonPictureModel : BaseNopEntityModel
    {
        //public int id { get; set; }
        public int PictureWidth { get; set; }
        public int PictureHeight { get; set; }
        public int PictureId { get; set; }
        public string PictureUrl { get; set; }
    }
}
