using Nop.Core.Configuration;

namespace Nop.Plugin.Misc.HANSAmedProductRibbon
{
    public class MiscProductRibbonSettings : ISettings
    {
        public bool Enabled { get; set; }
        public string ProductBoxSelector { get; set; }
        public string ProductBoxPictureContainerSelector { get; set; }
        public string ProductPagePictureParentContainerSelector { get; set; }
        public string ProductPageBigPictureContainerSelector { get; set; }

    }
}
