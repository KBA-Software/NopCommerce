using Nop.Core;
using Nop.Plugin.Misc.HANSAmedProductRibbon.Areas.Admin.Domain;

namespace Nop.Plugin.Misc.HANSAmedProductRibbon.Areas.Admin.Services
{
    /// <summary>
    /// Store pickup point service interface
    /// </summary>
    public partial interface IProductRibbonService
    {
        /// <summary>
        /// Gets all pickup points
        /// </summary>
        /// <param name="pictureId">The store identifier; pass 0 to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Pickup points</returns>
        IPagedList<SS_PR_RibbonPicture> GetAllProductRibbon(int pictureId = 0, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Gets a pickup point
        /// </summary>
        /// <param name="pictureId">Pickup point identifier</param>
        /// <returns>Pickup point</returns>
        SS_PR_RibbonPicture GetProductRibbontById(int pictureId);

        /// <summary>
        /// Inserts a pickup point
        /// </summary>
        /// <param name="rbbonPicture">Pickup point</param>
        void InsertProductRibbon(SS_PR_RibbonPicture rbbonPicture);

        /// <summary>
        /// Updates a pickup point
        /// </summary>
        /// <param name="rbbonPicture">Pickup point</param>
        void UpdateProductRibbon(SS_PR_RibbonPicture rbbonPicture);

        /// <summary>
        /// Deletes a pickup point
        /// </summary>
        /// <param name="rbbonPicture">Pickup point</param>
        void DeleteRibbonPicture(SS_PR_RibbonPicture rbbonPicture);
    }
}
