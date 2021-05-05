using Nop.Web.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Nop.Plugin.Misc.HANSAmedProductRibbon.Areas.Controllers
{
    public class ProductRibbons: BaseAdminController
    {

        public IActionResult Configure()
        {
            return View();

        }

    }
}
