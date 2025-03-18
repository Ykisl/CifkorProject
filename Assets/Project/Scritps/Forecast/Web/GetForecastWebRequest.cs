using System.Threading;
using UnityEngine.Networking;
using Unity.Plastic.Newtonsoft.Json;
using CifkorApp.WebRequest;

namespace CifkorApp.Forecast.Web
{
    public class GetForecastWebRequest : BaseWebRequest<GetForecastWebResponse>
    {
        private const string TARGET_URL = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";

        public GetForecastWebRequest(CancellationToken cancellationToken) : base(cancellationToken) { }

        public override UnityWebRequest CreateRequest()
        {
            return UnityWebRequest.Get(TARGET_URL);
        }

        protected override WebRequestResult<GetForecastWebResponse> ProcessRequestResult(DownloadHandler downloadHandler)
        {
            string jsonText = downloadHandler.text;
            if (string.IsNullOrEmpty(jsonText))
            {
                return null;
            }

            var response = JsonConvert.DeserializeObject<GetForecastWebResponse>(jsonText);
            var jsonReuslt = EWebRequestResultType.OK;
            if(response == null)
            {
                jsonReuslt = EWebRequestResultType.ERROR;
            }

            return new WebRequestResult<GetForecastWebResponse>(jsonReuslt, response);
        }
    }
}
