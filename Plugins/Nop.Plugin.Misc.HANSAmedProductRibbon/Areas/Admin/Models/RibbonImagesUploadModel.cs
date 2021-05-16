using Nop.Web.Framework.Models;
using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Nop.Plugin.Misc.HANSAmedProductRibbon.Areas.Admin.Models
{
    public partial class RibbonImagesUploadModel: BaseNopEntityModel
    {

        public RibbonImagesUploadModel()
        {
            ProductPictureSearchModel = new RibbonPictureSearchModel();
            RibbonPictureModels = new List<RibbonPictureModel>();
        }

        [UIHint("Picture")]
        [NopResourceDisplayName("Admin.Catalog.Products.Pictures.Fields.Picture")]
        public int PictureId { get; set; }

        //[NopResourceDisplayName("Admin.Catalog.Products.Pictures.Fields.Picture")]
        //public string PictureUrl { get; set; }

        //[NopResourceDisplayName("Admin.Catalog.Products.Pictures.Fields.DisplayOrder")]
        //public int DisplayOrder { get; set; }

        //[NopResourceDisplayName("Admin.Catalog.Products.Pictures.Fields.OverrideAltAttribute")]
        //public string OverrideAltAttribute { get; set; }

        //[NopResourceDisplayName("Admin.Catalog.Products.Pictures.Fields.OverrideTitleAttribute")]
        //public string OverrideTitleAttribute { get; set; }



        // RibbonPictureModel This is link to RibbonPictureListModel
        public IList<RibbonPictureModel> RibbonPictureModels { get; set; }
        public RibbonPictureSearchModel ProductPictureSearchModel { get; set; }


    }
}
