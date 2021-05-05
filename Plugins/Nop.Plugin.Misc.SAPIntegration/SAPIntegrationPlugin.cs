using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.Routing;
using Nop.Core.Domain.Localization;
using Nop.Core.Infrastructure;
using Nop.Data;
using Nop.Services.Common;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Misc.SAPIntegration
{
    public class SAPIntegrationPlugin : BasePlugin, IMiscPlugin, IAdminMenuPlugin
    {
        #region Fields
        private readonly ILocalizationService _localizationService;
        private readonly INopFileProvider _fileProvider;
        private readonly IRepository<Language> _languageRepository;
        #endregion

        #region Ctor
        public SAPIntegrationPlugin(ILocalizationService localizationService,
            INopFileProvider fileProvider,
            IRepository<Language> languageRepository)
        {
            _localizationService = localizationService;
            _fileProvider = fileProvider;
            _languageRepository = languageRepository;
        }
        #endregion

        #region Utilities
        /// <summary>
        ///Import Resource string from xml and save
        /// </summary>
        protected virtual void InstallLocaleResources()
        {
            //'English' language
            var language = _languageRepository.Table.FirstOrDefault(l => l.Name == "English");

            //save resources
            foreach (var filePath in Directory.EnumerateFiles(_fileProvider.MapPath("~/Plugins/Misc.SAPIntegration/Localization/ResourceString"),
                "ResourceStringEn.xml", SearchOption.TopDirectoryOnly))
            {
                var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
                using (var streamReader = new StreamReader(filePath))
                {
                    localizationService.ImportResourcesFromXml(language, streamReader);
                }
            }

        }

        ///<summry>
        ///Delete Resource String
        ///</summry>
        protected virtual void DeleteLocalResources()
        {
            var file = Path.Combine(_fileProvider.MapPath("~/Plugins/Misc.SAPIntegration/Localization/ResourceString"), "ResourceStringEn.xml");
            var languageResourceNames = from name in XDocument.Load(file).Document.Descendants("LocaleResource")
                                        select name.Attribute("Name").Value;

            foreach (var item in languageResourceNames)
            {
                _localizationService.DeletePluginLocaleResource(item);
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            //Insert Local resource
            InstallLocaleResources();

            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //Delete Local resource
            DeleteLocalResources();

            base.Uninstall();
        }

        /// <summary>
        /// Add menu in admin area base on condition if GBS is available then add chield menu in that or create GBS menu also
        /// </summary>
        /// <param name="rootNode">Root menu of admin area</param>
        public void ManageSiteMap(SiteMapNode rootNode)
        {
            var configure = new SiteMapNode()
            {
                SystemName = "SAPIntegration.Configure",
                Title = _localizationService.GetResource("SAPIntegration.Menu.Configure"),
                ControllerName = "SAPConfigure",
                ActionName = "Configure",
                Visible = true,
                IconClass = "fa fa fa-dot-circle-o",
                RouteValues = new RouteValueDictionary() { { "area", "Admin" } }
            };

            var sapRoot = new SiteMapNode()
            {
                SystemName = "SAPIntegration",
                Title = _localizationService.GetResource("SAPIntegration.RootMenu"),
                Visible = true,
                IconClass = "fa fa-cube",
                RouteValues = new RouteValueDictionary() { { "area", "Admin" } }
            };
            sapRoot.ChildNodes.Add(configure);

            rootNode.ChildNodes.Add(sapRoot);
        }

        #endregion
    }
}
