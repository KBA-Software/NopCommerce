using Nop.Plugin.Misc.HANSAmedProductRibbon.Models;

namespace Nop.Plugin.Misc.HANSAmedProductRibbon.Factories
{
    public interface IProductRibbonModelFactory
    {
        /// <summary>
        /// Prepare store pickup point list model
        /// </summary>
        /// <param name="searchModel">Store pickup point search model</param>
        /// <returns>Store pickup point list model</returns>
        RibbonPictureListModel PrepareProductRibbonListModel(RibbonPictureSearchModel searchModel);

        /// <summary>
        /// Prepare store pickup point search model
        /// </summary>
        /// <param name="searchModel">Store pickup point search model</param>
        /// <returns>Store pickup point search model</returns>
        RibbonPictureSearchModel PrepareProductRibbonSearchModel(RibbonPictureSearchModel searchModel);

    }
}
