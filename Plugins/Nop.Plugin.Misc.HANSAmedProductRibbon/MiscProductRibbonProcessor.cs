using System;
using System.Data;
using System.Data.SqlClient;
using Nop.Core;
using Nop.Services.Configuration;
using Nop.Services.Logging;
using Nop.Services.Plugins;
using Nop.Data;
using System.Xml;
using Nop.Web.Framework.Menu;
using Microsoft.AspNetCore.Routing;
using Nop.Services.Localization;

namespace Nop.Plugin.Misc.HANSAmedProductRibbon
{
    public class MiscProductRibbonProcessor : BasePlugin, IAdminMenuPlugin
    {
        #region Fields
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly ILogger _logger;

        private readonly ILocalizationService _localizationService;
        #endregion

        #region Ctor

        public MiscProductRibbonProcessor(
           ISettingService settingService,
           IWebHelper webHelper,
           ILogger logger,
           ILocalizationService localizationService)
        {
            _settingService = settingService;
            _webHelper = webHelper;
            _logger = logger;
            _localizationService = localizationService;
        }

        #endregion


        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            // Controller Name
            //return $"{_webHelper.GetStoreLocation()}Admin/MiscProductRibbon/Configure";
            return $"{_webHelper.GetStoreLocation()}Admin/ProductRibbons/Setting";

        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            //Default Settings
            var settings = new MiscProductRibbonSettings
            {
                Enabled = true,
                ProductBoxSelector = ".product-item, .item-holder",
                ProductBoxPictureContainerSelector = ".picture, .item-picture",
                ProductPagePictureParentContainerSelector = ".product-essential",
                ProductPageBigPictureContainerSelector = ".picture"
            };
            _settingService.SaveSetting(settings);
            base.Install();
        }
        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //_settingService.DeleteSetting<SAPOrderSettings>();
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
                SystemName = "HANSAmedProductRibbon.Configure",
                Title ="Setting", // _localizationService.GetResource("Hansamed.Menu.Configure"),
                ControllerName = "ProductRibbons",
                ActionName = "Setting",
                Visible = true,
                IconClass = "fa fa fa-dot-circle-o",
                RouteValues = new RouteValueDictionary() { { "area", "Admin" } }
            };

            var ribbonPictures = new SiteMapNode()
            {
                SystemName = "HANSAmedProductRibbon.ribbonPictures",
                Title = "Ribbon Images", 
                ControllerName = "ProductRibbons",
                ActionName = "RibbonImage",
                Visible = true,
                IconClass = "fa fa fa-dot-circle-o",
                RouteValues = new RouteValueDictionary() { { "area", "Admin" } }
            };

            var productRibbons = new SiteMapNode()
            {
                SystemName = "HANSAmedProductRibbon.productRibbons",
                Title = "Product Ribbons",
                ControllerName = "ProductRibbons",
                ActionName = "ProductRibbonList",
                Visible = true,
                IconClass = "fa fa fa-dot-circle-o",
                RouteValues = new RouteValueDictionary() { { "area", "Admin" } }
            };

            var sapRoot = new SiteMapNode()
            {
                SystemName = "HANSAmedProductRibbon",
                Title = _localizationService.GetResource("Hansamed.RootMenu"),
                Visible = true,
                IconClass = "fa fa-cube",
                RouteValues = new RouteValueDictionary() { { "area", "Admin" } }
            };
            sapRoot.ChildNodes.Add(configure);
            sapRoot.ChildNodes.Add(ribbonPictures);
            sapRoot.ChildNodes.Add(productRibbons);

            rootNode.ChildNodes.Add(sapRoot);
           
        }



        #region Database CAll
        private string GetPostXMLPayment(int PaymentId)
        {
            var datasettings = DataSettingsManager.LoadSettings();
            var connstring = datasettings.ConnectionString;

            var conn = new SqlConnection(connstring);
            SqlDataAdapter da = new SqlDataAdapter();
            conn.Open();
            var cmd = new SqlCommand("SELECT SAPPaymentPost FROM PaymentInvoice where id=" + PaymentId + "", conn);

            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;

            string postxml = "";
            try
            {
                using (XmlReader reader = cmd.ExecuteXmlReader())
                {
                    while (reader.Read())
                    {
                        postxml = reader.ReadOuterXml();
                    }
                }

                return postxml;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                da.Dispose();
                conn.Close();
            }
        }
        private string GetEmailFromPaymentInvoice(int PaymentId)
        {
            var datasettings = DataSettingsManager.LoadSettings();
            var connstring = datasettings.ConnectionString;

            var conn = new SqlConnection(connstring);
            SqlDataAdapter da = new SqlDataAdapter();
            conn.Open();
            var cmd = new SqlCommand("SELECT Email FROM PaymentInvoice where id=" + PaymentId + "", conn);

            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;

            da = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            try
            {
                return dt.Rows[0]["Email"].ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                da.Dispose();
                conn.Close();
            }
        }

        // Update IsPostToSAP column in table PaymentInvoice 
        private void UpdatePaymentInvoiceSuccess(int PaymentId)
        {
            var datasettings = DataSettingsManager.LoadSettings();
            var connstring = datasettings.ConnectionString;

            var conn = new SqlConnection(connstring);
            SqlDataAdapter da = new SqlDataAdapter();

            try
            {
                conn.Open();
                var cmd = new SqlCommand("Update PaymentInvoice SET IsPostToSAP = 1 where Id = " + PaymentId + "", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                var retval = cmd.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                _logger.Error(exc.Message, exc);
            }
            finally
            {
                da.Dispose();
                conn.Close();
            }
        }
        private void UpdatePaymentInvoiceFail(int PaymentId)
        {
            var datasettings = DataSettingsManager.LoadSettings();
            var connstring = datasettings.ConnectionString;

            var conn = new SqlConnection(connstring);
            SqlDataAdapter da = new SqlDataAdapter();

            try
            {
                conn.Open();
                var cmd = new SqlCommand("Update PaymentInvoice SET IsPostToSAP = 0 where Id = " + PaymentId + "", conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                var retval = cmd.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                _logger.Error(exc.Message, exc);
            }
            finally
            {
                da.Dispose();
                conn.Close();
            }
        }
        #endregion
  
    }
}
