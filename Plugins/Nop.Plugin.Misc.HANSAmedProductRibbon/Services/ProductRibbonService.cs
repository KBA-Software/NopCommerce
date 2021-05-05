using Nop.Core;
using Nop.Core.Caching;
using Nop.Data;
using Nop.Plugin.Misc.HANSAmedProductRibbon.Domain;
using Nop.Services.Caching;
using System;
using System.Linq;

namespace Nop.Plugin.Misc.HANSAmedProductRibbon.Services
{
    public partial class ProductRibbonService: IProductRibbonService
    {
        #region Constants

        /// <summary>
        /// Cache key for pickup points
        /// </summary>
        /// <remarks>
        /// {0} : current store ID
        /// </remarks>
        private readonly CacheKey _ribbonpictureAllKey = new CacheKey("Nop.ribbonpicture.all-{0}");
        private const string RIBBON_PICTURE_PATTERN_KEY = "Nop.ribbonpicture.";

        #endregion

        #region Fields

        private readonly ICacheKeyService _cacheKeyService;
        private readonly IRepository<SS_PR_RibbonPicture> _ribbonPictureRepository;
        private readonly IStaticCacheManager _staticCacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheKeyService">Cache service</param>
        /// <param name="ribbonPictureRepository">Store pickup point repository</param>
        /// <param name="staticCacheManager">Cache manager</param>
        public ProductRibbonService(ICacheKeyService cacheKeyService,
            IRepository<SS_PR_RibbonPicture> ribbonPictureRepository,
            IStaticCacheManager staticCacheManager)
        {
            _cacheKeyService = cacheKeyService;
            _ribbonPictureRepository = ribbonPictureRepository;
            _staticCacheManager = staticCacheManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets all pickup points
        /// </summary>
        /// <param name="storeId">The store identifier; pass 0 to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Pickup points</returns>
        public virtual IPagedList<SS_PR_RibbonPicture> GetAllProductRibbon(int pictureId = 0, int pageIndex = 0, int pageSize = int.MaxValue)
        {

            var query = _ribbonPictureRepository.Table;
            if (pictureId > 0)
            query = query.Where(point => point.PictureId == pictureId || point.PictureId == 0);
            //query = query.OrderBy(point => point.DisplayOrder).ThenBy(point => point.PictureId);
            return new PagedList<SS_PR_RibbonPicture>(query, pageIndex, pageSize);



            //var key = _cacheKeyService.PrepareKeyForShortTermCache(_ribbonpictureAllKey, pictureId);
            //var rez = _staticCacheManager.Get(key, () =>
            //{
            //    var query = _ribbonPictureRepository.Table;

            //    if (pictureId > 0)
            //        query = query.Where(point => point.PictureId == pictureId || point.PictureId == 0);
            //        //query = query.OrderBy(point => point.DisplayOrder).ThenBy(point => point.PictureId);

            //    return query.ToList();
            //});

            //return new PagedList<SS_PR_RibbonPicture>(rez, pageIndex, pageSize);
        }

        /// <summary>
        /// Gets a pickup point
        /// </summary>
        /// <param name="pickupPointId">Pickup point identifier</param>
        /// <returns>Pickup point</returns>
        public virtual SS_PR_RibbonPicture GetProductRibbontById(int pictureId)
        {
            if (pictureId == 0)
                return null;

            return _ribbonPictureRepository.GetById(pictureId);
        }

        /// <summary>
        /// Inserts a pickup point
        /// </summary>
        /// <param name="pickupPoint">Pickup point</param>
        public virtual void InsertProductRibbon(SS_PR_RibbonPicture ribbonPicture)
        {
            if (ribbonPicture == null)
                throw new ArgumentNullException(nameof(ribbonPicture));

            _ribbonPictureRepository.Insert(ribbonPicture);
            _staticCacheManager.RemoveByPrefix(RIBBON_PICTURE_PATTERN_KEY);
        }

        /// <summary>
        /// Updates the pickup point
        /// </summary>
        /// <param name="pickupPoint">Pickup point</param>
        public virtual void UpdateProductRibbon(SS_PR_RibbonPicture ribbonPicture)
        {
            if (ribbonPicture == null)
                throw new ArgumentNullException(nameof(ribbonPicture));

            _ribbonPictureRepository.Update(ribbonPicture);
            _staticCacheManager.RemoveByPrefix(RIBBON_PICTURE_PATTERN_KEY);
        }

        /// <summary>
        /// Deletes a pickup point
        /// </summary>
        /// <param name="pickupPoint">Pickup point</param>
        public virtual void DeleteRibbonPicture(SS_PR_RibbonPicture ribbonPicture)
        {
            if (ribbonPicture == null)
                throw new ArgumentNullException(nameof(ribbonPicture));

            _ribbonPictureRepository.Delete(ribbonPicture);
            //_staticCacheManager.RemoveByPrefix(RIBBON_PICTURE_PATTERN_KEY);
            //_staticCacheManager.Remove(_ribbonpictureAllKey);
        }


       

        #endregion
    }
}
