using Nop.Plugin.Misc.HANSAmedProductRibbon.Models;
using Nop.Plugin.Misc.HANSAmedProductRibbon.Services;
using Nop.Services.Localization;
using Nop.Services.Stores;
using System;
using System.Linq;
using Nop.Web.Framework.Models.Extensions;
using Nop.Services.Media;
using System.IO;

namespace Nop.Plugin.Misc.HANSAmedProductRibbon.Factories
{
    public class ProductRibbonModelFactory: IProductRibbonModelFactory
    {
        #region Fields
        private readonly IProductRibbonService _productRibbonService;
        private readonly IPictureService _pictureService;

        #endregion

        #region Ctor

        public ProductRibbonModelFactory(
            IProductRibbonService productRibbonService,
            IPictureService pictureService)
        {
            _productRibbonService = productRibbonService;
            _pictureService = pictureService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare store pickup point list model
        /// </summary>
        /// <param name="searchModel">Store pickup point search model</param>
        /// <returns>Store pickup point list model</returns>
        public RibbonPictureListModel PrepareProductRibbonListModel(RibbonPictureSearchModel searchModel)
        {
            var productRibbon = _productRibbonService.GetAllProductRibbon(pageIndex: searchModel.Page - 1,pageSize: searchModel.PageSize);

            var model = new RibbonPictureListModel().PrepareToGrid(searchModel, productRibbon, () =>
            {
                return productRibbon.Select(point =>
                {
                    return new RibbonPictureModel
                    {
                        Id = point.Id,
                        PictureId = point.PictureId,
                        PictureUrl = _pictureService.GetPictureUrl(point.PictureId),
                        PictureHeight = ImageSize(point.PictureId, "height"),
                        PictureWidth = ImageSize(point.PictureId,"width")
                    };
                });
            });
            return model;
        }

        /// <summary>
        /// Prepare store pickup point search model
        /// </summary>
        /// <param name="searchModel">Store pickup point search model</param>
        /// <returns>Store pickup point search model</returns>
        public RibbonPictureSearchModel PrepareProductRibbonSearchModel(RibbonPictureSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        private int ImageSize(int pictureId, string size)
        {
            try
            {
                var imgPath = _pictureService.GetPictureUrl(pictureId);
                var img = DownloadImageFromUrl(imgPath);
                if (size == "width")
                {
                    return img.Width;
                }
                else
                {
                    return img.Height;
                }
            }
            catch (Exception)
            {

                return 0;
            }
        }

        private System.Drawing.Image DownloadImageFromUrl(string imageUrl)
        {
            System.Drawing.Image image = null;
            try
            {
                System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(imageUrl);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Timeout = 1000;

                System.Net.WebResponse webResponse = webRequest.GetResponse();
                System.IO.Stream stream = webResponse.GetResponseStream();
                image = System.Drawing.Image.FromStream(stream);
                webResponse.Close();
            }
            catch (Exception)
            {
                return null;
            }

            return image;
        }

        #endregion
    }
}
