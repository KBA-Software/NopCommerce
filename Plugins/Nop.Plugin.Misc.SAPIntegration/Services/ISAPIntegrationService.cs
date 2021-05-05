using Nop.Plugin.Misc.SAPIntegration.Models;

namespace Nop.Plugin.Misc.SAPIntegration.Services
{
    public interface ISAPIntegrationService
    {
        void SyncOrderToSAP(SAPOrderModel model);
    }
}
