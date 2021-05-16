using Nop.Core;

namespace Nop.Plugin.Misc.HANSAmedProductRibbon.Areas.Admin.Domain
{
    public class RibbonPicture : BaseEntity
    {
        //public int id { get; set; }
        public int PictureWidth { get; set; }
        public int PictureHeight { get; set; }
        public int PictureId { get; set; }
        public string PictureUrl { get; set; }
        public int DisplayOrder { get; set; }
    }
}
