using Nop.Plugin.Misc.SAPIntegration.Models;
using Nop.Services.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace Nop.Plugin.Misc.SAPIntegration.Services
{
    public class SAPIntegrationService : ISAPIntegrationService
    {
        #region Filds
        public SAPSettings _sAPSettings;
        public ILogger _logger;
        #endregion

        #region Ctor
        public SAPIntegrationService(SAPSettings sAPSettings,
            ILogger logger)
        {
            _sAPSettings = sAPSettings;
            _logger = logger;
        }
        #endregion

        #region 

        private string GetToken(int orderId)
        {

            var client = new RestClient($"{_sAPSettings.SapAPI}User/Login?email={_sAPSettings.UserId}&password={_sAPSettings.Password}");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddParameter("text/plain", "", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.IsSuccessful)
            {
                var token = JsonConvert.DeserializeObject<TokenModel>(response.Content);
                return token.Token;
            }
            else
                _logger.InsertLog(Core.Domain.Logging.LogLevel.Information, $"Tokn not generated {orderId}");

            return "";
        }

        public void SyncOrderToSAP(SAPOrderModel model)
        {
            var token = GetToken(model.OrderNumber);

            var client = new RestClient($"{_sAPSettings.SapAPI}User/OrderSubmit");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", JsonConvert.SerializeObject(model), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful)
                _logger.InsertLog(Core.Domain.Logging.LogLevel.Information, $"Order Synced {model.OrderNumber}", response.Content);
            else
                _logger.InsertLog(Core.Domain.Logging.LogLevel.Information, $"Order not Synced {model.OrderNumber}", response.Content);

        }
        #endregion
    }
}
