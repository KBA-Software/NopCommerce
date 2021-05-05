using Nop.Core.Configuration;

namespace Nop.Plugin.Misc.SAPIntegration
{
    public class SAPSettings: ISettings
    {
        public string SapAPI { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
    }
}
