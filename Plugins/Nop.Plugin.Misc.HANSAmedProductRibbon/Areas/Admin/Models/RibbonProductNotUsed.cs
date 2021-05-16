using Nop.Web.Framework.Models;
using System.Collections.Generic;

namespace Nop.Plugin.Misc.HANSAmedProductRibbon.Areas.Admin.Models
{
    public class RibbonProductNotUsed : BaseNopEntityModel
    {

       public RibbonProductNotUsed()
       {
            ProductPictureSearchModel = new RibbonPictureSearchModel();
            RibbonPictureModels = new List<RibbonPictureModel>();
       }


        // RibbonPictureModel This is link to RibbonPictureListModel
        public IList<RibbonPictureModel> RibbonPictureModels { get; set; }
        public RibbonPictureSearchModel ProductPictureSearchModel { get; set; }


    }
}
