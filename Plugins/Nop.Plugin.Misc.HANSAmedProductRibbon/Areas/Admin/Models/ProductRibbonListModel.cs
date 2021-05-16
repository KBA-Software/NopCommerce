using Nop.Web.Framework.Models;
using System.Collections.Generic;

namespace Nop.Plugin.Misc.HANSAmedProductRibbon.Areas.Admin.Models
{
    public class ProductRibbonListModel: BaseNopEntityModel
    {
        
        public ProductRibbonListModel()
        {
            ProductRibbonModels = new List<ProductRibbonModel>();
            ProductPictureSearchModel = new RibbonPictureSearchModel();
        }

        // RibbonPictureModel This is link to RibbonPictureListModel
        public IList<ProductRibbonModel> ProductRibbonModels { get; set; }
        public RibbonPictureSearchModel ProductPictureSearchModel { get; set; }

    }
}
