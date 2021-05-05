using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Plugin.Misc.HANSAmedProductRibbon.Models;
using System;
using Nop.Services.Catalog;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using Nop.Data;
using Nop.Plugin.Misc.HANSAmedProductRibbon.Factories;
using Nop.Plugin.Misc.HANSAmedProductRibbon.Services;
using Nop.Web.Framework.Mvc;
using Nop.Services.Media;

namespace Nop.Plugin.Misc.HANSAmedProductRibbon.Controllers
{

    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public class MiscProductRibbonController : BasePluginController
    {
        #region Fields
        private readonly IPermissionService _permissionService;
        private readonly IProductRibbonModelFactory _productRibbonModelFactory;
        private readonly IProductRibbonService _productRibbonService;
        private readonly IPictureService _pictureService;

        #endregion

        #region Ctor

        public MiscProductRibbonController(
            IProductRibbonModelFactory productRibbonModelFactory,
            IProductRibbonService productRibbonService,
            IPermissionService permissionService,
            IPictureService pictureService)
        {
   
            _productRibbonModelFactory = productRibbonModelFactory;
            _productRibbonService = productRibbonService;
            _pictureService = pictureService;
            _permissionService = permissionService;
        }

        #endregion
        public IActionResult Configure()
        {

            if (!_permissionService.Authorize(StandardPermissionProvider.AccessAdminPanel))
                return AccessDeniedView();

            //List<RibbonPictureModel> pictures = new List<RibbonPictureModel>();
            //pictures = GetRibbonPictures();

            //if (!pictures.Any())
            //    return Content("No Images can be loaded");

            //var model = new ConfigurationModel();
            //model.PictureList = pictures;

            //prepare model
            var model = _productRibbonModelFactory.PrepareProductRibbonSearchModel(new RibbonPictureSearchModel());
            return View("~/Plugins/Misc.HANSAmedProductRibbon/Views/Configure.cshtml", model);
       

            //if (!_permissionService.Authorize(StandardPermissionProvider.ManagePaymentMethods))
            //    return AccessDeniedView();

            ////load settings for a chosen store scope
            //var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            //var setProductRibbonSettings = _settingService.LoadSetting<MiscProductRibbonSettings>(storeScope);

            //var model = new ConfigurationModel
            //{
            //    Enabled = setProductRibbonSettings.Enabled,
            //    DebugMode = setProductRibbonSettings.DebugMode,
            //};

            //if (storeScope <= 0)
            //    return View("~/Plugins/Misc.HANSAmedProductRibbon/Views/Configure.cshtml", model);

            //return View("~/Plugins/Misc.HANSAmedProductRibbon/Views/Configure.cshtml", model);





        }

        //[HttpPost, ActionName("ConfigurePost")]
        //[FormValueRequired("save")]
        //public IActionResult ConfigurePost(ConfigurationModel model)
        //{
        //    //   if (!ModelState.IsValid)
        //    //     return Configure();

        //   var storeScope = _storeContext.ActiveStoreScopeConfiguration;

        //    var setProductRibbonSettings = new MiscProductRibbonSettings
        //    {
        //        Enabled = model.Enabled,
        //        DebugMode = model.DebugMode,
        //    };

        //    _settingService.SaveSetting(setProductRibbonSettings, storeScope);
        //    _settingService.ClearCache();

        //    _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

        //    return Configure();
        //}

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
            var picture= _pictureService.GetPictureById(Id);

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
                    PictureUrl ="",
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
    }
}
