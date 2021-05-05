using Nop.Web.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Plugin.Misc.SAPIntegration.Areas.Admin.Models;

namespace Nop.Plugin.Misc.SAPIntegration.Areas.Admin.Controllers
{
    public class SAPConfigureController : BaseAdminController
    {
        #region Fields
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        #endregion

        #region Cor
        public SAPConfigureController(ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService,
            ISettingService settingService,
            IStoreContext storeContext)
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _settingService = settingService;
            _storeContext = storeContext;
        }
        #endregion

        #region Methods

        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var sapSettings = _settingService.LoadSetting<SAPSettings>(storeScope);
            var model = new SAPConfigureModel
            {
                SapAPI = sapSettings.SapAPI,
                UserId = sapSettings.UserId,
                Password = sapSettings.Password,
                ActiveStoreScopeConfiguration = storeScope
            };

            if (storeScope > 0)
            {
                model.SapAPI_OverrideForStore = _settingService.SettingExists(sapSettings, x => x.SapAPI, storeScope);
                model.UserId_OverrideForStore = _settingService.SettingExists(sapSettings, x => x.UserId, storeScope);
                model.Password_OverrideForStore = _settingService.SettingExists(sapSettings, x => x.Password, storeScope);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Configure(SAPConfigureModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var sapSettings = _settingService.LoadSetting<SAPSettings>(storeScope);

            sapSettings.SapAPI = model.SapAPI;
            sapSettings.UserId = model.UserId;
            sapSettings.Password = model.Password;
            
            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            _settingService.SaveSettingOverridablePerStore(sapSettings, x => x.SapAPI, model.SapAPI_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(sapSettings, x => x.UserId, model.UserId_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(sapSettings, x => x.Password, model.Password_OverrideForStore, storeScope, false);
            
            //now clear settings cache
            _settingService.ClearCache();

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));
            return Configure();
        }
        #endregion
    }
}
