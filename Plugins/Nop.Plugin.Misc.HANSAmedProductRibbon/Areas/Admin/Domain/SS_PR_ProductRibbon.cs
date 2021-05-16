using Nop.Core;
using System;

namespace Nop.Plugin.Misc.HANSAmedProductRibbon.Areas.Admin.Domain
{
    public class SS_PR_ProductRibbon: BaseEntity
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
