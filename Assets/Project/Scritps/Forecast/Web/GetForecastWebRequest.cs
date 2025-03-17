using CifkorApp.WebRequest;
using System.Threading;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace CifkorApp.Forecast.Web
{
    public class GetForecastWebRequest : BaseWebRequest<ForecastWebResponse>
    {
        private const string TARGET_URL = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";

        public GetForecastWebRequest(CancellationToken cancellationToken) : base(cancellationToken) { }

        public override UnityWebRequest CreateRequest()
        {
            return UnityWebRequest.Get(TARGET_URL);
        }

        protected override WebRequestResult<ForecastWebResponse> ProcessRequestResult(DownloadHandler downloadHandler)
        {
            string jsonText = downloadHandler.text;
            if (string.IsNullOrEmpty(jsonText))
            {
                return null;
            }

            var response = JsonConvert.DeserializeObject<ForecastWebResponse>(jsonText);
            var jsonReuslt = EWebRequestResultType.OK;
            if(response == null)
            {
                jsonReuslt = EWebRequestResultType.ERROR;
            }

            return new WebRequestResult<ForecastWebResponse>(jsonReuslt, response);
        }
    }
}
