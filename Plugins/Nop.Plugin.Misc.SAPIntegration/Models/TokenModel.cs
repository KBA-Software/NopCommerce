using Newtonsoft.Json;

namespace Nop.Plugin.Misc.SAPIntegration.Models
{
    public class TokenModel
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
