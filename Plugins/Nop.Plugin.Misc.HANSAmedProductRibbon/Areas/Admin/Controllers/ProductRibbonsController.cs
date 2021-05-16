using Nop.Web.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.HANSAmedProductRibbon.Areas.Admin.Models;
using Nop.Services.Security;
using Nop.Plugin.Misc.HANSAmedProductRibbon.Areas.Admin.Factories;
using Nop.Plugin.Misc.HANSAmedProductRibbon.Areas.Admin.Services;
using Nop.Services.Media;
using Nop.Services.Configuration;
using Nop.Core;
using Nop.Services.Messages;
using Nop.Services.Localization;
using Nop.Web.Framework.Controllers;
using System;
using System.Data.SqlClient;
using System.Data;
using Nop.Web.Framework.Mvc;
using Nop.Data;
using System.Collections.Generic;
using System.Linq;
using Nop.Plugin.Misc.HANSAmedProductRibbon.Areas.Admin.Domain;

namespace Nop.Plugin.Misc.HANSAmedProductRibbon.Areas.Controllers
{
    public class ProductRibbonsController: BaseAdminController
    {
        #region Fields
        private readonly IPermissionService _permissionService;
        private readonly IProductRibbonModelFactory _productRibbonModelFactory;
        private readonly IProductRibbonService _productRibbonService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly INotificationService _notificationService;
        private readonly ILocalizationService _localizationService;
        #endregion
        #region Ctor
        public ProductRibbonsController(
            IProductRibbonModelFactory productRibbonModelFactory,
            IProductRibbonService productRibbonService,
            IPermissionService permissionService,
            IPictureService pictureService,
            ISettingService settingService,
            IStoreContext storeContext,
            INotificationService notificationService,
            ILocalizationService localizationService)
        {
            _productRibbonModelFactory = productRibbonModelFactory;
            _productRibbonService = productRibbonService;
            _pictureService = pictureService;
            _permissionService = permissionService;
            _settingService = settingService;
            _storeContext = storeContext;
            _notificationService = notificationService;
            _localizationService = localizationService;
        }

        #endregion

        public IActionResult Setting()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.AccessAdminPanel))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var setProductRibbonSettings = _settingService.LoadSetting<MiscProductRibbonSettings>(storeScope);

            var model = new ConfigurationModel
            {
                Enabled = setProductRibbonSettings.Enabled,
                ProductBoxSelector = setProductRibbonSettings.ProductBoxSelector,
                ProductBoxPictureContainerSelector = setProductRibbonSettings.ProductBoxPictureContainerSelector,
                ProductPagePictureParentContainerSelector = setProductRibbonSettings.ProductPagePictureParentContainerSelector,
                ProductPageBigPictureContainerSelector = setProductRibbonSettings.ProductPagePictureParentContainerSelector
            };

            return View("~/Plugins/Misc.HANSAmedProductRibbon/Areas/Admin/Views/Setting.cshtml", model);
        }

        [HttpPost, ActionName("ConfigurePost")]
        [FormValueRequired("save")]
        public IActionResult ConfigurePost(ConfigurationModel model)
        {
            if (!ModelState.IsValid)
                return Configure();

            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var setProductRibbonSettings = new MiscProductRibbonSettings
            {
                Enabled = model.Enabled,
                ProductBoxSelector = model.ProductBoxSelector,
                ProductBoxPictureContainerSelector = model.ProductBoxPictureContainerSelector,
                ProductPagePictureParentContainerSelector = model.ProductPagePictureParentContainerSelector,
                ProductPageBigPictureContainerSelector = model.ProductPagePictureParentContainerSelector
            };

            _settingService.SaveSetting(setProductRibbonSettings, storeScope);
            _settingService.ClearCache();

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        public IActionResult RibbonImage()
        {
            var model = new RibbonImagesUploadModel();
            return View(model);
        }

        public IActionResult Configure()
        {
            var model = new RibbonImagesUploadModel();
            return View(model);

        }

        public virtual IActionResult ProductPictureAdd(int pictureId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            if (pictureId == 0)
                throw new ArgumentException();

            ////try to get a picture with the specified id
            //var picture = _pictureService.GetPictureById(pictureId)
            //    ?? throw new ArgumentException("No picture found with the specified id");

            var SSRRibbonPicture = new SS_PR_RibbonPicture();
            SSRRibbonPicture.PictureId = pictureId;
            _productRibbonService.InsertProductRibbon(SSRRibbonPicture);

            // Refresh here

            return Json(new { Result = true });
        }


        [HttpPost]
        public IActionResult List(RibbonPictureSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return AccessDeniedDataTablesJson();

            //prepare model
            var model = _productRibbonModelFactory.PrepareProductRibbonListModel(searchModel);

            return Json(model);
        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return AccessDeniedView();

            // delete picture from database
            var productRibbon = _productRibbonService.GetProductRibbontById(Id);
            if (productRibbon == null)
                return RedirectToAction("Configure");

            // delete picture from cloud container
            _productRibbonService.DeleteRibbonPicture(productRibbon);
            var picture = _pictureService.GetPictureById(Id);

            _pictureService.DeletePicture(picture);


            return new NullJsonResult();
        }

        public IActionResult GetPictures()
        {
            try
            {
                List<RibbonPictureModel> pictures = new List<RibbonPictureModel>();
                pictures = GetRibbonPictures();

                return Json(pictures);

            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        private List<RibbonPictureModel> GetRibbonPictures()
        {
            List<RibbonPictureModel> pictures = new List<RibbonPictureModel>();

            var datasettings = DataSettingsManager.LoadSettings();
            var connstring = datasettings.ConnectionString;

            var conn = new SqlConnection(connstring);
            SqlDataAdapter da = new SqlDataAdapter();
            conn.Open();
            var cmd = new SqlCommand("select Id,PictureId from SS_PR_RibbonPicture", conn);

            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;

            try
            {
                da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);

                pictures = (from DataRow dr in dt.Rows
                    select new RibbonPictureModel
                    {
                        //id = Convert.ToInt32(dr["id"]),
                        PictureId = Convert.ToInt32(dr["PictureId"]),
                        PictureUrl = "",
                        PictureHeight = 10,
                        PictureWidth = 10

                    }).ToList();
                return pictures;
            }
            catch (Exception)
            {
                return new List<RibbonPictureModel>();
            }
            finally
            {
                da.Dispose();
                conn.Close();
            }
        }


        #region ProductRibbon
        public IActionResult ProductRibbonList(RibbonPictureSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return AccessDeniedDataTablesJson();

            //prepare model
            var model = _productRibbonModelFactory.PrepareProductRibbonListModel(searchModel);

            return Json(model);


            //var model = new ProductRibbonListModel();
            //return View(model);

        }


        #endregion

    }
}
