using Nop.Web.Framework.Models;
using System;

namespace Nop.Plugin.Misc.HANSAmedProductRibbon.Areas.Admin.Models
{
    public class ProductRibbonModel: BaseNopEntityModel
    {
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public bool StopAddingRibbonsAftherThisOneIsAdded { get; set; }
        public int Priority { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string LimitedToStores { get; set; }

    }
}
